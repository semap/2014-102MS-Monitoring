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
            // This is not necessary in the emulator.
        }
    }
}
