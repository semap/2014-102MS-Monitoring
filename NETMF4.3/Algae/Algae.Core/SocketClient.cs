using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SPOT;
using Socket = System.Net.Sockets.Socket;

namespace Algae.Core
{
    // TODO Consider making this into an interface.
    public static class SocketClient
    {
        private const Int32 MicrosecondsPerSecond = 1000000;
        private static Regex ipAddressRegex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}.[0-9]{1,3}");

        public static string GetWebPage(string host, int port)
        {
            var request = GenerateHttpRequest(host, port);

            var ipAddress = ipAddressRegex.IsMatch(host)
                ? host
                : Dns.GetHostEntry(host).AddressList[0].ToString();

            using (var serverSocket = ConnectSocket(IPAddress.Parse(ipAddress), port))
            {
                SendRequest(serverSocket, request);
                return ReceiveResponse(serverSocket);
            }
        }

        private static string GenerateHttpRequest(string host, int port, bool keepAlive = false)
        {
            var url = "http://" + host;

            var builder = new StringBuilder();
            builder.Append("GET " + url + ":" + port.ToString() + "/ HTTP/1.1\r\n");
            builder.Append("Host: " + host + ":" + port.ToString() + "\r\n");

            if (keepAlive)
            {
                builder.Append("Connection: keep-alive\r\n\r\n");
            }
            else
            {
                builder.Append("Connection: close\r\n\r\n");
            }

            return builder.ToString();
        }

        private static Socket ConnectSocket(IPAddress server, int port)
        {
            // Create socket
            Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            // Connect socket
            socket.Connect(new IPEndPoint(server, port));

            return socket;
        }

        private static void SendRequest(Socket serverSocket, string request)
        {
            // Blocks until send returns.
            var bytesToSend = Encoding.UTF8.GetBytes(request);
            serverSocket.Send(bytesToSend, bytesToSend.Length, SocketFlags.None);
        }

        private static string ReceiveResponse(Socket serverSocket)
        {
            var bytesToReceive = new byte[1024];
            serverSocket.Receive(bytesToReceive);
            var page = new string(Encoding.UTF8.GetChars(bytesToReceive));
            return page;
        }
    }
}
