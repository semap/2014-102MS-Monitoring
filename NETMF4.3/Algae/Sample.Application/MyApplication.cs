using System;
using Algae.Abstractions;
using Microsoft.SPOT;

namespace Sample.Application
{
    public class MyApplication
    {
        public MyApplication(ITestHardwareCapacity hardwareCapacityTester)
        {
            TestAllSystems(hardwareCapacityTester);

            while (true)
            { 
                
            }
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            Debug.EnableGCMessages(false);
            hardwareCapacityTester.TestWanHttp();
            hardwareCapacityTester.TestLanHttp();
            hardwareCapacityTester.TestDhcp();
        }
    }
}
