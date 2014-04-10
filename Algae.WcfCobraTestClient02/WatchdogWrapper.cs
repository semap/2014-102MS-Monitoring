namespace Algae.WcfCobraTestClient02
{
    using System.Threading;
    using GHI.Premium.Hardware.LowLevel;

    public static class WatchdogWrapper
    {
        private const uint WatchdogTimeoutMs = 1000 * 10;
        private const int WatchdogResetMs = (int)WatchdogTimeoutMs - 1000;
        private static bool keepResettingWatchdog = true;
        private static Thread watchdogReset;

        public static void Watch()
        {
            // Enable Watchdog
            Watchdog.Enable(WatchdogTimeoutMs);

            // regularly reset the Watchdog
            watchdogReset = new Thread(WatchdogResetLoop);
            watchdogReset.Start();
        }

        public static void ForceReboot()
        {
            keepResettingWatchdog = false;
        }

        private static void WatchdogResetLoop()
        {
            while (keepResettingWatchdog)
            {
                Thread.Sleep(WatchdogResetMs);
                Watchdog.ResetCounter();
            }
        }
    }
}
