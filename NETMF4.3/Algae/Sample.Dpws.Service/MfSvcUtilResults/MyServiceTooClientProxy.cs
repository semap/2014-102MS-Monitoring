//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     .NET Micro Framework MFSvcUtil.Exe
//     Runtime Version:2.0.00001.0001
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Xml;
using Dpws.Client;
using Dpws.Client.Discovery;
using Dpws.Client.Eventing;
using Ws.Services;
using Ws.Services.Utilities;
using Ws.Services.Binding;
using Ws.Services.Soap;
using Ws.Services.WsaAddressing;
using Ws.Services.Xml;

namespace Algae.Schemas.MyServiceToo
{
    
    
    public class MyServiceTooClientProxy : DpwsClient
    {
        
        private IRequestChannel m_requestChannel = null;
        
        public MyServiceTooClientProxy(Binding binding, ProtocolVersion version) : 
                base(binding, version)
        {

            // Set client endpoint address
            m_requestChannel = m_localBinding.CreateClientChannel(new ClientBindingContext(m_version));
        }
        
        public virtual void Start()
        {

            // Create request header
            String action;
            action = "http://algae.schemas/MyServiceToo/IMyServiceToo/Start";
            WsWsaHeader header;
            header = new WsWsaHeader(action, null, EndpointAddress, m_version.AnonymousUri, null, null);
            WsMessage request = new WsMessage(header, null, WsPrefix.None);
            request.Method = "Start";


            // Send service request
            m_requestChannel.Open();
            m_requestChannel.RequestOneWay(request);
            m_requestChannel.Close();
        }
    }
}
