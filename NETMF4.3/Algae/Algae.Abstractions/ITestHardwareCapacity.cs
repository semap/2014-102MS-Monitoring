using System;

namespace Algae.Abstractions
{
    public interface ITestHardwareCapacity
    {
        // e.g. bigfont.ca
        bool TestWanViaHttp(string host);

        // e.g. 192.168.1.148
        bool TestLanViaHttp(string host);

        bool TestWanViaPing();

        bool TestLanViaPing();

        bool TestDhcp();
    }
}
