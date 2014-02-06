using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace WCFGadgetLocator
{
    public class Locator
    {

        public Locator(Type serviceType)
        {
            DeviceFindCriteria = new FindCriteria(serviceType); 
        }


        /// <summary>
        /// Initiate a probe for a service of type contractType. 
        /// This uses DiscoveryVersion.WSDiscovery11
        /// </summary>
        /// <returns>EndpointDiscoveryMetadata of the located service or null</returns>
        public EndpointDiscoveryMetadata FindService()
        {

            try
            {
                var endPoint = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscovery11);

                //http://social.msdn.microsoft.com/Forums/en-US/wcf/thread/c08c55c6-784a-4896-abfa-ea5299a03cfa
                //http://support.microsoft.com/kb/2777305
                endPoint.Behaviors.Add(new WorkaroundBehavior());

                DiscoveryClient discoveryClient = new DiscoveryClient(endPoint);

                Collection<EndpointDiscoveryMetadata> services = discoveryClient.Find(DeviceFindCriteria).Endpoints;

                discoveryClient.Close();

                if (services.Count != 0)
                {
                    return services[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public delegate void HelloEventHandler(Object sender, EndpointDiscoveryMetadata metadata);

        /// <summary>
        /// This event is fired when a service is located by a probe either from a timer or when a
        /// matching Hello message is received
        /// </summary>
        public event HelloEventHandler OnHelloEvent;


        /// <summary>
        /// Service host for the announcement service listening for hello messages
        /// </summary>
        private ServiceHost announcementServiceHost;

        /// <summary>
        /// Find criteria used to identify the Device in hello messages
        /// </summary>
        public FindCriteria DeviceFindCriteria { get; private set; }

        public void ListenForAnnouncements()
        {
            // Create an AnnouncementService instance
            AnnouncementService announcementService = new AnnouncementService();

            // Subscribe the announcement events
            announcementService.OnlineAnnouncementReceived += announcementService_OnlineAnnouncementReceived;
            // announcementService.OfflineAnnouncementReceived += OnOfflineEvent;

            // Create ServiceHost for the AnnouncementService
            announcementServiceHost = new ServiceHost(announcementService);

            // Listen for the announcements sent over UDP multicast
            announcementServiceHost.AddServiceEndpoint(new UdpAnnouncementEndpoint());
            announcementServiceHost.Open();

        }


        /// <summary>
        /// Match incoming announcements and fire the OnHello event
        /// if the raising service matches our gadget
        /// </summary>
        /// <param name="sender">this</param>
        /// <param name="e">The Announcement Event Args from ServiceModel</param>
        private void announcementService_OnlineAnnouncementReceived(object sender, AnnouncementEventArgs e)
        {

            var metaData = e.EndpointDiscoveryMetadata;

            if (DeviceFindCriteria.IsMatch(metaData))
            {
                // This is our device host
                if (OnHelloEvent != null)
                {
                    OnHelloEvent(this, metaData);
                }
                return;
            }

        }
    }
}
