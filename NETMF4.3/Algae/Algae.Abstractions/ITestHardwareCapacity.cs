using System;
using Microsoft.SPOT;

namespace Algae.Abstractions
{
    public interface ITestHardwareCapacity
    {
        bool TestWanHttp();

        bool TestDhcp();

        bool TestLanHttp();
    }
}
