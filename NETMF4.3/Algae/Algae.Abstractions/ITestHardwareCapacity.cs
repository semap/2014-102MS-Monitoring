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
        bool TestHttpRequest(Proximity proximity, string host = "");

        bool TestPing();

        bool TestNetworkInterfaces();
    }
}
