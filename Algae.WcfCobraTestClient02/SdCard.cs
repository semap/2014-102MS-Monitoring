namespace Algae.WcfCobraTestClient02
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using GHI.Hardware.G120;
    using GHI.Premium.IO;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using Microsoft.SPOT.IO;

    public static class SdCard
    {
        private static InputPort sdDetectPin;

        static SdCard()
        {                                 
            sdDetectPin = new InputPort(Pin.P1_8, false, Port.ResistorMode.PullUp);
            new Thread(SDMountThread).Start();
        }

        public static void WriteException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            // get volume info
            VolumeInfo volumeInfo;
            if (TryGetVolumeInfo(out volumeInfo) == false)
            {
                return;
            }

            // get root directory
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
                // we cannot log the error to the SdCard
                // so just print it to the console
                Debug.Print(ex.ToString());
            }
        }

        private static bool TryGetVolumeInfo(out VolumeInfo volumeInfo)
        {
            volumeInfo = null;
            if (VolumeInfo.GetVolumes().Length == 0)
            {
                return false;
            }
            foreach (var vi in VolumeInfo.GetVolumes())
            {
                if (vi.Name.Equals("SD"))
                {
                    volumeInfo = vi;
                    break;
                }
            }
            if (volumeInfo == null)
            {
                return false;
            }
            return true;
        }

        private static void SDMountThread()
        {
            PersistentStorage sdPS = null;
            const int POLL_TIME = 500; // check every 500 millisecond

            bool sdExists;
            while (true)
            {
                try // If SD card was removed while mounting, it may throw exceptions
                {
                    sdExists = sdDetectPin.Read() == false;

                    // make sure it is fully inserted and stable
                    if (sdExists)
                    {
                        Thread.Sleep(50);
                        sdExists = sdDetectPin.Read() == false;
                    }

                    if (sdExists && sdPS == null)
                    {
                        // mount the sd card
                        sdPS = new PersistentStorage("SD");
                        sdPS.MountFileSystem();
                    }
                    else if (!sdExists && sdPS != null)
                    {
                        // unmount
                        sdPS.UnmountFileSystem();
                        sdPS.Dispose();
                        sdPS = null;
                    }
                }
                catch
                {
                    if (sdPS != null)
                    {
                        sdPS.Dispose();
                        sdPS = null;
                    }
                }

                Thread.Sleep(POLL_TIME);
            }
        }
    }
}
