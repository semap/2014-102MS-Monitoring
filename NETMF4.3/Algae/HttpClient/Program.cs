using System;
using Microsoft.SPOT;

namespace HttpClient
{
    public class Program
    {
        const string WideAreaNetworkHttpAddress = "http://www.bigfont.ca";
        const string LocalAreaNetworkHttpAddress = "http://10.10.40.124"; // e.g. localhost on emulator's computer

        public static void Main()
        {
            Debug.EnableGCMessages(false);

            Get(WideAreaNetworkHttpAddress, "BigFont");
            Get(LocalAreaNetworkHttpAddress, "IIS Windows");
        }

        public static void Get(string address, string htmlContentTest)
        {
            Debug.Print("Getting " + address.ToString() + ":");

            var html = HttpClient.GetWebPage(address, 80);
            var success = html.IndexOf(htmlContentTest) > 0;
            Debug.Print(success.ToString());
        }
    }
}
