using GHI.Hardware.G120;
using GHI.Premium.Net;
using SpotBase = Microsoft.SPOT;
using SpotIO = Microsoft.SPOT.IO;
using SpotWare = Microsoft.SPOT.Hardware;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            SpotBase.Debug.Print(
                Resources.GetString(Resources.StringResources.HelloWorld));

            SpotBase.Debug.Print("Volumes:" + SpotIO.VolumeInfo.GetVolumes().Length);
            SpotBase.Debug.Print("SystemInfo.Version:" + SpotWare.SystemInfo.Version);
            SpotBase.Debug.Print("SystemInfo.OEMString:" + SpotWare.SystemInfo.OEMString);

            const string todo = "todo";
            SpotWare.HardwareProvider hwp = SpotWare.HardwareProvider.HwProvider;
            SpotBase.Debug.Print("GetAnalogChannelsCount:" + hwp.GetAnalogChannelsCount());
            SpotBase.Debug.Print("GetAnalogOutputChannelsCount:" + hwp.GetAnalogOutputChannelsCount());
            SpotBase.Debug.Print("GetAnalogOutputPinForChannel:" + todo);
            SpotBase.Debug.Print("GetAnalogPinForChannel:" + todo);
            SpotBase.Debug.Print("GetAvailableAnalogOutputPrecisionInBitsForChannel:" + todo);
            SpotBase.Debug.Print("GetAvailablePrecisionInBitsForChannel:" + todo);
            SpotBase.Debug.Print("GetButtonPins:" + hwp.GetButtonPins(SpotWare.Button.LastSystemDefinedButton));
            SpotBase.Debug.Print("GetPWMChannelsCount:" + hwp.GetPWMChannelsCount());
            SpotBase.Debug.Print("GetPinsCount:" + hwp.GetPinsCount());
            SpotBase.Debug.Print("GetPinsUsage:" + hwp.GetPinsUsage(SpotWare.Cpu.Pin.GPIO_Pin0));
            SpotBase.Debug.Print("GetPwmPinForChannel:" + todo);
            SpotBase.Debug.Print("GetSerialPortsCount:" + hwp.GetSerialPortsCount());
            SpotBase.Debug.Print("GetSpiPortsCount:" + hwp.GetSpiPortsCount());
            SpotBase.Debug.Print("GetSupportedInterruptModes:" + todo);
            SpotBase.Debug.Print("GetSupportedResistorModes:" + todo);
            SpotBase.Debug.Print("IsSupportedBaudRate:" + todo);
            SpotBase.Debug.Print("GetI2CPins:" + todo);
            SpotBase.Debug.Print("GetLCDMetrics:" + todo);

            SpotWare.Cpu.PinUsage[] pinUsuage;
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

        // https://www.ghielectronics.com/docs/48/fez-cobra-ii-developer#425
        private static void WiFiExample()
        {
            var wifi = new WiFiRS9110(SpotWare.SPI.SPI_module.SPI2, Pin.P1_10, Pin.P2_11, Pin.P1_9, 4000);

            if (!wifi.IsOpen)
            {
                // open the driver
                wifi.Open();
            }

            if (!wifi.NetworkInterface.IsDhcpEnabled)
            {
                wifi.NetworkInterface.EnableDhcp();
            }

            // do we have to do this, and if so, what does it do?
            NetworkInterfaceExtension.AssignNetworkingStackTo(wifi);

            // scan all Wifi channels for available networks
            WiFiNetworkInfo[] scanResults = wifi.Scan();

            // inspect the available WiFi networks
            SpotBase.Debug.Print("Wifi Scan Results");
            foreach (WiFiNetworkInfo r in scanResults)
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

            // join the specified network
            const string preSharedKey = "NutButter3";
            WiFiNetworkInfo targetWifiNetwork = scanResults[0];
            wifi.Join(targetWifiNetwork, preSharedKey);

            // check whether we are connected to a network
            SpotBase.Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);

            // todo Create a peer-to-peer connection for two way communication with FONTY (i.e. with Shaun's main Desktop Computer).
            wifi.StartAdHocHost(targetWifiNetwork.SSID, targetWifiNetwork.SecMode, preSharedKey,
                targetWifiNetwork.ChannelNumber);
        }
    }
}