#define UseTestClass

namespace Algae.WcfCobraTestClient02
{
    using System;

    public class Program
    {
        public static void Main()
        {
            WatchdogWrapper.Watch();

            // put any code that we want to watch in the try block
            // this mimics global error handling
            try
            {
#if UseTestClass
                    WatchdogTestClass p = new WatchdogTestClass();
#else
                    AlgaeMonitor m = new AlgaeMonitor(new Network());
                    m.SendContinuousTestDataAcrossNetwork();
#endif
            }
            catch (Exception ex)
            {
                // reboot on unhandle exceptions
                SdCard.WriteException(ex);
                WatchdogWrapper.ForceReboot();
            }
        }
    }
}
