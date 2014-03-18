using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfServiceLibrary
{
    // NOTE: make sure that this and the ServiceHost.Description.Namespace match
    [ServiceContract(Namespace = "http://Gadgeteer.WCF.Sample")]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    // NOTE: make sure that this and the ServiceHost.Description.Namespace match with the addition of .Data
    [DataContract(Namespace = "http://Gadgeteer.WCF.Sample.Data")]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        public override string ToString()
        {
            return String.Format("BoolValue: {0}, StringValue: {1}", BoolValue, StringValue);
        }
    }
}
