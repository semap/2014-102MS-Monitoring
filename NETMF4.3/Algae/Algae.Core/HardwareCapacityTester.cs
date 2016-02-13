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
        public HardwareCapacityTester()
        {
            // turn off the noise :)
            Debug.EnableGCMessages(false);
        }
        
        public bool TestDhcp()
        {
            Debug.Print("-----");
            Debug.Print("TestDhcp");

            var result = false;

            try
            {
                var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var builder = new StringBuilder();

                int[] columns = new int[] 
                { 
                    30, 
                    40, 
                    60,
                    80
                };

                builder.Append("Network Interface");
                builder.Append(new string(' ', columns[0] - builder.Length));

                builder.Append("Enabled");
                builder.Append(new string(' ', columns[1] - builder.Length));

                builder.Append("IPAddress");
                builder.Append(new string(' ', columns[2] - builder.Length));

                builder.Append("SubnetMask");
                builder.Append(new string(' ', columns[3] - builder.Length));

                builder.Append("GatewayAddress");
                Debug.Print(builder.ToString());

                foreach (var ni in allNetworkInterfaces)
                {
                    builder.Clear();

                    builder.Append(EnumName(ni.NetworkInterfaceType));
                    builder.Append(new string(' ', columns[0] - builder.Length));

                    builder.Append(ni.IsDhcpEnabled.ToString());
                    builder.Append(new string(' ', columns[1] - builder.Length));

                    builder.Append(ni.IPAddress);
                    builder.Append(new string(' ', columns[2] - builder.Length));

                    builder.Append(ni.SubnetMask);
                    builder.Append(new string(' ', columns[3] - builder.Length));

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

        public bool TestWanHttp(string host)
        {
            Debug.Print("-----");
            Debug.Print("TestWanHttp");
            return TestHttp(host);
        }

        public bool TestLanHttp(string host)
        {
            Debug.Print("-----");
            Debug.Print("TestLanHttp");
            return TestHttp(host);
        }

        public bool TestWanPing()
        {
            return true;
        }

        public bool TestLanPing()
        {
            return true;
        }

        private static bool TestHttp(string host)
        {
            var success = false;

            Debug.Print("Requesting " + host);

            try
            {
                var html = SocketClient.GetWebPage(host, 80);
                success =
                    html.IndexOf("head") > 0 &&
                    html.IndexOf("body") > 0;
            }
            catch (Exception)
            {
                Debug.Print("Request failed");
                Debug.Print("Try requesting the host through your web browser.");
                Debug.Print("And if you are testing the LAN, ensure the host's port 80 is open.");
            }

            if (success)
            {
                Debug.Print("Request succeeded.");
            }
            else
            {
                Debug.Print("Request failed.");
            }

            return success;
        }

        private static string EnumName(NetworkInterfaceType enumValue)
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
