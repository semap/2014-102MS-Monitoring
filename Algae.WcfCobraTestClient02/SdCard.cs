namespace Algae.WcfCobraTestClient02
{
    using System;
    using System.IO;
    using System.Text;
    using GHI.Premium.IO;
    using Microsoft.SPOT;
    using Microsoft.SPOT.IO;
    
    public static class SdCard
    {
        private static PersistentStorage storage;

        static SdCard()
        {
            storage = new PersistentStorage("SD");
            storage.MountFileSystem();
        }

        public static void WriteException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            if (VolumeInfo.GetVolumes().Length == 0)
            {
                return;
            }

            // Assume one storage device is available, 
            // access it through NETMF
            VolumeInfo volumeInfo = VolumeInfo.GetVolumes()[0];
            string rootDirectory = volumeInfo.RootDirectory;

            if (rootDirectory == null)
            {
                return;
            }

            // format the message
            StringBuilder builder = new StringBuilder();
            builder.Append("DateTime:" + DateTime.Now.ToString());
            builder.Append("\n");
            builder.Append(exception.ToString());

            try
            {
                // create a file
                FileStream fileStream =
                    new FileStream(rootDirectory + @"\exception-" + DateTime.Now.Ticks.ToString() + @".txt", FileMode.CreateNew);

                // encode message
                byte[] data = Encoding.UTF8.GetBytes(builder.ToString());

                // write the data and close the file
                fileStream.Write(data, 0, data.Length);

                // ensure the data is on the media
                fileStream.Flush();
                volumeInfo.FlushAll();

                fileStream.Close();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());                
            }
        }
    }
}
