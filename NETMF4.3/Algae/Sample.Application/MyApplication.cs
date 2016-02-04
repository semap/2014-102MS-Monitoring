using System;
using Algae.Abstractions;
using Microsoft.SPOT;

// TODO Consider whether these assemblies/namespaces belongs 
// in the Application or not.
using Dpws.Device;
using Dpws.Device.Services;
using Ws.Services;
using Ws.Services.Binding;

namespace Sample.Application
{
    public class MyApplication
    {
        public MyApplication(
            ITestHardwareCapacity hardwareCapacityTester,
            IDpwsSimpleService simpleDpwsService)
        {
            TestAllSystems(hardwareCapacityTester);
            StartService(simpleDpwsService);
        }

        private void StartService(IDpwsSimpleService simpleDpwsService)
        {
            Device.HostedServices.Add(new MyHostedService());

            var version = new ProtocolVersion11();
            var serverBindingContext = new ServerBindingContext(version);
            Device.Start(serverBindingContext);
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            if (!hardwareCapacityTester.TestHttp())
            {
                throw new NotSupportedException("Http connectivity is down.");
            }

            Debug.EnableGCMessages(false);
            Debug.Print("----");
            Debug.Print("All systems go.");
            Debug.Print("----");
            System.Diagnostics.Debugger.Break();
        }
    }

    public class MyHostedService : DpwsHostedService
    { 
    
    }
}
