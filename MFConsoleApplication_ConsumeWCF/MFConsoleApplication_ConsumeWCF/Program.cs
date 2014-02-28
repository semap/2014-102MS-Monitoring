﻿using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHI.Premium.Net;
using GHI.Hardware.G120;
using Ws.Services.Binding;
using tempuri.org;
using System;
using System.Net;
using System.IO;

namespace MFConsoleApplication_ConsumeWCF
{
    public class Program
    {
        static EthernetENC28J60 eth;

        public static void Main()
        {
            // Initialize FEZ Cobra II built-in Ethernet port
            eth = new EthernetENC28J60(SPI.SPI_module.SPI2, Pin.P1_10, Pin.P2_11, Pin.P1_9, 4000);

            eth.NetworkAddressChanged += eth_NetworkAddressChanged;
            eth.CableConnectivityChanged += eth_CableConnectivityChanged;

            if (!eth.IsOpen)
                eth.Open();

            NetworkInterfaceExtension.AssignNetworkingStackTo(eth);

            if (!eth.NetworkInterface.IsDhcpEnabled)
                eth.NetworkInterface.EnableDhcp();

            // Required to kick DHCP into action
            eth.NetworkInterface.RenewDhcpLease();

            Debug.Print("IP address:" + eth.NetworkInterface.IPAddress);

            while (eth.NetworkInterface.IPAddress.Equals("0.0.0.0"))
            {
                Debug.Print(eth.NetworkInterface.IPAddress);
            }

            // we can reach a page on the internet
            WebRequest request = HttpWebRequest.Create("http://www.bigfont.ca");
            WebResponse response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadLine();
            }

            // we can reach the default iis page on the lan
            request = HttpWebRequest.Create("http://192.168.1.102");
            response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
            }

            // we cannot, though, connect to the WCF Http Service.
            ConnectToWcfServiceViaHttp();
        }

        static void ConnectToWcfServiceViaHttp()
        {            
            WS2007HttpBinding binding =
                new WS2007HttpBinding(new HttpTransportBindingConfig(new System.Uri("http://192.168.1.102:80/YahooWcf/DuperSvc/")));

            IDuperSvcClientProxy proxy = new IDuperSvcClientProxy(binding, new Ws.Services.ProtocolVersion11());
            try
            {
                //GetDataResponse resp = proxy.GetData(new GetData() { value = 123 });
                IsActiveResponse resp = proxy.IsActive(new IsActive());
            }
            catch (Exception e)
            {
                string m = e.Message;
                m += "";
            }
        }

        static void ConnectToWcfServiceViaTcp()
        {
            // todo 
        }

        static void eth_CableConnectivityChanged(object sender, EthernetENC28J60.CableConnectivityEventArgs e)
        {
            Debug.Print("Network cable " + (e.IsConnected ? "Connected" : "Disconnected"));
        }

        // DHCP will assign an IP address to the adapter sometime after startup. 
        // This can be 20 to 30 seconds or more depending on the network.
        static void eth_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("DHCP assigned IP address: " + eth.NetworkInterface.IPAddress);
        }

    }
}
