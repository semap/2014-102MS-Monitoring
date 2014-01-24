using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

// When you create your own project you need to add refrences to the following:
// System.Xml
// MFDpwsClient
// MFDpwsExtensions
// MFWsStack
using Ws.Services;
using Ws.Services.Binding;

// for service proxy client
// this changes if you update the namespaces in the service and service host and regenerate the proxies
using Gadgeteer.WCF.Sample;
using Gadgeteer.WCF.Sample.Data;

namespace GadgeteerWCFClient
{
    public partial class Program
    {
        private bool networkUp;

        void ProgramStarted()
        {
            // Generate the service proxy by doing the following:
            // "%ProgramFiles(x86)%\Microsoft .NET Micro Framework\v4.1\Tools\MFSvcUtil.exe" http://localhost/GadgeteerWCFHost?wsdl /d:C:\Temp /o:Service1
            // C:\Temp will have the following files:
            //      Service1.cs
            //      Service1ClientProxy.cs
            //      Service1HostedService.cs
            // Copy the first two into your gadgeteer project
            // Alternatively you can run the UpdateClientProxy.cmd from a command prompt window in the project folder.
   
            // setup ethernet
            ethernet_J11D.NetworkUp += NetworkUp;
            ethernet_J11D.UseDHCP();

            // the led will turn on while it is calling the WCF service and turn off when complete
            button.LEDMode = Button.LEDModes.Off;
            button.ButtonPressed +=
                (o, e) =>
                {
                    if (!networkUp)
                        return;

                    button.TurnLEDOn();

                    try
                    {
                        //*****
                        // TODO: change this to the IP address of your machine that is running the WCF service
                        //*****
                        string hostRunningWCFService = "192.168.1.179";
                        IService1ClientProxy proxy =
                            new IService1ClientProxy(
                                new WS2007HttpBinding(),
                                new ProtocolVersion11());
                        // NOTE: the endpoint needs to match the endpoint of the servicehost
                        proxy.EndpointAddress = "http://" + hostRunningWCFService + "/GadgeteerWCFHost";


                        // first call test
                        var data = proxy.GetData(
                            new GetData()
                            {
                                value = 12345
                            });
                        // should print 12345
                        Debug.Print(data.GetDataResult);


                        // second call test
                        var data1 = proxy.GetDataUsingDataContract(
                            new GetDataUsingDataContract()
                            {
                                composite = new CompositeType()
                                {
                                    BoolValue = true,
                                    StringValue = "String input"
                                }
                            });
                        // should print "String inputSuffix"
                        Debug.Print(data1.GetDataUsingDataContractResult.StringValue);
                    }
                    catch (System.IO.IOException)
                    {
                        Debug.Print("Error making WCF call");
                    }
                    finally
                    {
                        button.TurnLEDOff();
                    }
                };


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started. Wait till you see a message about the network being ready.");
        }

        private void NetworkUp(GTM.Module.NetworkModule sender, Gadgeteer.Modules.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("The network is up. Press the button to make WCF calls.");
            Debug.Print("IP Address: " + sender.NetworkSettings.IPAddress);
            networkUp = true;
        }
    }
}
