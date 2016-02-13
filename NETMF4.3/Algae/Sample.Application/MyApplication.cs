using System;
using Algae.Abstractions;

namespace Sample.Application
{
    public class MyApplication
    {
        public MyApplication(
            ITestHardwareCapacity hardwareCapacityTester,
            INetworkDriver networkDriver)
        {
            networkDriver.InitializeNetwork();
            
            TestAllSystems(hardwareCapacityTester);

            while (true)
            { 
                
            }
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            hardwareCapacityTester.TestNetworkInterfaces();
            hardwareCapacityTester.TestHttpRequest(Proximity.WideAreaNetwork, "bigfont.ca");
            hardwareCapacityTester.TestHttpRequest(Proximity.LocalAreaNetwork, "192.168.1.148");
            hardwareCapacityTester.TestHttpRequest(Proximity.Self);
        }
    }
}
