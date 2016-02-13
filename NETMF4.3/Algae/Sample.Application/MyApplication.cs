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
            hardwareCapacityTester.TestWanViaHttpRequestToRemoteHost("bigfont.ca");
            hardwareCapacityTester.TestLanViaHttpRequestToRemoteHost("192.168.1.148");
        }
    }
}
