using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GHI.Hardware.G120;
using GHI.Premium.Net;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace MFConsoleApplication3
{
    public class Program
    {
        private static WiFiRS9110 _wifi;
        private static readonly ManualResetEvent mre = new ManualResetEvent(false);

        private const string AdHocHostName = "MyAdHocHost";
        private const string IpAddress = "169.254.0.200";
        private const string SubnetMask = "255.255.0.0";
        private const string GatewayAddress = "169.254.0.1";

        public static void Main()
        {
            PrintMethodNameForDebugging("Main");

            try
            {
                _wifi = CreateWifiDriver();
                OpenTheWifiDriver(_wifi);
                DisconnectTheWifiDriver(_wifi);
                AddEventListenersToTheWifiDriver(_wifi);
                ConfigureIpAddress(_wifi, false);

                new Thread(CreateSocket).Start();
                SetupAdHocHost(_wifi, AdHocHostName);

                Debug.Print("Exit");

                /* --------------------------------------
                 
                 * Open PowerShell and run:
                 
                 netsh interface ip delete arpcache
                 netsh wlan show networks mode=bssid
                 
                 * Then open Network and Sharing Center
                 * Set up a new connection or network
                 * Manually connect to a wireless network
                 * Network name: YOURSSID
                 * Security type: No authentication (Open)
                 * Leave other defaults
                 * Next
                 
                 * Open PowerShell and run:
                 
                 netsh wlan connect ssid=YOURSSID name=PROFILENAME
                 e.g. netsh wlan connect ssid=MyAdHocHost129 name=MyAdHocHost129
                 
                 * "Connection request was completed successfully."
                 * This means that the Desktop Computer is connected to the Cobra WiFi.
                 
                 * However, we haven't been able to send data via TCP.
                 * I.e. the CreateSocket() method never goes past Waiting for client...
                 
                 
                 -------------------------------------- */
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        private static void CreateSocket()
        {
            mre.WaitOne();
            PrintMethodNameForDebugging("CreateSocket");

            if (_wifi.IsLinkConnected)
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress hostIp = IPAddress.Parse(_wifi.NetworkInterface.IPAddress);
                var hostEndPoint = new IPEndPoint(hostIp, 80);
                socket.Bind(hostEndPoint);

                // does this need to wait too?
                socket.Listen(1);
                Debug.Print("Waiting for client...");

                Socket client = socket.Accept();
                var clientEndPoint = client.RemoteEndPoint as IPEndPoint;
                if (clientEndPoint != null)
                {
                    Debug.Print("Connected with " + clientEndPoint.Address + " at port " + clientEndPoint.Port);
                }
            }
        }

        private static void SetupAdHocHost(WiFiRS9110 wifi, string hostName)
        {
            PrintMethodNameForDebugging("SetupAdHocHost");

            hostName += DateTime.Now.Ticks.ToString().Substring(0, 3);
            Debug.Print("hostName:" + hostName);
            wifi.StartAdHocHost(hostName, SecurityMode.Open, "", 10);
        }

        // See more at: https://www.ghielectronics.com/community/forum/topic?id=10374&page=3#sthash.j13kLPi4.dpuf
        private static void ConfigureIpAddress(WiFiRS9110 wifi, bool doDhcp)
        {
            PrintMethodNameForDebugging("ConfigureIpAddress");

            // the ipAddress is apparently finicky
            if (doDhcp)
            {
                // dynamic means that we need a DHCP server on the LAN
                Debug.Print("EnableDhcp");
                if (!wifi.NetworkInterface.IsDhcpEnabled)
                {
                    wifi.NetworkInterface.EnableDhcp();
                }
            }
            else
            {
                // static apparently works well with laptop using MS-Windows as long you use an IP address 169.255.x.x
                Debug.Print("EnableStaticIp");
                wifi.NetworkInterface.EnableStaticIP(IpAddress, SubnetMask, GatewayAddress);
                wifi.NetworkInterface.EnableStaticDns(new string[] { "192.168.1.1" });
            }
        }

        private static void wifi_NetworkAddressChanged(object sender, EventArgs e)
        {
            PrintMethodNameForDebugging("wifi_NetworkAddressChanged");

            PrintNetworkInterfaceDetails(_wifi);
        }

        private static void wifi_WirelessConnectivityChanged(object sender, WiFiRS9110.WirelessConnectivityEventArgs e)
        {
            PrintMethodNameForDebugging("wifi_WirelessConnectivityChanged");

            Debug.Print("IsConnected:" + e.IsConnected);
            Debug.Print("NetworkInformation:" + e.NetworkInformation); // this is null   

            PrintNetworkInterfaceDetails(_wifi);

            mre.Set();
        }

        private static void AddEventListenersToTheWifiDriver(WiFiRS9110 wifi)
        {
            PrintMethodNameForDebugging("AddEventListenersToTheWifiDriver");

            wifi.NetworkAddressChanged += wifi_NetworkAddressChanged;
            wifi.WirelessConnectivityChanged += wifi_WirelessConnectivityChanged;
        }

        private static void DisconnectTheWifiDriver(WiFiRS9110 wifi)
        {
            PrintMethodNameForDebugging("DisconnectTheWifiDriver");

            if (wifi.IsLinkConnected)
            {
                wifi.Disconnect();
            }
        }

        private static void OpenTheWifiDriver(WiFiRS9110 wifi)
        {
            PrintMethodNameForDebugging("OpenTheWifiDriver");

            if (!wifi.IsOpen)
            {
                // open the driver
                wifi.Open();
            }
        }

        private static WiFiRS9110 CreateWifiDriver()
        {
            PrintMethodNameForDebugging("CreateWifiDriver");

            // the wifi module is MOD2 and we connect via an SPI
            const SPI.SPI_module mod2 = SPI.SPI_module.SPI2;

            // why do we choose 1_10 as the chipset pin? 
            const Cpu.Pin chipSelect = Pin.P1_10;

            // why do we choose 2_11 as the external interupt pin?
            const Cpu.Pin externalInterupt = Pin.P2_11;

            // why do we choose 1_9 as the reset pin?
            const Cpu.Pin reset = Pin.P1_9;

            // why do we choose 4000?
            const uint clockRateKhz = 4000;

            // create a wifi object
            var wifi = new WiFiRS9110(mod2, chipSelect, externalInterupt, reset, clockRateKhz);

            return wifi;
        }

        private static void PrintNetworkInterfaceDetails(WiFiRS9110 wifi)
        {
            PrintMethodNameForDebugging("PrintNetworkInterfaceDetails");

            Debug.Print("GatewayAddress:" + wifi.NetworkInterface.GatewayAddress);
            Debug.Print("IPAddress:" + wifi.NetworkInterface.IPAddress);
            Debug.Print("SubnetMask:" + wifi.NetworkInterface.SubnetMask);
            Debug.Print("DnsAddresses");
            foreach (var s in wifi.NetworkInterface.DnsAddresses)
            {
                Debug.Print(s);
            }
            Debug.Print("IsDhcpEnabled:" + wifi.NetworkInterface.IsDhcpEnabled);
            Debug.Print("IsDynamicDnsEnabled:" + wifi.NetworkInterface.IsDynamicDnsEnabled);
            Debug.Print("NetworkInterfaceType:" + wifi.NetworkInterface.NetworkInterfaceType);
        }

        private static void PrintMethodNameForDebugging(string header)
        {
            Debug.Print("\n\n");
            Debug.Print(header);
            Debug.Print("--");
        }
    }
}
