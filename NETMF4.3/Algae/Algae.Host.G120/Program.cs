using Algae.Core;
using Algae.Hardware.G120;
using Sample.Application;

namespace Algae.Main
{
    public class Program
    {
        public static void Main()
        {
            var application = new MyApplication(
                hardwareCapacityTester: new HardwareCapacityTester(logger:new SbcLogger()),
                flasher : new SbcFlasher(),
                networkDriver: new SbcNetwork(logger: new SbcLogger()),
                socketServer: new SocketServer());
        }
    }
}
