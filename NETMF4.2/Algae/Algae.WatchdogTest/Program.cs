namespace Algae.WatchdogTest
{
    using Microsoft.SPOT.Hardware;
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main()
        {
            Watchdog.Behavior = WatchdogBehavior.DebugBreak_Managed;
            Watchdog.Enabled = true;
            Watchdog.Timeout = new TimeSpan(0, 0, 5); // five seconds

            TimerCallback(new object());

            Thread.Sleep(Timeout.Infinite);
        }

        private static OutputPort led1 = new OutputPort(GHI.Hardware.G120.Pin.P1_15, true);
        private static int period = 1000;
        private static Timer timer;
        private static void TimerCallback(object stateInfo)
        {
            timer = new Timer(TimerCallback, new object(), period, Timeout.Infinite);

            period = period - 100;
            if (period == 0)
            {
                period = 1000;
            }

            Flash();
        }

        private static void Flash()
        {
            bool isOn = led1.Read();
            led1.Write(!isOn);
        }
    }
}
