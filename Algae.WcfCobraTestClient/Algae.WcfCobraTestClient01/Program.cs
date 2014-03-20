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
    using schemas.datacontract.org.Algae.WcfServiceLibrary;
    using tempuri.org;
    using Ws.Services.Binding;

    public class Program
    {
        private static EthernetENC28J60 eth;
        private static int moderateTimespan = 1000;
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
        private static IPersistenceSvcClientProxy _proxy;
        private static IPersistenceSvcClientProxy proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = CreateWcfProxy();
                }
                return _proxy;
            }            
        }

        public static void Main()
        {
            SetupButtonPressEvents();

            InitializeFezCobraIIEthernetPort();

            RepeatedlySendDataToWcfService();
        }

        #region Button Press Events

        private static void FlashLed()
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

        private static void SetupButtonPressEvents()
        {
            lrd0.OnInterrupt += DoFlashing;
            lrd1.OnInterrupt += DoNotDoFlashing;
        }

        #endregion

        private static void InitializeFezCobraIIEthernetPort()
        {
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

        #region Send Data to WCF Service

        private static void RepeatedlySendDataToWcfService()
        {
            doFlashing = true;
            timer = new Timer(TimerCallback_SendSbcData, new object(), 0, 1000);
        }

        private static void TimerCallback_SendSbcData(object stateInfo)
        {
            try
            {                    
                ConnectWcfProxy();
                SendTestDataToWcfServiceViaHttp(proxy);
                FlashLed();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Create a Wcf Proxy
        /// </summary>
        private static IPersistenceSvcClientProxy CreateWcfProxy()
        {
            Uri serviceUri = new System.Uri(wcfServiceEndpointUri);
            HttpTransportBindingConfig config = new HttpTransportBindingConfig(serviceUri);
            WS2007HttpBinding binding = new WS2007HttpBinding(config);
            IPersistenceSvcClientProxy proxy = new IPersistenceSvcClientProxy(binding, new Ws.Services.ProtocolVersion11());
            return proxy;
        }

        /// <summary>
        /// Keep trying to connect to the Wcf service until connected.
        /// </summary>
        private static void ConnectWcfProxy()
        {
            var isConnected = false;
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
        }

        private static void SendTestDataToWcfServiceViaHttp(IPersistenceSvcClientProxy proxy)
        {
            if (proxy.IsConnected(new IsConnected()).IsConnectedResult)
            {
                sendCounter++;

                // this garbage collecting is just for testing.
                // it probably slows things down to do it every loop
                Debug.GC(true);
                GC.WaitForPendingFinalizers();

                Send send = new Send();
                send.data = new schemas.datacontract.org.Algae.WcfServiceLibrary.ArrayOfSbcData();
                send.data.SbcData = new SbcData[] 
                {
                    new SbcData() 
                    {
                        Data = sendCounter.ToString(), 
                        SensorGuid = new Guid().ToString(),
                        Timestamp = DateTime.Now,
                        DataMetric = DataMetric.Celsius,
                        DataType = DataType.Long
                    }
                };
                proxy.Send(send);
            }
        }

        #endregion

        #region Ethernet Events

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

        #endregion

        private static void HandleException(Exception ex)
        {
            Debug.Print(ex.Message);
        }
    }
}
