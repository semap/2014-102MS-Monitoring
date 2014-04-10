namespace Algae.WcfCobraTestClient02
{
    using System;

    public class Program
    {
        public static void Main()
        {
            WatchdogWrapper.Watch();

            // start program
            try
            {
                ////WatchdogTestClass p = new WatchdogTestClass();

                AlgaeMonitor m = new AlgaeMonitor(new Network());
                m.SendContinuousTestDataAcrossNetwork();
            }
            catch (Exception ex)
            {
                SdCard.WriteException(ex);
                WatchdogWrapper.ForceReboot();
            }
        }
    }
}
