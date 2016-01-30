namespace Algae.WcfCobraTestClient01
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using GHI.Hardware.G120;
    using GHI.Premium.Net;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using schemas.datacontract.org.Algae.WcfServiceLibrary;
    using tempuri.org;
    using Ws.Services.Binding;

    public class Network
    {
        private const string ZeroIpAddress = "0.0.0.0";
        private const string WcfServiceEndpointUri = "http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/";

        // ethernet
        private EthernetENC28J60 eth;
        private bool hasIpAddress = false;

        // flash led
        private OutputPort led1 = new OutputPort(GHI.Hardware.G120.Pin.P1_15, true);
        private bool doFlashing = false;

        // buttons
        private InterruptPort lrd1 =
            new InterruptPort(GHI.Hardware.G120.Pin.P0_22, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        private InterruptPort lrd0 =
            new InterruptPort(GHI.Hardware.G120.Pin.P2_10, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        // wcf proxy
        private IPersistenceSvcClientProxy proxy;

        public Network()
        {
            this.SetupButtonPressEvents();
            this.InitializeFezCobraIIEthernetPort();
        }

        private IPersistenceSvcClientProxy Proxy
        {
            get
            {
                if (this.proxy == null)
                {
                    this.proxy = this.CreateWcfProxy();
                }

                return this.proxy;
            }
        }

        internal void Send(SbcData[] data)
        {
            // todo Write a better implementation of checking for connection before sending.
            if (this.hasIpAddress)
            { 
                try
                {
                    this.ConnectWcfProxy();
                    this.SendDataToWcfServiceViaHttp(data);
                    this.FlashLed();
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }
        }

        #region Button Press Events for Testing

        private void FlashLed()
        {
            if (this.doFlashing)
            {
                bool isOn = this.led1.Read();
                this.led1.Write(!isOn);
            }
            else
            {
                this.led1.Write(false);
            }
        }

        private void DoFlashing(uint data1, uint data2, DateTime time)
        {
            this.doFlashing = true;
        }

        private void DoNotDoFlashing(uint data1, uint data2, DateTime time)
        {
            this.doFlashing = false;
        }

        private void SetupButtonPressEvents()
        {
            this.lrd0.OnInterrupt += this.DoFlashing;
            this.lrd1.OnInterrupt += this.DoNotDoFlashing;
        }

        #endregion

        private void InitializeFezCobraIIEthernetPort()
        {
            this.eth = new EthernetENC28J60(SPI.SPI_module.SPI2, Pin.P1_10, Pin.P2_11, Pin.P1_9, 4000);

            this.eth.NetworkAddressChanged += this.Eth_NetworkAddressChanged;
            this.eth.CableConnectivityChanged += this.Eth_CableConnectivityChanged;

            if (!this.eth.IsOpen)
            {
                this.eth.Open();
            }

            NetworkInterfaceExtension.AssignNetworkingStackTo(this.eth);

            if (!this.eth.NetworkInterface.IsDhcpEnabled)
            {
                this.eth.NetworkInterface.EnableDhcp();
            }

            // Required to kick DHCP into action
            this.eth.NetworkInterface.RenewDhcpLease();
        }

        #region Send Data to WCF Service

        /// <summary>
        /// Create a Wcf Proxy
        /// </summary>
        private IPersistenceSvcClientProxy CreateWcfProxy()
        {
            Uri serviceUri = new System.Uri(Network.WcfServiceEndpointUri);
            HttpTransportBindingConfig config = new HttpTransportBindingConfig(serviceUri);
            WS2007HttpBinding binding = new WS2007HttpBinding(config);
            IPersistenceSvcClientProxy proxy = new IPersistenceSvcClientProxy(binding, new Ws.Services.ProtocolVersion11());
            return proxy;
        }

        /// <summary>
        /// Keep trying to connect to the Wcf service until connected.
        /// </summary>
        private void ConnectWcfProxy()
        {
            var isConnected = false;
            while (!isConnected)
            {
                try
                {
                    IsConnectedResponse resp = this.Proxy.IsConnected(new IsConnected());
                    isConnected = resp.IsConnectedResult;
                }
                catch (Exception)
                {
                    isConnected = false;
                }
            }
        }

        private void SendDataToWcfServiceViaHttp(SbcData[] sbcDataArray)
        {
            if (this.proxy.IsConnected(new IsConnected()).IsConnectedResult)
            {
                // this garbage collecting is just for testing object lifetime
                // it probably slows things down to do it every loop
#if ManuallyCollectGarbage
                Debug.GC(true);
                GC.WaitForPendingFinalizers();
#endif
                Send send = new Send();
                send.data = new schemas.datacontract.org.Algae.WcfServiceLibrary.ArrayOfSbcData();
                send.data.SbcData = sbcDataArray;
                this.proxy.Send(send);
            }
        }

        #endregion

        #region Ethernet Events

        private void Eth_CableConnectivityChanged(object sender, EthernetENC28J60.CableConnectivityEventArgs e)
        {
            Debug.Print("Network cable " + (e.IsConnected ? "Connected" : "Disconnected"));
        }

        // DHCP will assign an IP address to the adapter sometime after startup. 
        // This can be 20 to 30 seconds or more depending on the network.
        private void Eth_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("DHCP assigned IP address: " + this.eth.NetworkInterface.IPAddress);
            if (this.eth.NetworkInterface.IPAddress != ZeroIpAddress)
            {
                this.hasIpAddress = true;
            }
            else
            {
                this.hasIpAddress = false;
            }
        }

        #endregion

        private void HandleException(Exception ex)
        {
            Debug.Print(ex.Message);
        }
    }
}
