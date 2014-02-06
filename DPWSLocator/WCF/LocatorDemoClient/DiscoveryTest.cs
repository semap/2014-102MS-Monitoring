using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFGadgetLocator;

namespace LocatorDemoClient
{
    public partial class DiscoveryTest : Form
    {
        private Locator _locator;

        public DiscoveryTest()
        {
            InitializeComponent();
        }

        void _locator_OnHelloEvent(object sender, System.ServiceModel.Discovery.EndpointDiscoveryMetadata metadata)
        {
            labelAnnouncement.Text = metadata.ListenUris[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelProbeResult.Text = "Probing ...";
            var svcFound = _locator.FindService();
            labelProbeResult.Text = (svcFound == null) ? "Not Found" : svcFound.ListenUris[0].AbsoluteUri.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _locator = new Locator(typeof(IGadgetService));
            _locator.OnHelloEvent += _locator_OnHelloEvent;
            _locator.ListenForAnnouncements();
        }
    }
}
