namespace Algae.WcfCobraTestClient01
{
    using System;
    using Microsoft.SPOT;
    using GHI.Premium.IO;
    using Microsoft.SPOT.IO;
    using System.IO;
    using System.Text;

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
            // Assume one storage device is available, 
            // access it through NETMF
            VolumeInfo volumeInfo = VolumeInfo.GetVolumes()[0];
            string rootDirectory = volumeInfo.RootDirectory;

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
            catch(Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }
    }
}
