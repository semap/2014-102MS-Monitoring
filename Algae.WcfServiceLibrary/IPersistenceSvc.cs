using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Algae.WcfServiceLibrary
{
    [ServiceContract(Name = "IPersistenceSvc", Namespace = "urn:SingularBiogenics/Aquaponics/2014/03")]
    public interface IPersistenceSvc
    {
        /// <summary>
        /// Determine whether the network is connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        [OperationContract(Name = "IsConnected")]
        bool IsConnected();

        /// <summary>
        /// Send data to the Persistence service.
        /// </summary>
        /// <param name="data">The data to send.</param>
        [OperationContract(Name = "Send")]
        void Send(SbcData[] data);
    }    
}
