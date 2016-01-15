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

### ...in the Services Management Console
1. Run ```services.msc```
1. Find AlgaePersistenceSvc
1. Check its Log On As value (e.g. Local System / Network Service / Other)
1. Start the Service if it isn't already.
    - If it doesn't start, 
    - then give its Log On permissions to the EXE directory.

### ...with a browser.
1. At http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/
    - you will see the WCF Help page for PersistenceSvc
1. At http://192.168.1.102/Algae.WcfServiceLibrary/PersistenceSvc/mex.wsdl
    - you will see the WCF Metadata for PersistenceSvc
1. During troubleshooting, 
    - it is worth visiting these links from another computer on your LAN

### ...with the WcfTestClient.
1. Open a Developer Command Prompt.
1. Run "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\WcfTestClient.exe"
1. The WCFTestClient GUI will open.
1. File > Add Service > Use the Svc mex.wsdl address (above).
1. You will be able to call the service's methods.

## Deploy to the Fez Cobra II Net

### Refresh the Fez Cobra II

This step is necessary if you want to delete all existing software except NETMF.

1. Plug in the Fez Cobra II into the PC via USB. 
1. Run Fez Config on your PC. 
1. Ensure the device is connected to Fez Config.
1. Choose Advanced > Loader (Tiny Booter) Update > Fez Cobra II
1. Follow the instructions to wipe the device.

### Deploy the Softare

1. Plug the Cobra II into the PC via USB.
1. Plug the Cobra II into the LAN via Ethernet.
1. Right click on Algae.WcfCobraTestClient01 > Deploy
1. The NETMF project will deploy.
1. Restart the Cobra II.
1. The Cobra II will send data to the WCF Service every one second.
