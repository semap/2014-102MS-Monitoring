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

    public class Program
    {
        private static EthernetENC28J60 eth;
        private static string zeroIpAddress = "0.0.0.0";
        private static int moderateSleep = 500;
        private static string testInternetUri = "http://www.bigfont.ca";
        private static string testLanUri = "http://192.168.1.102:80";
        private static string wcfServiceEndpointUri = "http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/";

        public static void Main()
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

            // Loop until we're connected.
            int i = 0;
            while (eth.NetworkInterface.IPAddress.Equals(zeroIpAddress))
            {
                ++i;
                Debug.Print("Awaiting a non-zero IP address. Loop #" + i.ToString());
                Thread.Sleep(moderateSleep);
            }

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

            // Can we reach the default iis page on the LAN?
            request = HttpWebRequest.Create(testLanUri);
            response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadLine();
                if (result.Length > 0)
                {
                    Debug.Print("Connected to the default iis page on the lan");
                    Debug.Print(result);                
                }
            }

            // Can we connect to the WCF Http Service?
            var isConnectedToWcfService = ConnectToWcfServiceViaHttp();
            if (isConnectedToWcfService)
            {
                Debug.Print("Connected to WCF Service!");
            }
        }

        private static bool ConnectToWcfServiceViaHttp()
        {
            Uri serviceUri = new System.Uri(wcfServiceEndpointUri);
            HttpTransportBindingConfig config = new HttpTransportBindingConfig(serviceUri);
            WS2007HttpBinding binding = new WS2007HttpBinding(config);
            IPersistenceSvcClientProxy proxy = new IPersistenceSvcClientProxy(binding, new Ws.Services.ProtocolVersion11());
            var result = false;
            try
            {
                IsActiveResponse resp = proxy.IsActive(new IsActive());
                result = resp.IsActiveResult;
            }
            catch (Exception e)
            {
                string m = e.Message;
            }

            return result;
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
