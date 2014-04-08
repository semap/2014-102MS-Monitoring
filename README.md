2014-102MS-Monitoring
=====================

# Usuage

## Install the hosted WCF Service

1. Rebuild Algae.WindowsService in Release configuration
    - If it doesn't build, 
    - then run ```services.msc``` to stop the AlgaePersistenceSvc.
1. Open a Developer Command Prompt at ..\Algae.WindowsService\bin\Release 
1. Run ```installutil Algae.WindowsService.exe``` (i.e. the EXE name).
    - If it's already installed,
    - then first run ```installutil Algae.WindowsService.exe -u```
    
## Test the hosted WCF Service

1. View it in the Services Management Console
    - Run ```services.msc```
    - Find AlgaePersistenceSvc
    - Check its Log On As value (e.g. Local System / Network Service / Other)
    - Start the Service if it isn't already.
        - If it doesn't start, 
        - then give its Log On permissions to the EXE directory.
1. Test with a browser.
    - At http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/
    - you will see the WCF Help page for PersistenceSvc
    - At http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/mex.wsdl
    - You will see the WCF Metadata for PersistenceSvc
    - During troubleshooting, 
    - it is worth visiting these links from another computer on your LAN
- Test with the WcfTestClient.
    - Open a Developer Command Prompt.
    - Run "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\WcfTestClient.exe"
    - The WCFTestClient GUI will open.
    - File > Add Service > Use the Svc mex.wsdl address (above).
    - You will be able to call the service's methods.
