using System;

// You will need to add a reference to this assembly
using System.ServiceModel;
using System.ServiceModel.Description;

// You will need to add a project reference to the wcf library assembly
using WcfServiceLibrary;

namespace WCFServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Type serviceHostType = typeof(Service1);
            Type serviceEndpointHostType = typeof(IService1);
            string endpointUrl = "http://localhost/GadgeteerWCFHost";

            // create the service host
            ServiceHost sh = 
                new ServiceHost(
                    serviceHostType,
                    new Uri(endpointUrl));

            // setup wsdl
            sh.Description.Behaviors.Add(
                new ServiceMetadataBehavior() 
                {
                    HttpGetEnabled = true 
                });

            // NOTE: set the namespace to match the service contract namespace
            sh.Description.Namespace = "http://Gadgeteer.WCF.Sample";

            // setup main endpoint
            sh.AddServiceEndpoint(
                serviceEndpointHostType,
                new WSHttpBinding(SecurityMode.None),
                String.Empty);
            
            sh.Open();

            Console.WriteLine("Service is running. Press any key to exit.");
            Console.ReadKey();

            sh.Close();
        }
    }
}
