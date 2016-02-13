using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Algae.Abstractions;
using Algae.Core.Extensions.NETMF;
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
        
        public bool TestNetworkInterfaces()
        {
            Debug.Print("-----");
            Debug.Print("TestNetworkInterfaces");

            var result = false;

            try
            {
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

                var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var networkInterface in allNetworkInterfaces)
                {
                    builder.Clear();

                    builder.Append(networkInterface.NetworkInterfaceType.GetName());
                    builder.Append(new string(' ', columns[0] - builder.Length));

                    builder.Append(networkInterface.IsDhcpEnabled.ToString());
                    builder.Append(new string(' ', columns[1] - builder.Length));

                    builder.Append(networkInterface.IPAddress);
                    builder.Append(new string(' ', columns[2] - builder.Length));

                    builder.Append(networkInterface.SubnetMask);
                    builder.Append(new string(' ', columns[3] - builder.Length));

                    builder.Append(networkInterface.GatewayAddress);
                    Debug.Print(builder.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            return result;
        }

        public bool TestHttpRequest(Proximity networkProximity, string host = "")
        {
            Debug.Print("-----");
            Debug.Print("TestHttpClient:" + networkProximity.ToString());

            if (networkProximity == Proximity.Self && host == string.Empty)
            {
                host = GetLocalHost();
            }

            return TestHttp(host);
        }

        public bool TestPing()
        {
            throw new NotImplementedException();
        }

        private static string GetLocalHost()
        {
            var localHost = string.Empty;
            var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in allNetworkInterfaces)
            {
                if (networkInterface.IsDhcpEnabled && 
                    networkInterface.IPAddress != "0.0.0.0" &&
                    networkInterface.GatewayAddress != "0.0.0.0")
                {
                    localHost = networkInterface.IPAddress;
                }
            }

            return localHost;
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
    }
}
