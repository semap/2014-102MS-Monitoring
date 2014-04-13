////#define UseTestClass
////#define ManuallyCollectGarbage
#define UseWatchdog

namespace Algae.WcfCobraTestClient02
{
    using System;

    public class Program
    {
        public static void Main()
        {
#if UseWatchdog
            WatchdogWrapper.Watch();
#endif

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
#if UseWatchdog
                WatchdogWrapper.ForceReboot();
#endif
            }
        }
    }
}
