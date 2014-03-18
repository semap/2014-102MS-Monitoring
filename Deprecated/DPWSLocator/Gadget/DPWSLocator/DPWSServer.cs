using System;
using Microsoft.SPOT;
using Dpws.Device;
using Ws.Services;
using Ws.Services.Binding;

namespace DPWSLocator
{
    /// <summary>
    /// Generic DPWS server that exposes a runtime defined service name in hello messages and probe responses
    /// </summary>
    public class DPWSServer
    {

        private ProtocolVersion version;

        /// <summary>
        /// Creates a generic DPWS server
        /// </summary>
        public DPWSServer( )
        {
            version = new ProtocolVersion11();
        }

        /// <summary>
        /// Set up the metadata for the device
        /// </summary>
        private void SetMetaData()
        {
            // Set device information
            Device.ThisModel.Manufacturer = "Microsoft Corporation";
            Device.ThisModel.ManufacturerUrl = "http://www.microsoft.com/";
            Device.ThisModel.ModelName = "Gadget Test Device";
            Device.ThisModel.ModelNumber = "1.0";
            Device.ThisModel.ModelUrl = "http://www.microsoft.com/";
            Device.ThisModel.PresentationUrl = "http://www.microsoft.com/";

            Device.ThisDevice.FriendlyName = "MyGadget";
            Device.ThisDevice.FirmwareVersion = "alpha";
            Device.ThisDevice.SerialNumber = "12345678";
        }

        /// <summary>
        /// Initilize bindings and the DPWS device
        /// </summary>
        private void Initialize()
        {
            // Initialize the binding
            Guid g = Guid.NewGuid();
            string urn = "urn:uuid:" + g.ToString();

            Device.Initialize(new WS2007HttpBinding(new HttpTransportBindingConfig(urn, 8084)), version);
        }

        /// <summary>
        /// Set up and start the DPWS service.
        /// Call this once the network has started.
        /// </summary>
        public void StartDPWS()
        {

            Initialize();

            SetMetaData();

            // Add a Host service type
            Device.Host = new DPWSHostService(version);

            // Set this device property if you want to ignore this clients request
            Device.IgnoreLocalClientRequest = false;

            ServerBindingContext sbContext = new ServerBindingContext(version);
            // Start the device
            Device.Start(sbContext);
        }
    }
}
