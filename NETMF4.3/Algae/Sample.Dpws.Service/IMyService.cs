using Ws.ServiceModel;

namespace Sample.Dpws.Service
{
    [ServiceContract(Name = "MyService", Namespace = "http://Algae.Schemas")]
    public interface IMyService
    {
        [OperationContract(Name = "Start", IsOneWay = true)]
        void Start();
    }
}
