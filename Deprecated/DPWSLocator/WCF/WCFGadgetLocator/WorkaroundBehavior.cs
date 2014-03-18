using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WCFGadgetLocator
{
    /// <summary>
    /// workaround for issue where probes interact with announcements and cause a storm of
    /// soap faults:
    ///     http://social.msdn.microsoft.com/Forums/en-US/wcf/thread/c08c55c6-784a-4896-abfa-ea5299a03cfa
    /// </summary>
    class WorkaroundBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime == null)
            {
                throw new ArgumentNullException("clientRuntime");
            }
            clientRuntime.CallbackDispatchRuntime.UnhandledDispatchOperation.Invoker = new UnhandledActionOperationInvoker();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        class UnhandledActionOperationInvoker : IOperationInvoker
        {
            public bool IsSynchronous
            {
                get
                {
                    return true;
                }
            }

            public object[] AllocateInputs()
            {
                return new object[1];
            }

            public object Invoke(object instance, object[] inputs, out object[] outputs)
            {
                outputs = new object[0];
                return Message.CreateMessage(MessageVersion.None, string.Empty);
            }

            public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
            {
                throw new NotImplementedException();
            }
        }
    }
}
