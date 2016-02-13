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
            hardwareCapacityTester.TestDhcp();
            hardwareCapacityTester.TestWanViaHttp("bigfont.ca");
            hardwareCapacityTester.TestLanViaHttp("192.168.1.148");
        }
    }
}
