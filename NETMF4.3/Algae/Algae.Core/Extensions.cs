using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Algae.Core.Extensions.NETMF
{
    public static class Extensions
    {
        public static string GetName(this NetworkInterfaceType enumValue)
        {
            // TODO Make this generic via reflection. 

            var name = string.Empty;

            switch (enumValue)
            {
                case NetworkInterfaceType.Ethernet:
                    name = "Ethernet";
                    break;

                case NetworkInterfaceType.Wireless80211:
                    name = "Wireless80211";
                    break;

                case NetworkInterfaceType.Unknown:
                default:
                    name = "Unknown Network Interface";
                    break;
            }

            return name;
        }
    }
}
