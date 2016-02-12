using System;
using Microsoft.SPOT;

namespace Algae.Abstractions
{
    public interface ITestHardwareCapacity
    {
        bool TestWanHttp(string url);

        bool TestWanPing();

        bool TestLanHttp();

        bool TestLanPing();

        bool TestDhcp();
    }
}
