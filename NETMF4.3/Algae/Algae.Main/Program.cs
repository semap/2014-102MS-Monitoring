using Algae.Core;
using Sample.Application;

namespace Algae.Main
{
    public class Program
    {
        public static void Main()
        {
            var application = new MyApplication(
                new HardwareCapacityTester());
        }
    }
}
