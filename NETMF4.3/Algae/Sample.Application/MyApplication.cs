using System;
using Algae.Abstractions;
using Microsoft.SPOT;

// TODO Consider whether these assemblies/namespaces belongs 
// in the Application or not.
using Dpws.Device;
using Dpws.Device.Services;
using Ws.Services;
using Ws.Services.Binding;
using Algae.Schemas;
using Ws.Services.Xml;
using Algae.Schemas.MyService;

namespace Sample.Application
{
    public class MyApplication
    {
        public MyApplication(
            ITestHardwareCapacity hardwareCapacityTester,
            IMyService myService)
        {
            TestAllSystems(hardwareCapacityTester);
            StartService(myService);

            System.Diagnostics.Debugger.Break();
        }

        private void StartService(IMyService myService)
        {
            string guid = "urn:uuid:18571766-87df-06e2-bb68-5136c48f483a";
            ProtocolVersion version = new ProtocolVersion11();

            var binding = new WS2007HttpBinding(new HttpTransportBindingConfig(guid, 80));
            Device.Initialize(binding, version);

            Device.Host = new MyDeviceHost(version);

            Device.HostedServices.Add(new MyService(myService));
            var serverBindingContext = new ServerBindingContext(version);

            try
            {
                Device.Start(serverBindingContext);
            }
            catch (Exception ex)
            {
                Debug.Print("Blarg.");
                Debug.Print(ex.ToString());
                Debug.Print("End blarg.");

                System.Diagnostics.Debugger.Break();
            }

            System.Diagnostics.Debugger.Break();
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            Debug.EnableGCMessages(false);

            if (!hardwareCapacityTester.TestHttp())
            {
                Debug.Print("Http connectivity is down.");
            }
            else
            {
                Debug.Print("----");
                Debug.Print("All systems go.");
                Debug.Print("----");
            }
        }
    }

    public class MyDeviceHost : DpwsHostedService
    {
        public MyDeviceHost(ProtocolVersion v) : base(v)
        {
            // Set base service properties
            ServiceNamespace = new WsXmlNamespace("mys", "http://Algae.Schemas/MyService");
            ServiceID = "urn:uuid:3cb0d1ba-cc3a-46ce-b416-212ac2419b51";
            ServiceTypeName = "SimpleDeviceType";
        }
    }
}
