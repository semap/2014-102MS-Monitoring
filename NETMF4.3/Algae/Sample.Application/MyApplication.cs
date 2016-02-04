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
}
