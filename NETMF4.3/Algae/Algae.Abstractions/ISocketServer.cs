using System;
using Microsoft.SPOT;

namespace Algae.Abstractions
{
   public  interface ISocketServer
    {
        void Start();

        void Stop();
    }
}
