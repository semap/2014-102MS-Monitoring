using System;
using System.Net;
using GHI.Hardware.G120;
using GHI.Premium.Net;
using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using Microsoft.SPOT.Hardware;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.HelloWorld));

            PrintDeviceInfo();

            var wifi = CreateAndOpenNewWiFiRs9110Proxy();

            ConnectWifiToAccessPoint(wifi, "BlueMaple", "NutButter3");

            RetrievePageFromWebServer(wifi, "http://www.bigfont.ca");

            DisconnectWifiFromAccessPoint(wifi);

            SetupAdHocHost(wifi, "MyAdhocHost");

            Debug.Print("Done");
        }

        private static void PrintDeviceInfo()
        {
            const string todo = "todo";

            // volumn info
            Debug.Print("Volumes:" + VolumeInfo.GetVolumes().Length);

            // hardware info
            HardwareProvider hwp = HardwareProvider.HwProvider;
            Debug.Print("SystemInfo.Version:" + SystemInfo.Version);
            Debug.Print("SystemInfo.OEMString:" + SystemInfo.OEMString);

            // pins
            Cpu.PinUsage[] pinUsuage;
            int pinCount;
            hwp.GetPinsMap(out pinUsuage, out pinCount);
            Debug.Print("GetPinsMap:" + pinCount);
            Debug.Print("GetSerialPins:" + todo);
            Debug.Print("GetSpiPins:" + todo);

            // other hardware stuff
            Debug.Print("GetSupportBaudRates:" + todo);
            Debug.Print("GetAnalogChannelsCount:" + hwp.GetAnalogChannelsCount());
            Debug.Print("GetAnalogOutputChannelsCount:" + hwp.GetAnalogOutputChannelsCount());
            Debug.Print("GetAnalogOutputPinForChannel:" + todo);
            Debug.Print("GetAnalogPinForChannel:" + todo);
            Debug.Print("GetAvailableAnalogOutputPrecisionInBitsForChannel:" + todo);
            Debug.Print("GetAvailablePrecisionInBitsForChannel:" + todo);
            Debug.Print("GetButtonPins:" + hwp.GetButtonPins(Button.LastSystemDefinedButton));
            Debug.Print("GetPWMChannelsCount:" + hwp.GetPWMChannelsCount());
            Debug.Print("GetPinsCount:" + hwp.GetPinsCount());
            Debug.Print("GetPinsUsage:" + hwp.GetPinsUsage(Cpu.Pin.GPIO_Pin0));
            Debug.Print("GetPwmPinForChannel:" + todo);
            Debug.Print("GetSerialPortsCount:" + hwp.GetSerialPortsCount());
            Debug.Print("GetSpiPortsCount:" + hwp.GetSpiPortsCount());
            Debug.Print("GetSupportedInterruptModes:" + todo);
            Debug.Print("GetSupportedResistorModes:" + todo);
            Debug.Print("IsSupportedBaudRate:" + todo);
            Debug.Print("GetI2CPins:" + todo);
            Debug.Print("GetLCDMetrics:" + todo);
        }

        private static WiFiRS9110 CreateAndOpenNewWiFiRs9110Proxy()
        {
            // the wifi module is MOD2 and we connect via an SPI
            const SPI.SPI_module mod2 = SPI.SPI_module.SPI2;

            // why do we choose 1_10 as the chipset pin? 
            const Cpu.Pin chipSelect = Pin.P1_10;

            // why do we choose 2_11 as the external interupt pin?
            const Cpu.Pin externalInterupt = Pin.P2_11;

            // why do we choose 1_9 as the reset pin?
            const Cpu.Pin reset = Pin.P1_9;

            // why do we choose 4000?
            const uint clockRateKhz = 4000;

            // create a wifi object
            var wifi = new WiFiRS9110(mod2, chipSelect, externalInterupt, reset, clockRateKhz);

            if (!wifi.IsOpen)
            {
                // open the driver
                wifi.Open();
            }

            wifi.WirelessConnectivityChanged += new WiFiRS9110.WirelessConnectivityChangedEventHandler(WiFi_Changed);

            // we ALWAYS have to class AssignNetworkingStackTo() because, 
            // "Currently, only one interface can access the TCP/IP stack at a time. Use this function to assign the TCP/IP stack to a certain [wifi] interface."
            NetworkInterfaceExtension.AssignNetworkingStackTo(wifi);

            return wifi;
        }

        private static void DisconnectWifiFromAccessPoint(WiFiRS9110 wifi)
        {
            if (wifi.IsLinkConnected)
            {
                wifi.Disconnect();
            }
        }

        private static void RetrievePageFromWebServer(WiFiRS9110 wifi, string url)
        {
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            if (request == null)
            {
                return;
            }

            request.KeepAlive = true;

            WebResponse resp = null;

            try
            {
                resp = request.GetResponse();
            }
            catch (Exception e)
            {
                Debug.Print("Exception in HttpWebRequest.GetResponse(): " + e.ToString());
            }

            if (resp != null && resp.ContentLength > 0)
            {
                Debug.Print("There was a response");
            }
        }

        private static void ConnectWifiToAccessPoint(WiFiRS9110 wifi, string ssid, string preSharedKey)
        {
            if (!wifi.NetworkInterface.IsDhcpEnabled)
            {
                // use the router to assign dynamic ip addresses
                wifi.NetworkInterface.EnableDhcp();
            }

            // get the target ssid network
            WiFiNetworkInfo[] scanResults = wifi.Scan(ssid);
            if (scanResults.Length == 0)
            {
                Debug.Print(ssid + " was not available. Try one of these:");

                scanResults = wifi.Scan();

                // inspect the available WiFi networks                
                foreach (WiFiNetworkInfo r in scanResults)
                {
                    Debug.Print("-----");
                    Debug.Print("SSID:" + r.SSID);
                    Debug.Print("ChannelNumber:" + r.ChannelNumber);
                    Debug.Print("PhysicalAddress:" + r.PhysicalAddress);
                    Debug.Print("RSSI:" + r.RSSI);
                    Debug.Print("SecMode:" + r.SecMode);
                    Debug.Print("networkType:" + r.networkType);
                }
            }
            else
            {
                // check whether we are connected to a network
                Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);

                // join a network i.e. we are joining BlueMaple
                WiFiNetworkInfo targetWifiNetwork = scanResults[0];
                wifi.Join(targetWifiNetwork, preSharedKey);

                // check whether we are connected to a network
                Debug.Print("IsLinkConnected:" + wifi.IsLinkConnected);
            }
        }

        private static void SetupAdHocHost(WiFiRS9110 wifi, string hostName)
        {
            wifi.StartAdHocHost(hostName, SecurityMode.Open, " ", 10);
            wifi.NetworkInterface.EnableStaticIP("169.254.0.200", "255.255.0.0", "169.254.0.1");
        }

        private static void WiFi_Changed(object sender, WiFiRS9110.WirelessConnectivityEventArgs e)
        {
            Debug.Print("isConnected = " + e.IsConnected);
            Debug.Print("Info = " + e.NetworkInformation); // this is null        
        }
    }
}