namespace Algae.WatchdogTest01
{
    using System;
    using System.Threading;
    using GHI.Premium.Hardware.LowLevel;

    public class OtherProgram
    {
        private static Timer timer;
        private static int period = 1000;
     
        // LED
        private static Microsoft.SPOT.Hardware.OutputPort led1 = 
            new Microsoft.SPOT.Hardware.OutputPort(GHI.Hardware.G120.Pin.P1_15, true);

        // buttons
        private static Microsoft.SPOT.Hardware.InterruptPort lrd1 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P0_22, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        private static Microsoft.SPOT.Hardware.InterruptPort lrd0 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P2_10, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        public OtherProgram()
        {
            lrd0.OnInterrupt += OnInterrupt_ThrowFatalError;
            lrd1.OnInterrupt += OnInterrupt_ThrowFatalError;
            timer = new Timer(new TimerCallback(TimerCallback), new object(), 0, period);
        }

        private static void OnInterrupt_ThrowFatalError(uint data1, uint data2, DateTime time)
        {
            throw new OutOfMemoryException("What's up");        
        }

        private void TimerCallback(object stateInfo)
        {
            Flash();
        }

        private void Flash()
        {
            bool isOn = led1.Read();
            led1.Write(!isOn);
        }
    }

    public class Program
    {
        private const uint WatchdogTimeoutMs = 6000;
        private static Timer timer;
        private static int period = (int)WatchdogTimeoutMs - 1000;
        
        public static void Main()
        {
            // Enable Watchdog
            Watchdog.Enable(WatchdogTimeoutMs);

            // regularly reset the Watchdog
            timer = new Timer(new TimerCallback(TimerCallback), new object(), 0, period);

            // start program
            OtherProgram p = new OtherProgram();

            // prevent exiting
            Thread.Sleep(Timeout.Infinite);
        }

        private static void TimerCallback(object state)
        {
            Watchdog.ResetCounter();
        }
    }
}
