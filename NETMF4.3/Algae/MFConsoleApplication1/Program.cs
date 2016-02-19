using System;
using Algae.Hardware.G120;

namespace MFConsoleApplication1
{
    public class Program
    {
        public static void Main()
        {
            var flasher = new SbcFlasher();

            while (true)
            {
                System.Threading.Thread.Sleep(500);
                flasher.Flash();
            }
        }
    }
}
