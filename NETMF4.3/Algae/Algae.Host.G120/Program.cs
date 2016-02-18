using Algae.Core;
using Algae.Hardware.G120;
using Sample.Application;

namespace Algae.Main
{
    public class Program
    {
        public static void Main()
        {
            var logger = new SbcLogger();

            var application = new MyApplication(
                hardwareCapacityTester: new HardwareCapacityTester(logger),
                flasher : new SbcFlasher(),
                networkDriver: new SbcNetwork(logger),
                socketServer: new SocketServer());
        }
    }
}
