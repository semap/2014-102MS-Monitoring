using System;
using Microsoft.SPOT;

namespace Algae.Abstractions
{
    public interface ILogger
    {
        void Write(string message);
    }
}
