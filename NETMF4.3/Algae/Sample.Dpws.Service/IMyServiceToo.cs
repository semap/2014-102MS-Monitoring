using Ws.ServiceModel;

namespace Sample.Dpws.Service
{
    [ServiceContract(Name = "MyServiceToo", Namespace = "http://Algae.Schemas")]
    public interface IMyServiceToo
    {
        [OperationContract(Name = "Start", IsOneWay = true)]
        void Start();
    }
}
