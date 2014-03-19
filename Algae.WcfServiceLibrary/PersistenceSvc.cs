using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Algae.WcfServiceLibrary
{
    public class PersistenceSvc : IPersistenceSvc
    {
        public bool IsConnected()
        {
            return true;
        }

        public void Send(SbcData[] data)        
        {
            if (data == null || data.Length == 0 || data[0] == null)
            {
                data = CreateTestSbcDataArray();
            }
            
            for (int i = 0; i < data.Length; ++i)
            {
                string datumString = ConvertDatumToString(data[i]);
                AppendTextToAnExistingFile(datumString);
            }
        }

        #region Helpers

        private string ConvertDatumToString(SbcData datum)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("SensorGuid:{0}", datum.SensorGuid);
            builder.AppendLine();

            builder.AppendFormat("Timestamp:{0}", datum.Timestamp);
            builder.AppendLine();

            builder.AppendFormat("Data:{0}", datum.Data);
            builder.AppendLine();

            builder.AppendFormat("DataType:{0}", datum.DataType);
            builder.AppendLine();

            builder.AppendFormat("DataMetric:{0}", datum.DataMetric);
            builder.AppendLine();

            return builder.ToString();
        }

        private SbcData[] CreateTestSbcDataArray()
        {
            SbcData[] data = new SbcData[] {
                    new SbcData() {
                        SensorGuid = new Guid().ToString(),
                        Timestamp = DateTime.Now,
                        Data = "12",
                        DataType = typeof(Int16),
                        DataMetric = DataMetric.Celsius
                    }
                };
            return data;
        }

        private const string sendLogFileName = "sendLog.txt";
        // see http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
        private const string friendlyDateTimeFormat = "ddd dd MMM yyyy @ hh:mm:ss tt";
        private void AppendTextToAnExistingFile(string textToAppend)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logFile = Path.Combine(baseDirectory, sendLogFileName);

            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFile, true))
            {
                file.WriteLine("-----");
                file.WriteLine(textToAppend);
            }
        }

        #endregion
    }
}
