using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Algae.Hardware.Emulator.Ethernet
{
    public class SbcNetwork
    {
        public SbcNetwork()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        }
    }
}
