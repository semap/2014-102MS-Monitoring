namespace Algae.WatchdogTest01
{
    using System;
    using System.Threading;
    using GhiHardwareLowLevel = GHI.Premium.Hardware.LowLevel;
    using MsHardware = Microsoft.SPOT.Hardware;

    public class OtherProgram
    {
        private int period = 1000;
        private bool throwError = false;

        // LED
        private Microsoft.SPOT.Hardware.OutputPort led1 =
            new Microsoft.SPOT.Hardware.OutputPort(GHI.Hardware.G120.Pin.P1_15, true);

        // buttons
        private Microsoft.SPOT.Hardware.InterruptPort lrd1 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P0_22, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        private Microsoft.SPOT.Hardware.InterruptPort lrd0 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P2_10, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        public OtherProgram()
        {
            this.SetupButtons();
            this.Loop();
        }

        private void SetupButtons()
        {
            this.lrd0.OnInterrupt += this.OnInterrupt_ThrowFatalError;
            this.lrd1.OnInterrupt += this.OnInterrupt_ThrowFatalError;
        }

        private void OnInterrupt_ThrowFatalError(uint data1, uint data2, DateTime time)
        {
            this.throwError = true;
        }

        private void Loop()
        {
            while (true)
            {
                Thread.Sleep(this.period);
                if (this.throwError)
                {
                    this.ThrowError();
                }
                else
                {
                    this.Flash();
                }
            }
        }

        private void ThrowError()
        {
            object o = null;
            o.ToString();
        }

        private void Flash()
        {
            bool isOn = this.led1.Read();
            this.led1.Write(!isOn);
        }
    }

    public class Program
    {
        private const uint WatchdogTimeoutMs = 1000 * 3;
        private static bool keepResettingWatchdog = true;
        private static int watchdogResetMs = (int)WatchdogTimeoutMs - 1000;
        private static Thread WatchdogReset;

        public static void Main()
        {
            // Enable Watchdog
            GhiHardwareLowLevel.Watchdog.Enable(WatchdogTimeoutMs);

            // regularly reset the Watchdog
            WatchdogReset = new Thread(WatchdogResetLoop);
            WatchdogReset.Start();

            // start program
            try
            {
                OtherProgram p = new OtherProgram();
            }
            catch (Exception)
            {
                keepResettingWatchdog = false;
            }

            // prevent exiting
            Thread.Sleep(Timeout.Infinite);
        }

        private static void WatchdogResetLoop()
        {
            while (keepResettingWatchdog)
            {
                Thread.Sleep(watchdogResetMs);
                GhiHardwareLowLevel.Watchdog.ResetCounter();
            }
        }
    }
}
