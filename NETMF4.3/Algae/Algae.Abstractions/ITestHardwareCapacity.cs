using System;
using Microsoft.SPOT;

namespace Algae.Abstractions
{
    public interface ITestHardwareCapacity
    {
        bool TestWanHttp();

        bool TestWanPing();

        bool TestLanHttp();

        bool TestLanPing();

        bool TestDhcp();
    }
}
