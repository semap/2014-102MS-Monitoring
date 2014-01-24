using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GHI.Hardware.G120;
using GHI.Premium.Net;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.IO;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.HelloWorld));

            bool printDeviceInfo = false;
            if (printDeviceInfo)
            {
                PrintDeviceInfo();
            }

            WiFiRS9110 wifi = CreateAndOpenNewWiFiRs9110Proxy();

            bool connectToAccessPoint = false;
            if (connectToAccessPoint)
            {
                ConnectWifiToAccessPoint(wifi, "BlueMaple", "NutButter3");
                MakeAnHttpWebRequestOverInternet(wifi, "http://www.bigfont.ca");
                UseSocketsToGetWebPage();
                DisconnectWifiFromAccessPoint(wifi);
            }

            SetupAdHocHost(wifi, "MyAdhocHost");

            Debug.Print("Done");
        }

        private static void PrintDeviceInfo()
        {
            const string todo = "todo";

            // volumn info
            Debug.Print("Volumes:" + VolumeInfo.GetVolumes().Length);

            // hardware info
            HardwareProvider hwp = HardwareProvider.HwProvider;
            Debug.Print("SystemInfo.Version:" + SystemInfo.Version);
            Debug.Print("SystemInfo.OEMString:" + SystemInfo.OEMString);

            // pins
            Cpu.PinUsage[] pinUsuage;
            int pinCount;
            hwp.GetPinsMap(out pinUsuage, out pinCount);
            Debug.Print("GetPinsMap:" + pinCount);
            Debug.Print("GetSerialPins:" + todo);
            Debug.Print("GetSpiPins:" + todo);

            // other hardware stuff
            Debug.Print("GetSupportBaudRates:" + todo);
            Debug.Print("GetAnalogChannelsCount:" + hwp.GetAnalogChannelsCount());
            Debug.Print("GetAnalogOutputChannelsCount:" + hwp.GetAnalogOutputChannelsCount());
            Debug.Print("GetAnalogOutputPinForChannel:" + todo);
            Debug.Print("GetAnalogPinForChannel:" + todo);
            Debug.Print("GetAvailableAnalogOutputPrecisionInBitsForChannel:" + todo);
            Debug.Print("GetAvailablePrecisionInBitsForChannel:" + todo);
            Debug.Print("GetButtonPins:" + hwp.GetButtonPins(Button.LastSystemDefinedButton));
            Debug.Print("GetPWMChannelsCount:" + hwp.GetPWMChannelsCount());
            Debug.Print("GetPinsCount:" + hwp.GetPinsCount());
            Debug.Print("GetPinsUsage:" + hwp.GetPinsUsage(Cpu.Pin.GPIO_Pin0));
            Debug.Print("GetPwmPinForChannel:" + todo);
            Debug.Print("GetSerialPortsCount:" + hwp.GetSerialPortsCount());
            Debug.Print("GetSpiPortsCount:" + hwp.GetSpiPortsCount());
            Debug.Print("GetSupportedInterruptModes:" + todo);
            Debug.Print("GetSupportedResistorModes:" + todo);
            Debug.Print("IsSupportedBaudRate:" + todo);
            Debug.Print("GetI2CPins:" + todo);
            Debug.Print("GetLCDMetrics:" + todo);
        }

        private static WiFiRS9110 CreateAndOpenNewWiFiRs9110Proxy()
        {
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

            if (!wifi.IsOpen)
            {
                // open the driver
                wifi.Open();
            }

            wifi.WirelessConnectivityChanged += WiFi_Changed;

            // we ALWAYS have to class AssignNetworkingStackTo() because, 
            // "Currently, only one interface can access the TCP/IP stack at a time. Use this function to assign the TCP/IP stack to a certain [wifi] interface."
            NetworkInterfaceExtension.AssignNetworkingStackTo(wifi);

            return wifi;
        }

        private static void DisconnectWifiFromAccessPoint(WiFiRS9110 wifi)
        {
            if (wifi.IsLinkConnected)
            {
                wifi.Disconnect();
            }
        }

        private static void UseSocketsToGetWebPage()
        {
            const string url = "http://www.bigfont.ca";
            const string host = "bigfont.ca";
            IPHostEntry hostEntry = Dns.GetHostEntry(host);

            // we are assuming port 80
            var hostEndpoint = new IPEndPoint(hostEntry.AddressList[0], 80);

            // other interesting AddressFamilies include InterNetworkV6, Ieee12844
            // other interesting ProtocolTypes include Udp, dGram
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(hostEndpoint);

            using (socket)
            {
                const Int32 cMicrosecondsPerSecond = 1000000;

                // Send request to the server.
                const string request = "GET " + url + " HTTP/1.1\r\nHost: " + host +
                                       "\r\nConnection: Close\r\n\r\n";
                byte[] bytesToSend = Encoding.UTF8.GetBytes(request);
                socket.Send(bytesToSend, bytesToSend.Length, 0);

                // Reusable buffer for receiving chunks of the document.
                var buffer = new Byte[1024];

                // Accumulates the received page as it is built from the buffer.
                string page = String.Empty;

                // Wait up to 30 seconds for initial data to be available.  Throws 
                // an exception if the connection is closed with no data sent.
                DateTime timeoutAt = DateTime.Now.AddSeconds(30);
                while (socket.Available == 0 && DateTime.Now < timeoutAt)
                {
                    Thread.Sleep(100);
                }

                // Poll for data until 30-second timeout.  Returns true for data and 
                // connection closed.
                while (socket.Poll(30 * cMicrosecondsPerSecond,
                    SelectMode.SelectRead))
                {
                    // If there are 0 bytes in the buffer, then the connection is 
                    // closed, or we have timed out.
                    if (socket.Available == 0)
                        break;

                    // Zero all bytes in the re-usable buffer.
                    Array.Clear(buffer, 0, buffer.Length);

                    // Read a buffer-sized HTML chunk.
                    Int32 bytesRead = socket.Receive(buffer);

                    // Append the chunk to the string.
                    page = page + new String(Encoding.UTF8.GetChars(buffer));

                    Debug.Print(page);
                }
            }
        }

        private static void MakeAnHttpWebRequestOverInternet(WiFiRS9110 wifi, string url)
        {
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            if (request == null)
            {
                return;
            }

            request.KeepAlive = true;

            WebResponse resp = null;

            try
            {
                resp = request.GetResponse();
            }
            catch (Exception e)
            {
                Debug.Print("Exception in HttpWebRequest.GetResponse(): " + e);
            }

            if (resp != null && resp.ContentLength > 0)
            {
                Debug.Print("There was a response");
            }
        }

        private static void ConnectWifiToAccessPoint(WiFiRS9110 wifi, string ssid, string preSharedKey)
        {
            if (!wifi.NetworkInterface.IsDhcpEnabled)
            {
                // use the router to assign dynamic ip addresses
                wifi.NetworkInterface.EnableDhcp();
            }

            // get the target ssid network
            WiFiNetworkInfo[] scanResults = wifi.Scan(ssid);
            if (scanResults.Length == 0)
            {
                Debug.Print(ssid + " was not available. Try one of these:");

                scanResults = wifi.Scan();

                // inspect the available WiFi networks                
                foreach (WiFiNetworkInfo r in scanResults)
                {
                    Debug.Print("-----");
                    Debug.Print("SSID:" + r.SSID);
                    Debug.Print("ChannelNumber:" + r.ChannelNumber);
                    Debug.Print("PhysicalAddress:" + r.PhysicalAddress);
                    Debug.Print("RSSI:" + r.RSSI);
                    Debug.Print("SecMode:" + r.SecMode);
                    Debug.Print("networkType:" + r.networkType);
                }
            }
            else
            {
                // check whether we are connected to a network
                Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);

                // join a network i.e. we are joining BlueMaple
                WiFiNetworkInfo targetWifiNetwork = scanResults[0];
                wifi.Join(targetWifiNetwork, preSharedKey);

                // check whether we are connected to a network
                Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);
            }
        }

        private const string ipAddress = "169.255.0.200";
        private const string subnetMask = "255.255.0.0";
        private const string gatewayAddress = "169.255.0.1";

        private static void SetupAdHocHost(WiFiRS9110 wifi, string hostName)
        {
            // works well with laptop using MS-Windows as long you use an IP address 169.255.x.x
            // See more at: https://www.ghielectronics.com/community/forum/topic?id=10374&page=3#sthash.j13kLPi4.dpuf
            wifi.NetworkInterface.EnableStaticIP(ipAddress, subnetMask, gatewayAddress);
            wifi.StartAdHocHost(hostName, SecurityMode.Open, "", 10);            
        }        

        private static void RunHttpListener()
        {
            Debug.Print("Run server.");
            var listener = new HttpListener("http", -1);
            listener.Start();
            HttpListenerContext context = listener.GetContext();
            if (context != null)
            {
                HttpListenerResponse response = context.Response;          
            }
            Debug.Print("Done");
        }

        private static void RunTcpSocketServer()
        {
            Debug.Print("RunTcpSocketServer");

            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 80);
            server.Bind(localEndpoint);
            server.Listen(1);
            while (true)
            {
                Debug.Print("Foo");
                // Wait for a client to connect.
                try
                {
                    Socket clientSocket = server.Accept();
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
                
                Debug.Print("Bar");
            }
        }

        private static void WiFi_Changed(object sender, WiFiRS9110.WirelessConnectivityEventArgs e)
        {          
            Debug.Print("isConnected = " + e.IsConnected);
            Debug.Print("Info = " + e.NetworkInformation); // this is null  
      
            RunTcpSocketServer();
        }
    }
}