2014-102MS-Monitoring
=====================

1. Install Visual Studio 2013 Community.
2. Install NETMF 4.3.
3. Open `\NETMF4.3\Algae\Algae.sln` in Visual Studio 2013.
4. Right click on `Algae.Host.Emulator` and choose `Debug > Start new instance`. 

This will run the project in the NETMF emulator. In the Visual Studio output window, you will something like this: 

```
TestNetworkInterfaces
Network Interface             Enabled   IPAddress           SubnetMask          GatewayAddress
Wireless80211                 True      169.254.231.31      255.255.0.0         0.0.0.0
Ethernet                      True      192.168.1.148       255.255.255.0       192.168.1.1
Ethernet                      True      169.254.137.140     255.255.0.0         0.0.0.0
Unknown Network Interface     False     127.0.0.1           255.0.0.0           0.0.0.0
Unknown Network Interface     False     0.0.0.0             0.0.0.0             0.0.0.0
Unknown Network Interface     False     0.0.0.0             0.0.0.0             0.0.0.0
-----
TestHttpClient:1
Requesting bigfont.ca
Request succeeded.
-----
TestHttpClient:2
Requesting 192.168.1.148
Request succeeded.
-----
TestHttpClient:0
Requesting 127.0.0.1
The thread '<No Name>' (0x4) has exited with code 0 (0x0).
Request succeeded.
```
