using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Socket = System.Net.Sockets.Socket;

namespace HttpServer
{
    // http://127.0.0.1:12000/
    public class HttpServer
    {
        const Int32 Port = 12000;
        const Int32 MicrosecondsPerSecond = 1000000;

        private Socket _clientSocket;

        public HttpServer(Boolean asynchronously)
        {
            var server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream, 
                ProtocolType.Tcp);

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);
            server.Bind(localEndPoint);
            server.Listen(Int32.MaxValue);

            while (true)
            {
                _clientSocket = server.Accept();

                if (asynchronously)
                {
                    new Thread(ProcessRequest).Start();
                }
                else
                {
                    ProcessRequest();
                }
            }
        }

        private void ProcessRequest()
        {
            using (_clientSocket)
            {
                // Wait for the client request to start to arrive.
                Byte[] buffer = new Byte[1024];
                if (_clientSocket.Poll(5 * MicrosecondsPerSecond, SelectMode.SelectRead))
                {
                    // If 0 bytes in buffer, then the connection has been closed, 
                    // reset, or terminated.
                    if (_clientSocket.Available == 0)
                    {
                        return;
                    }

                    // Read the first chunk of the request (we don't actually do anything with it).
                    int bytesRead = _clientSocket.Receive(buffer, _clientSocket.Available, SocketFlags.None);

                    // Return a static HTML document to the client.
                    String s =
                        "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\n\r\n<html><head><title>.NET Micro Framework Web Server</title></head>" +
                       "<body><bold><a href=\"http://www.microsoft.com/netmf/\">Learn more about the .NET Micro Framework by clicking here</a></bold></body></html>";

                    _clientSocket.Send(Encoding.UTF8.GetBytes(s));
                }
            }
        }
    }
}
