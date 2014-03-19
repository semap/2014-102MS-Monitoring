using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Algae.WcfServiceLibrary
{
    [ServiceContract]
    public interface IPersistenceSvc
    {
        /// <summary>
        /// Determine whether the network is connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        [OperationContract]
        bool IsConnected();

        /// <summary>
        /// Send data to the Persistence service.
        /// </summary>
        /// <param name="data">The data to send.</param>
        [OperationContract]
        void Send(SbcData[] data);
    }    
}
