using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SPOT;
using Socket = System.Net.Sockets.Socket;

namespace Algae.Core
{
    public static class SocketClient
    {
        private const Int32 MicrosecondsPerSecond = 1000000;
        private static Regex ipAddressRegex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}.[0-9]{1,3}");

        public static string GetWebPage(string host, int port)
        {
            var request = GenerateHttpRequest(host);

            var ipAddress = ipAddressRegex.IsMatch(host)
                ? host
                : Dns.GetHostEntry(host).AddressList[0].ToString();

            using (var serverSocket = ConnectSocket(IPAddress.Parse(ipAddress), port))
            {
                SendRequest(serverSocket, request);
                return ReceiveResponse(serverSocket);
            }
        }

        private static string GenerateHttpRequest(string host)
        {
            var url = "http://" + host;

            var builder = new StringBuilder();
            builder.Append("GET " + url + " HTTP/1.1\r\n");
            builder.Append("Host: " + host + "\r\n");
            builder.Append("Connection: Close\r\n\r\n");
            return builder.ToString();
        }

        private static Socket ConnectSocket(IPAddress server, int port)
        {
            // Create socket and connect to the server's IP address and port
            Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(server, port));
            return socket;
        }

        private static void SendRequest(Socket serverSocket, string request)
        {
            Byte[] bytesToSend = Encoding.UTF8.GetBytes(request);
            serverSocket.Send(bytesToSend, bytesToSend.Length, 0);
        }

        private static string ReceiveResponse(Socket serverSocket)
        {
            // Reusable buffer for receiving chunks of the document.
            Byte[] buffer = new Byte[1024];

            // Accumulates the received page as it is built from the buffer.
            String page = String.Empty;

            // Wait up to 30 seconds for initial data to be available.  Throws 
            // an exception if the connection is closed with no data sent.
            DateTime timeoutAt = DateTime.Now.AddSeconds(30);
            while (serverSocket.Available == 0 && DateTime.Now < timeoutAt)
            {
                System.Threading.Thread.Sleep(100);
            }

            // Poll for data until 30-second timeout. Returns true for data and 
            // connection closed.
            while (serverSocket.Poll(30 * MicrosecondsPerSecond,
                SelectMode.SelectRead))
            {
                // If there are 0 bytes in the buffer, then the connection is 
                // closed, or we have timed out.
                if (serverSocket.Available == 0)
                    break;

                // Zero all bytes in the re-usable buffer.
                Array.Clear(buffer, 0, buffer.Length);

                // Read a buffer-sized HTML chunk.
                Int32 bytesRead = serverSocket.Receive(buffer);

                // Append the chunk to the string.
                page = page + new String(Encoding.UTF8.GetChars(buffer));
            }

            return page;
        }
    }
}
