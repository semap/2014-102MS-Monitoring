namespace Algae.WcfCobraTestClient02
{
    using System;
    using System.Threading;

    public class WatchdogTestClass
    {
        private int period = 1000;
        private bool throwError = false;

        // buttons
        private Microsoft.SPOT.Hardware.InterruptPort lrd1 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P0_22, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        private Microsoft.SPOT.Hardware.InterruptPort lrd0 =
            new Microsoft.SPOT.Hardware.InterruptPort(GHI.Hardware.G120.Pin.P2_10, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullUp, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);

        public WatchdogTestClass()
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
                    Flasher.Flash();
                }
            }
        }

        private void ThrowError()
        {
            object o = null;
            o.ToString();
        }
    }
}
