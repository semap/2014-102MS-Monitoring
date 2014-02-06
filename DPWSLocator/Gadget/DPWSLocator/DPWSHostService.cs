using System;
using Microsoft.SPOT;
using Dpws.Device.Services;
using Ws.Services;
using Ws.Services.WsaAddressing;
using Ws.Services.Xml;

namespace DPWSLocator
{
    class DPWSHostService : DpwsHostedService
    {
        private String serviceTypeName = "IMyCustomGadget";

        public DPWSHostService(ProtocolVersion v)
            : base(v)
        {
            // Add ServiceNamespace. Set ServiceID and ServiceTypeName
            ServiceNamespace = new WsXmlNamespace("h", "http://schemas.example.org/" + serviceTypeName);
            ServiceID = "urn:uuid:3cb0d1ba-cc3a-46ce-b416-212ac2419b51";
            ServiceTypeName = serviceTypeName;
        }
    }
}
