using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlgaeWcfServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PersistenceSvc" in both code and config file together.
    public class PersistenceSvc : IPersistenceSvc
    {
        public bool IsActive()
        {
            return true;
        }       
    }
}
