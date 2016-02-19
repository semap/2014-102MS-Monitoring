using Algae.Core;
using Algae.Hardware.G120;
using Sample.Application;

namespace Algae.Host
{
    public class Program
    {
        public static void Main()
        {
            //var flasher = new SbcFlasher();
            //while (true)
            //{
            //    System.Threading.Thread.Sleep(500);
            //    flasher.Flash();
            //}

            var logger = new SbcLogger();

            var application = new MyApplication(
                hardwareCapacityTester: new HardwareCapacityTester(logger),
                flasher: new SbcFlasher(),
                networkDriver: new SbcNetwork(logger),
                socketServer: new SocketServer());
        }
    }
}
