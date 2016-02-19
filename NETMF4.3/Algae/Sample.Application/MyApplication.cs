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
            IFlasher flasher,
            INetworkDriver networkDriver,
            ISocketServer socketServer)
        {
            networkDriver.InitializeNetwork();

            _socketServer = socketServer;
            new Thread(socketServer.Start).Start();
            
            // TestAllSystems(hardwareCapacityTester);
            
            while (true)
            {
                Thread.Sleep(1000);

                flasher.Flash();

                hardwareCapacityTester.TestHttpRequest(
                    Proximity.LocalAreaNetwork, "192.168.1.148",
                    5000); // Kestrel

                flasher.Flash();
            }
        }

        private void TestAllSystems(ITestHardwareCapacity hardwareCapacityTester)
        {
            // this takes about one to five minutes.

            hardwareCapacityTester.TestNetworkInterfaces();
            hardwareCapacityTester.TestHttpRequest(Proximity.WideAreaNetwork, "bigfont.ca"); // Azure
            hardwareCapacityTester.TestHttpRequest(Proximity.LocalAreaNetwork, "192.168.1.148", 80); // IIS
            hardwareCapacityTester.TestHttpRequest(Proximity.LocalAreaNetwork, "192.168.1.148", 5000); // Kestrel
            hardwareCapacityTester.TestHttpRequest(Proximity.Self, "127.0.0.1", 12000); // Sbc
        }
    }
}
