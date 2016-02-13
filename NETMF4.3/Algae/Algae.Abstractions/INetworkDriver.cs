using System;

namespace Algae.Abstractions
{
    public interface INetworkDriver
    {
        void InitializeNetwork();

        void InitializeServer();
    }
}
