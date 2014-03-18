using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace MFConsoleApplication2
{
    public class Program
    {
        public static void Main()
        {
            // First, make sure we actually have a network interface to work with!
            if (NetworkInterface.GetAllNetworkInterfaces().Length < 1)
            {
                Debug.Print("No Active network interfaces. Bombing out.");
                Thread.CurrentThread.Abort();
            }

            // OK, retrieve the network interface
            NetworkInterface NI = NetworkInterface.GetAllNetworkInterfaces()[0];

            // If DHCP is not enabled, then enable it and get an IP address, else renew the lease. Most iof us have a DHCP server
            // on a network, even at home (in the form of an internet modem or wifi router). If you want to use a static IP
            // then comment out the following code in the "DHCP" region and uncomment the code in the "fixed IP" region.

            #region DHCP Code

            //if (NI.IsDhcpEnabled == false)
            //{
            //    Debug.Print("Enabling DHCP.");
            //    NI.EnableDhcp();
            //    Debug.Print("DCHP - IP Address = " + NI.IPAddress + " ... Net Mask = " + NI.SubnetMask +
            //                " ... Gateway = " + NI.GatewayAddress);
            //}
            //else
            //{
            //    Debug.Print("Renewing DHCP lease.");
            //    NI.RenewDhcpLease();
            //    Debug.Print("DCHP - IP Address = " + NI.IPAddress + " ... Net Mask = " + NI.SubnetMask +
            //                " ... Gateway = " + NI.GatewayAddress);
            //}

            #endregion

            #region Static IP code

            // Uncomment the following line if you want to use a static IP address, and comment out the DHCP code region above
            NI.EnableStaticIP("192.169.0.25", "255.255.255.0", "192.169.0.1");

            #endregion

            // Create the socket            
            var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the listening socket to the port
            IPAddress hostIP = IPAddress.Parse(NI.IPAddress);
            var ep = new IPEndPoint(hostIP, 80);
            try
            {
                listenSocket.Bind(ep);
            }
            catch (Exception e)
            {                
                Debug.Print(e.Message);
            }

            // Start listening
            listenSocket.Listen(1);

            // Main thread loop
            while (true)
            {
                try
                {
                    Debug.Print("listening...");
                    Socket newSock = listenSocket.Accept();
                    Debug.Print("Accepted a connection from " + newSock.RemoteEndPoint);
                    byte[] messageBytes = Encoding.UTF8.GetBytes("Hello, browser! I think the time is " + DateTime.Now);
                    newSock.Send(messageBytes);
                    newSock.Close();
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
            }
        }
    }
}