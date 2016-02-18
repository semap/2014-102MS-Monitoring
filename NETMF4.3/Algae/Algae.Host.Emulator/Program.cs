using Algae.Core;
using Algae.Hardware.Emulator;
using Sample.Application;

namespace Algae.Main
{
    public class Program
    {
        public static void Main()
        {
            var application = new MyApplication(
                hardwareCapacityTester: new HardwareCapacityTester(logger: new SbcLogger()),
                flasher: new SbcFlasher(),
                networkDriver: new SbcNetwork(),
                socketServer: new SocketServer());
        }
    }
}
