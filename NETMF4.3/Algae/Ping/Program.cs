using System;
using System.Net;
using Microsoft.SPOT;

namespace Ping
{
    public class Program
    {
        public static void Main()
        {
            Ping("8.8.8.8"); // Wide Area Network, e.g. google.com
            Ping("10.10.40.124"); // Local Area Network,  e.g. localhost on emulator's computer
        }

        public static void Ping(string ipAddress)
        {
            var timeToLive = 128;
            var payloadSize = 512;
            var pingId = (ushort)1337;
            var remoteAddress = IPAddress.Parse(ipAddress);

            var pingSocket = new RawSocketPing(timeToLive, payloadSize, pingId);

            pingSocket.InitializeSocket();
            pingSocket.BuildPingPacket();

            var remainingTries = 3;
            Debug.Print("Pinging " + remoteAddress.ToString() + " with " + payloadSize + " bytes of data:");

            while (remainingTries > 0)
            {
                remainingTries--;

                bool success = pingSocket.DoPing(remoteAddress);
                Debug.Print(success.ToString());
            }

            pingSocket.Close();
        }
    }
}
