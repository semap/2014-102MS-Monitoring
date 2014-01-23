using GhiHardwareG120 = GHI.Hardware.G120;
using GhiPremiumNet = GHI.Premium.Net;
using SpotBase = Microsoft.SPOT;
using SpotIO = Microsoft.SPOT.IO;
using SpotHardware = Microsoft.SPOT.Hardware;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            SpotBase.Debug.Print(
                Resources.GetString(Resources.StringResources.HelloWorld));

            SpotBase.Debug.Print("Volumes:" + SpotIO.VolumeInfo.GetVolumes().Length);
            SpotBase.Debug.Print("SystemInfo.Version:" + SpotHardware.SystemInfo.Version);
            SpotBase.Debug.Print("SystemInfo.OEMString:" + SpotHardware.SystemInfo.OEMString);

            const string todo = "todo";
            SpotHardware.HardwareProvider hwp = SpotHardware.HardwareProvider.HwProvider;
            SpotBase.Debug.Print("GetAnalogChannelsCount:" + hwp.GetAnalogChannelsCount());
            SpotBase.Debug.Print("GetAnalogOutputChannelsCount:" + hwp.GetAnalogOutputChannelsCount());
            SpotBase.Debug.Print("GetAnalogOutputPinForChannel:" + todo);
            SpotBase.Debug.Print("GetAnalogPinForChannel:" + todo);
            SpotBase.Debug.Print("GetAvailableAnalogOutputPrecisionInBitsForChannel:" + todo);
            SpotBase.Debug.Print("GetAvailablePrecisionInBitsForChannel:" + todo);
            SpotBase.Debug.Print("GetButtonPins:" + hwp.GetButtonPins(SpotHardware.Button.LastSystemDefinedButton));
            SpotBase.Debug.Print("GetPWMChannelsCount:" + hwp.GetPWMChannelsCount());
            SpotBase.Debug.Print("GetPinsCount:" + hwp.GetPinsCount());
            SpotBase.Debug.Print("GetPinsUsage:" + hwp.GetPinsUsage(SpotHardware.Cpu.Pin.GPIO_Pin0));
            SpotBase.Debug.Print("GetPwmPinForChannel:" + todo);
            SpotBase.Debug.Print("GetSerialPortsCount:" + hwp.GetSerialPortsCount());
            SpotBase.Debug.Print("GetSpiPortsCount:" + hwp.GetSpiPortsCount());
            SpotBase.Debug.Print("GetSupportedInterruptModes:" + todo);
            SpotBase.Debug.Print("GetSupportedResistorModes:" + todo);
            SpotBase.Debug.Print("IsSupportedBaudRate:" + todo);
            SpotBase.Debug.Print("GetI2CPins:" + todo);
            SpotBase.Debug.Print("GetLCDMetrics:" + todo);

            SpotHardware.Cpu.PinUsage[] pinUsuage;
            int pinCount;
            hwp.GetPinsMap(out pinUsuage, out pinCount);
            SpotBase.Debug.Print("GetPinsMap:" + pinCount);
            SpotBase.Debug.Print("GetSerialPins:" + todo);
            SpotBase.Debug.Print("GetSpiPins:" + todo);
            SpotBase.Debug.Print("GetSupportBaudRates:" + todo);

            SpotBase.Debug.Print("---------------");
            SpotBase.Debug.Print("--WiFiExample--");
            SpotBase.Debug.Print("---------------");

            WiFiExample();

            SpotBase.Debug.Print("Done");
        }

        private static GhiPremiumNet.WiFiRS9110 CreateWifiProxy()
        {
            // the wifi module is MOD2 and we connect via an SPI
            const SpotHardware.SPI.SPI_module mod2 = SpotHardware.SPI.SPI_module.SPI2;

            // why do we choose 1_10 as the chipset pin? 
            const SpotHardware.Cpu.Pin chipSelect = GhiHardwareG120.Pin.P1_10;

            // why do we choose 2_11 as the external interupt pin?
            const SpotHardware.Cpu.Pin externalInterupt = GhiHardwareG120.Pin.P2_11;

            // why do we choose 1_9 as the reset pin?
            const SpotHardware.Cpu.Pin reset = GhiHardwareG120.Pin.P1_9;

            // why do we choose 4000?
            const uint clockRateKhz = 4000;

            // create a wifi object
            var wifi = new GhiPremiumNet.WiFiRS9110(mod2, chipSelect, externalInterupt, reset, clockRateKhz);

            return wifi;
        }

        // https://www.ghielectronics.com/docs/48/fez-cobra-ii-developer#425
        private static void WiFiExample()
        {
            var wifi = CreateWifiProxy();

            if (!wifi.IsOpen)
            {
                // open the driver
                wifi.Open();
            }

            if (!wifi.NetworkInterface.IsDhcpEnabled)
            {
                // enable DHCP 
                // we assume because we need to assign IP addresses
                wifi.NetworkInterface.EnableDhcp();
            }

            // we have to do this because...
            // "Currently, only one interface can access the TCP/IP stack at a time. Use this function to assign the TCP/IP stack to a certain interface."
            GhiPremiumNet.NetworkInterfaceExtension.AssignNetworkingStackTo(wifi);

            // scan all Wifi channels for available networks
            GhiPremiumNet.WiFiNetworkInfo[] scanResults = wifi.Scan();

            // inspect the available WiFi networks
            SpotBase.Debug.Print("Wifi Scan Results");
            foreach (GhiPremiumNet.WiFiNetworkInfo r in scanResults)
            {
                SpotBase.Debug.Print("-----");
                SpotBase.Debug.Print("SSID:" + r.SSID);
                SpotBase.Debug.Print("ChannelNumber:" + r.ChannelNumber);
                SpotBase.Debug.Print("PhysicalAddress:" + r.PhysicalAddress);
                SpotBase.Debug.Print("RSSI:" + r.RSSI);
                SpotBase.Debug.Print("SecMode:" + r.SecMode);
                SpotBase.Debug.Print("networkType:" + r.networkType);
            }

            // check whether we are connected to a network
            SpotBase.Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);

            // join a network i.e. we are joining BlueMaple
            const string preSharedKey = "NutButter3";
            GhiPremiumNet.WiFiNetworkInfo targetWifiNetwork = scanResults[0];
            wifi.Join(targetWifiNetwork, preSharedKey);

            // check whether we are connected to a network
            SpotBase.Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);

            // todo Create a peer-to-peer connection for two way communication with FONTY (i.e. with Shaun's main Desktop Computer).
            wifi.StartAdHocHost("Hello World", GhiPremiumNet.SecurityMode.Open, "", 0);

            SpotBase.Debug.Print("-----StartAdHocHost-----");
            
            // todo Figure out how to send data across the Ethernet network via TCP or UDP
            // Options
            // HttpWebRequest
            // Windows Communication Foundation
            // How can we leverage the peer-to-peer communication in StartAdHocHost?
        }
    }
}