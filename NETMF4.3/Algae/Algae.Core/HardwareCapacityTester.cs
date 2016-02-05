using System;
using Algae.Abstractions;
using Microsoft.SPOT;

namespace Algae.Core
{
    public class HardwareCapacityTester : ITestHardwareCapacity
    {
        public bool TestHttp()
        {
            var result = false;

            try
            {
                var html = SocketClient.GetWebPage("http://www.bigfont.ca", 80);
                result = html.IndexOf("BigFont") > 0;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            return result;
        }
    }
}
