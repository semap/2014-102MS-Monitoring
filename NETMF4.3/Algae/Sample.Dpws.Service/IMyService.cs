using Ws.ServiceModel;

namespace Sample.Dpws.Service
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        void Start();
    }
}
