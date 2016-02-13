using System;
using Algae.Abstractions;
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Algae.Hardware.Emulator
{
    public class SbcNetwork : INetworkDriver
    {
        public void InitializeNetwork()
        {
            // The emulator's network 
            // appears to be the host computer's network.
        }

        public void InitializeServer()
        {
            // The emulator's server 
            // appears to be host computer's server.
        }
    }
}
