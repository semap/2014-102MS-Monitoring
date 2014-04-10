namespace Algae.WcfCobraTestClient02
{
    public static class Flasher
    {        
        // LED
        private static Microsoft.SPOT.Hardware.OutputPort led1 =
            new Microsoft.SPOT.Hardware.OutputPort(GHI.Hardware.G120.Pin.P1_15, true);

        public static void Flash()
        {
            bool isOn = Flasher.led1.Read();
            Flasher.led1.Write(!isOn);
        }
    }
}
