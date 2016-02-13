using System;
using System.Threading;
using Algae.Abstractions;

namespace Sample.Application
{
    public class MyApplication
    {
        public static ISocketServer _socketServer;

        public MyApplication(
            ITestHardwareCapacity hardwareCapacityTester,
            INetworkDriver networkDriver,
            ISocketServer socketServer)
        {
            networkDriver.InitializeNetwork();

            _socketServer = socketServer;
            new Thread(socketServer.Start).Start();
            
            TestAllSystems(hardwareCapacityTester);
            
            while (true)
            { 
                // prevent program exit :)    
            }
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            hardwareCapacityTester.TestNetworkInterfaces();
            hardwareCapacityTester.TestHttpRequest(Proximity.WideAreaNetwork, "bigfont.ca");
            hardwareCapacityTester.TestHttpRequest(Proximity.LocalAreaNetwork, "192.168.1.148");
            hardwareCapacityTester.TestHttpRequest(Proximity.Self, "127.0.0.1", 12000);
        }
    }
}
