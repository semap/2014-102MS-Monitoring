using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Algae.WindowsService
{
    // Provide the ProjectInstaller class which allows 
    // the service to be installed by the Installutil.exe tool
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            // events
            this.AfterInstall += ProjectInstaller_AfterInstall;

            // service process
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            Installers.Add(serviceProcessInstaller);

            // service
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "AlgaePersistenceSvc";
            Installers.Add(serviceInstaller);            
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
