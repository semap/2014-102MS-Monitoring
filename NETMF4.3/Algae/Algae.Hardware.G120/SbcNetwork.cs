using System;
using System.Threading;
using GHI.Networking;
using GGI_Pins = GHI.Pins;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Algae.Abstractions;

namespace Algae.Hardware.G120
{
    /*
     * https://www.ghielectronics.com/docs/30/networking#3123
     * https://www.ghielectronics.com/docs/318/fez-cobra-ii-developers-guide
     */
    public class SbcNetwork : INetworkDriver
    {
        private EthernetENC28J60 Ethernet;

        private ILogger _logger;

        public SbcNetwork(ILogger logger)
        {
            _logger = logger;
        }

        public void InitializeNetwork()
        {
            Ethernet = new EthernetENC28J60(
                SPI.SPI_module.SPI2,
                GGI_Pins.G120.P1_10, 
                GGI_Pins.G120.P2_11, 
                GGI_Pins.G120.P1_9);
 
            Ethernet.Open();
            Ethernet.EnableDhcp();
            Ethernet.EnableDynamicDns();

            while (Ethernet.IPAddress == "0.0.0.0")
            {
                _logger.Write("Waiting for DHCP");
                Thread.Sleep(250);
            }

            _logger.Write(Ethernet.IPAddress);
            _logger.Write("The network is now ready to use.");
        }

        public void InitializeServer()
        {

        }
    }
}
