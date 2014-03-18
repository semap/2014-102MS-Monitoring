using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlgaeWcfSvcLibrary
{
    public class PersistenceSvc : IPersistenceSvc
    {
        public bool IsActive()
        {
            return true;
        }
    }
}
