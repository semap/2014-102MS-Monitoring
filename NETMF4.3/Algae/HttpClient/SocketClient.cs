using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SPOT;
using Socket = System.Net.Sockets.Socket;

namespace HttpClient
{
    public static class HttpClient
    {
        /// <summary>
        /// Issues a request for the root document on the specified server.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static String GetWebPage(String url, Int32 port)
        {
            const Int32 MicrosecondsPerSecond = 1000000;
            string server = GetHostFromURL(url);

            // Create a socket connection to the specified server and port.
            using (Socket serverSocket = ConnectSocket(server, port))
            {
                // Send request to the server.
                String request =
                    "GET " + url +
                    " HTTP/1.1\r\nHost: " + server +
                    "\r\nConnection: Close\r\n\r\n";

                Byte[] bytesToSend = Encoding.UTF8.GetBytes(request);
                serverSocket.Send(bytesToSend, bytesToSend.Length, 0);

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

        /// <summary>
        /// Creates a socket and uses the socket to connect to the server's IP 
        /// address and port.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static Socket ConnectSocket(String server, Int32 port)
        {
            // Get server's IP address.
            IPAddress ipAddress = null;
            var ipAddressRegex = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            if (ipAddressRegex.IsMatch(server))
            {
                ipAddress = IPAddress.Parse(server);
            }
            else
            {
                var hostEntry = Dns.GetHostEntry(server);
                ipAddress = hostEntry.AddressList[0];
            }

            // Create socket and connect to the server's IP address and port
            Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(ipAddress, port));
            return socket;
        }

        /// <summary>
        /// Extracts the host string from the URL.
        /// </summary>
        /// <param name="url">The complete URL to parse.</param>
        /// <returns>The host string.</returns>
        private static String GetHostFromURL(string url)
        {
            // Figure out host
            int start = url.IndexOf("://");
            int end = start >= 0 ? url.IndexOf('/', start + 3) : url.IndexOf('/');

            if (start >= 0)
            {
                // move start after ://
                start += 3;

                if (end >= 0)
                {
                    // http://example.com/example
                    return url.Substring(start, end - start);
                }
                else
                {
                    // http://example.com
                    return url.Substring(start);
                }
            }
            if (end >= 0)
            {
                // example.com/example
                return url.Substring(0, end + 1);
            }

            return url;
        }
    }
}
