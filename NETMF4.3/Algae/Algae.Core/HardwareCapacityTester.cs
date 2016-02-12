using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Algae.Abstractions;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Algae.Core
{
    public class HardwareCapacityTester : ITestHardwareCapacity
    {
        /// <summary>
        /// Test a simple HTTP request to an IP Address on the WAN.
        /// </summary>
        /// <returns>true if the request succeeded.</returns>
        public bool TestWanHttp(string url)
        {
            var success = false;

            Debug.Print("-----");
            Debug.Print("TestWanHttp");
            Debug.Print("Requesting " + url);

            try
            {
                var html = SocketClient.GetWebPage(url, 80);
                success =
                    html.IndexOf("head") > 0 &&
                    html.IndexOf("body") > 0;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            if (success)
            {
                Debug.Print("Wan online.");
            }

            return success;
        }

        /// <summary>
        /// Test a simple HTTP request to an IP Address on the LAN.
        /// </summary>
        /// <remarks>
        /// For the NETMF Emulator,
        /// use the IP Address of the localhost and 
        /// ensure that IIS is serving a simple page at that address.
        /// </remarks>
        /// <returns>true if the request succeeded.</returns>
        public bool TestLanHttp()
        {
            var result = false;

            Debug.Print("TestLanHttp");

            try
            {
                var ipAddress = IPAddress.Parse("192.168.1.148");

                var socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Icmp);

                socket.Connect(new IPEndPoint(ipAddress, 80));

                result = true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            return result;
        }

        public bool TestWanPing()
        {
            return true;
        }

        public bool TestLanPing()
        {
            return true;
        }

        public bool TestDhcp()
        {
            var result = false;

            Debug.Print("-----");
            Debug.Print("TestDhcp");

            try
            {
                var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var builder = new StringBuilder();

                const int colWidth = 30;

                builder.Append("Network Interface");
                builder.Append(new string(' ', colWidth*1 - builder.Length));

                builder.Append("Enabled");
                builder.Append(new string(' ', colWidth*2 - builder.Length));

                builder.Append("IPAddress");
                builder.Append(new string(' ', colWidth*3 - builder.Length));

                builder.Append("SubnetMask");
                builder.Append(new string(' ', colWidth*4 - builder.Length));

                builder.Append("GatewayAddress");
                Debug.Print(builder.ToString());
                
                foreach (var ni in allNetworkInterfaces)
                {
                    builder.Clear();

                    builder.Append(EnumName(ni.NetworkInterfaceType));
                    builder.Append(new string(' ', colWidth * 1 - builder.Length));

                    builder.Append(ni.IsDhcpEnabled.ToString());
                    builder.Append(new string(' ', colWidth * 2 - builder.Length));

                    builder.Append(ni.IPAddress);
                    builder.Append(new string(' ', colWidth * 3 - builder.Length));

                    builder.Append(ni.SubnetMask);
                    builder.Append(new string(' ', colWidth * 4 - builder.Length));

                    builder.Append(ni.GatewayAddress);
                    Debug.Print(builder.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            return result;
        }

        public static string EnumName(NetworkInterfaceType enumValue)
        {
            var name = string.Empty;

            switch (enumValue)
            {
                case NetworkInterfaceType.Ethernet:
                    name = "Ethernet";
                    break;

                case NetworkInterfaceType.Wireless80211:
                    name = "Wireless80211";
                    break;

                case NetworkInterfaceType.Unknown:
                default:
                    name = "Unknown Network Interface";
                    break;
            }

            return name;
        }
    }
}
