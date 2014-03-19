//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sprocket Enterprises">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Algae.WcfCobraTestClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using GHI.Hardware.G120;
    using GHI.Premium.Net;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using tempuri.org;
    using Ws.Services.Binding;
    using schemas.datacontract.org.Algae.WcfServiceLibrary;

    public class Program
    {
        private static EthernetENC28J60 eth;
        private static string zeroIpAddress = "0.0.0.0";
        private static int moderateSleep = 500;
        private static string testInternetUri = "http://www.bigfont.ca";
        private static string testLanUri = "http://192.168.1.102:80";
        private static string wcfServiceEndpointUri = "http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/";

        // timer
        // note: this is private static to prevent garbage collection
        // see also http://stackoverflow.com/questions/477351/in-c-where-should-i-keep-my-timers-reference
        private static Timer timer;

        // flash led
        private static OutputPort led1 = new OutputPort(GHI.Hardware.G120.Pin.P1_15, true);
        private static bool doFlashing = false;

        // buttons
        private static InterruptPort lrd1 =
            new InterruptPort(GHI.Hardware.G120.Pin.P0_22, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        private static InterruptPort lrd0 =
            new InterruptPort(GHI.Hardware.G120.Pin.P2_10, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        // wcf proxy
        private static int sendCounter = 0;
        private static IPersistenceSvcClientProxy proxy;

        public static void Main()
        {
            lrd0.OnInterrupt += DoFlashing;
            lrd1.OnInterrupt += DoNotDoFlashing;

            InitializeFezCobraIIEthernetPort();

            LoopUntilWeHaveAnIpAddress();

            TryToReachAnInternetPage();

            TryToReachALanPage();

            TryToConnectToTheWcfService();

            PreventTheThreadFromExiting();
        }

        private static void TimerCallback_SendSbcData(object stateInfo)
        {
            try
            {
                ConnectWcfProxy();
                SendTestDataToWcfServiceViaHttp(proxy);
                Flash();
            }
            catch (Exception)
            {
                int i = 0;
                ++i;
                // Hmm. What do to.
            }
        }

        private static void PreventTheThreadFromExiting()
        {
            doFlashing = true;
            timer = new Timer(TimerCallback_SendSbcData, new object(), 0, 1000);
        }

        private static void TryToConnectToTheWcfService()
        {
            // Can we connect to the WCF Http Service?            
            var isConnectedToWcfService = ConnectWcfProxy();
            if (isConnectedToWcfService)
            {
                Debug.Print("Connected to WCF Service!");
            }
        }

        private static void TryToReachALanPage()
        {
            // Can we reach the default iis page on the LAN?
            WebRequest request = HttpWebRequest.Create(testLanUri);
            WebResponse response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadLine();
                if (result.Length > 0)
                {
                    Debug.Print("Connected to the default iis page on the lan");
                    Debug.Print(result);
                }
            }
        }

        private static void TryToReachAnInternetPage()
        {
            // Can we reach a page on the internet?
            WebRequest request = HttpWebRequest.Create(testInternetUri);
            WebResponse response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadLine();
                if (result.Length > 0)
                {
                    Debug.Print("Connected to a page on the Internet");
                    Debug.Print(result);
                }
            }
        }

        private static void InitializeFezCobraIIEthernetPort()
        {
            // Initialize FEZ Cobra II built-in Ethernet port
            eth = new EthernetENC28J60(SPI.SPI_module.SPI2, Pin.P1_10, Pin.P2_11, Pin.P1_9, 4000);

            eth.NetworkAddressChanged += Eth_NetworkAddressChanged;
            eth.CableConnectivityChanged += Eth_CableConnectivityChanged;

            if (!eth.IsOpen)
            {
                eth.Open();
            }

            NetworkInterfaceExtension.AssignNetworkingStackTo(eth);

            if (!eth.NetworkInterface.IsDhcpEnabled)
            {
                eth.NetworkInterface.EnableDhcp();
            }

            // Required to kick DHCP into action
            eth.NetworkInterface.RenewDhcpLease();
        }

        private static void LoopUntilWeHaveAnIpAddress()
        {
            // Loop until we're connected.
            int i = 0;
            while (eth.NetworkInterface.IPAddress.Equals(zeroIpAddress))
            {
                ++i;
                Debug.Print("Awaiting a non-zero IP address. Loop #" + i.ToString());
                Thread.Sleep(moderateSleep);
            }
        }

        private static void Flash()
        {
            if (doFlashing)
            {
                bool isOn = led1.Read();
                led1.Write(!isOn);
            }
            else
            {
                led1.Write(false);
            }
        }

        private static void DoFlashing(uint data1, uint data2, DateTime time)
        {
            doFlashing = true;
        }

        private static void DoNotDoFlashing(uint data1, uint data2, DateTime time)
        {
            doFlashing = false;
        }

        private static void SendTestDataToWcfServiceViaHttp(IPersistenceSvcClientProxy proxy)
        {
            if (proxy.IsConnected(new IsConnected()).IsConnectedResult)
            {
                sendCounter++;

                Debug.GC(true);
                GC.WaitForPendingFinalizers();

                if (sendCounter > 43)
                {
                    int i = 0;
                    ++i;
                }

                Send send = new Send();
                send.data = new schemas.datacontract.org.Algae.WcfServiceLibrary.ArrayOfSbcData();
                send.data.SbcData = new SbcData[] {
                    new SbcData () {
                        Data = sendCounter.ToString(), 
                        SensorGuid = new Guid().ToString()                        
                    }
                };
                proxy.Send(send);                                
            }
        }

        private static bool ConnectWcfProxy()
        {
            var isConnected = false;
            Uri serviceUri = new System.Uri(wcfServiceEndpointUri);
            HttpTransportBindingConfig config = new HttpTransportBindingConfig(serviceUri);
            WS2007HttpBinding binding = new WS2007HttpBinding(config);

            if (proxy == null)
            { 
                proxy = new IPersistenceSvcClientProxy(binding, new Ws.Services.ProtocolVersion11());
            }

            while (!isConnected)
            {
                try
                {
                    IsConnectedResponse resp = proxy.IsConnected(new IsConnected());
                    isConnected = resp.IsConnectedResult;
                }
                catch (Exception)
                {
                    isConnected = false;
                }
            }

            return isConnected;
        }

        private static void ConnectToWcfServiceViaTcp()
        {
            // todo 
            // This will require the creation of a custom WCF binding.
        }

        private static void Eth_CableConnectivityChanged(object sender, EthernetENC28J60.CableConnectivityEventArgs e)
        {
            Debug.Print("Network cable " + (e.IsConnected ? "Connected" : "Disconnected"));
        }

        // DHCP will assign an IP address to the adapter sometime after startup. 
        // This can be 20 to 30 seconds or more depending on the network.
        private static void Eth_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("DHCP assigned IP address: " + eth.NetworkInterface.IPAddress);
        }
    }
}
