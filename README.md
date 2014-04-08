2014-102MS-Monitoring
=====================

# Usuage

1. Rebuild Algae.WindowsService in Release configuration
    - If it doesn't build, 
    - then run ```services.msc``` to stop the AlgaePersistenceSvc.
1. Open a Developer Command Prompt at ..\Algae.WindowsService\bin\Release 
1. Run ```installutil Algae.WindowsService.exe``` (i.e. the EXE name).
    - if it's already installed,
    - then first run ```installutil Algae.WindowsService.exe -u```
