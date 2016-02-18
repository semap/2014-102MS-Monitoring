using System;
using System.IO;
using System.Text;
using Algae.Abstractions;
using Microsoft.SPOT.IO;

namespace Algae.Core
{
    public class SbcLogger : ILogger
    {
        // Emulator location - root/DOTNETMF_FS_EMULATION/WINFS/log-0000-0000-0000.txt
        private readonly string _filePath;

        public SbcLogger()
        {
            var fileName = "log-" + Guid.NewGuid().ToString() + ".txt";
            foreach (var info in VolumeInfo.GetVolumes())
            {
                if (info.IsFormatted)
                {
                     _filePath = Path.Combine(info.RootDirectory, fileName);
                    break;
                }
            }
        }

        public void Write(string message)
        {
            if (_filePath == null || _filePath.Length == 0)
            {
                // there is no formatted file system :(
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message + "\r\n");
            
            var fileMode = File.Exists(_filePath)
                ? FileMode.Append
                : FileMode.Create;

            var fileStream = new FileStream(_filePath, fileMode);

            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
    }
}
