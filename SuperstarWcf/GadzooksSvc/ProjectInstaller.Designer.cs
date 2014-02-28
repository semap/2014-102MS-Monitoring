namespace GadzooksSvc
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GarbanzoSvcProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.GarbanzoSvcInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // GarbanzoSvcProcessInstaller
            // 
            this.GarbanzoSvcProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.GarbanzoSvcProcessInstaller.Password = null;
            this.GarbanzoSvcProcessInstaller.Username = null;
            this.GarbanzoSvcProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // GarbanzoSvcInstaller
            // 
            this.GarbanzoSvcInstaller.ServiceName = "Garbanzo";
            this.GarbanzoSvcInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.GarbanzoSvcInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GarbanzoSvcProcessInstaller,
            this.GarbanzoSvcInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller GarbanzoSvcProcessInstaller;
        private System.ServiceProcess.ServiceInstaller GarbanzoSvcInstaller;
    }
}