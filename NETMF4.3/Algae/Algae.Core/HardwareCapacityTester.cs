using System;
using System.Net;
using System.Net.Sockets;
using Algae.Abstractions;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Algae.Core
{
    public class HardwareCapacityTester : ITestHardwareCapacity
    {
        public bool TestWanHttp()
        {
            var result = false;

            Debug.Print("Testing Wan");

            try
            {
                var html = SocketClient.GetWebPage("http://www.bigfont.ca", 80);
                result = html.IndexOf("BigFont") > 0;

                Debug.Print("Wan online.");
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

        public bool TestLanHttp()
        {
            var result = false;

            Debug.Print("Testing Lan");

            try
            {
                var ipAddress = IPAddress.Parse("169.254.41.34");
                
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

        public bool TestLanPing()
        {
            return true;
        }

        public bool TestDhcp()
        {
            var result = false;

            Debug.Print("Testing Dhcp.");

            try
            {
                var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var ni in allNetworkInterfaces)
                {
                    Debug.Print("NetworkInterfaceType:" + ni.NetworkInterfaceType.ToString());
                    Debug.Print("IsDhcpEnabled:" + ni.IsDhcpEnabled);
                    Debug.Print("IPAddress:" + ni.IPAddress);
                    Debug.Print("SubnetMask:" + ni.SubnetMask);
                    Debug.Print("GatewayAddress:" + ni.GatewayAddress);
                    Debug.Print("--");
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            return result;
        }
    }
}
