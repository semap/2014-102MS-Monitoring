using Algae.Abstractions;

namespace Algae.Hardware.G120
{
    public class SbcFlasher : IFlasher
    {
        // LED
        private static Microsoft.SPOT.Hardware.OutputPort led1 =
            new Microsoft.SPOT.Hardware.OutputPort(GHI.Pins.G120.P1_15, true);

        public void Flash()
        {
            bool isOn = SbcFlasher.led1.Read();
            SbcFlasher.led1.Write(!isOn);
        }
    }
}
