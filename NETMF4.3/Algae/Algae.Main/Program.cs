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
                new HardwareCapacityTester(),
                new SbcNetwork());
        }
    }
}
