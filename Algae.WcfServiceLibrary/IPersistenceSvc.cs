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
        [OperationContract]
        bool IsActive();
    }    
}
