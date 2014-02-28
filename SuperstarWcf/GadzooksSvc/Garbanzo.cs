using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using YahooWcf;

namespace GadzooksSvc
{
    public partial class Garbanzo : ServiceBase
    {
        internal static ServiceHost stunningServiceHost = null; 


        public Garbanzo()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (stunningServiceHost != null)
            {
                stunningServiceHost.Close();
            }
            stunningServiceHost = new ServiceHost(typeof(DuperSvc));
            stunningServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (stunningServiceHost != null)
            {
                stunningServiceHost.Close();
                stunningServiceHost = null;
            }
        }
    }
}
