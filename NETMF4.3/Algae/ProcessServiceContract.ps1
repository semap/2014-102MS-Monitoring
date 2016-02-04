
# To run do `.\ProcessServiceContract.ps1`

$library = "C:\Program Files (x86)\Microsoft .NET Micro Framework\v4.3\Assemblies\le";

$svcProjectDir = "C:\GitHub\2014-102MS-Monitoring\NETMF4.3\Algae\Sample.Dpws.Service";
$svcAssemblyDir = $svcProjectDir + "\bin\Debug\Sample.Dpws.Service.dll";

$destinationDir = ".\MfSvcUtilResults";


# Setup Directories
cd $svcProjectDir;
New-Item -ItemType Directory $destinationDir -Force

# Create WSDL File
mfsvcutil $svcAssembly /R:$library /D:$destinationDir 

# Generate Code from WSDL File

Set-Location $destinationDir

Get-ChildItem -Filter *.wsdl | ForEach-Object {  mfsvcutil $_.Name /P:Microframework }

Set-Location ..
