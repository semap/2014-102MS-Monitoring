using System;

namespace Algae.Abstractions
{
    public enum Proximity
    { 
        Self,
        WideAreaNetwork,
        LocalAreaNetwork
    }

    public interface ITestHardwareCapacity
    {
        // e.g. bigfont.ca
        bool TestHttpRequest(Proximity proximity, string host, int port = 80);

        bool TestPing();

        bool TestNetworkInterfaces();
    }
}
