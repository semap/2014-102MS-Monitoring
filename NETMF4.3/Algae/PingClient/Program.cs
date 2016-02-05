using System;
using System.Net;
using Microsoft.SPOT;

namespace PingClient
{
    public class Program
    {
        const string WideAreaNetworkPingAddress = "8.8.8.8"; //  e.g. google.com
        const string LocalAreaNetworkPingAddress = "10.10.40.124"; // e.g. localhost on emulator's computer

        public static void Main()
        {
            Debug.EnableGCMessages(false);

            Ping(WideAreaNetworkPingAddress); 
            Ping(LocalAreaNetworkPingAddress);
        }

        public static void Ping(string ipAddress)
        {
            var timeToLive = 128;
            var payloadSize = 512;
            var pingId = (ushort)1337;
            var remoteAddress = IPAddress.Parse(ipAddress);

            Debug.Print("Pinging " + remoteAddress.ToString() + " with " + payloadSize + " bytes of data:");

            var pingSocket = new PingClient(timeToLive, payloadSize, pingId);

            pingSocket.InitializeSocket();
            pingSocket.BuildPingPacket();

            var remainingTries = 3;
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
