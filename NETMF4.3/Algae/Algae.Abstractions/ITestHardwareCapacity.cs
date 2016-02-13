using System;

namespace Algae.Abstractions
{
    public interface ITestHardwareCapacity
    {
        // e.g. bigfont.ca
        bool TestWanHttp(string host);

        // e.g. 192.168.1.148
        bool TestLanHttp(string host);

        bool TestWanPing();

        bool TestLanPing();

        bool TestDhcp();
    }
}
