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
        private ILogger _logger;

        public HardwareCapacityTester(ILogger logger)
        {
            _logger = logger;
            Debug.EnableGCMessages(false);
        }
        
        public bool TestNetworkInterfaces()
        {
            _logger.Write("-----");
            _logger.Write("TestNetworkInterfaces");

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
                _logger.Write(builder.ToString());

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
                    _logger.Write(builder.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex.ToString());
            }

            return result;
        }

        public bool TestHttpRequest(Proximity networkProximity, string host, int port = 80)
        {
            _logger.Write("-----");
            _logger.Write("TestHttpClient:" + networkProximity.ToString());

            // TODO Something with the Proximity enum.
            // Why do we even have it?

            return TestHttp(host, port);
        }

        public bool TestPing()
        {
            throw new NotImplementedException();
        }

        private string GetLocalHost()
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
                    break;
                }
            }

            return localHost;
        }

        private bool TestHttp(string host, int port)
        {
            var success = false;

            _logger.Write("Requesting " + host + ":" + port.ToString());

            try
            {
                var html = SocketClient.GetWebPage(host, port);
                success =
                    html.IndexOf("head") > 0 &&
                    html.IndexOf("body") > 0;
            }
            catch (Exception)
            {
                _logger.Write("Request failed");
                _logger.Write("Try requesting the host through your web browser.");
                _logger.Write("And if you are testing the LAN, ensure the host's port 80 is open.");
            }

            if (success)
            {
                _logger.Write("Request succeeded.");
            }
            else
            {
                _logger.Write("Request failed.");
            }

            return success;
        }
    }
}
