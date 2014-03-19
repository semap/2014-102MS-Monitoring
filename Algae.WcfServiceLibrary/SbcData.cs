using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Algae.WcfServiceLibrary
{
    /// <summary>
    /// Allows the client to specify the metric of data.
    /// </summary>
    [DataContract(Name = "DataMetric", Namespace = "urn:SingularBiogenics/Schema/2014/03", IsReference = false)]
    public enum DataMetric
    {
        [EnumMember]
        Celsius,
        [EnumMember]
        OnOff,
        [EnumMember]
        Moles
    }

    [DataContract(Name = "SbcData", Namespace = "urn:SingularBiogenics/Schema/2014/03", IsReference = false)]
    public class SbcData
    {
        /// <summary>
        /// uniquely identifies the sensor
        /// </summary>
        [DataMember(Name = "SensorGuid", Order = 1, IsRequired = false, EmitDefaultValue = true)]
        public string SensorGuid { get; set; }
        /// <summary>
        /// the actual data collected (a byte[] might be better than a string)
        /// </summary>
        [DataMember(Name = "Data", Order = 2, IsRequired = false, EmitDefaultValue = true)]
        public string Data { get; set; }
        /// <summary>
        /// the time at which the sensor collected data
        /// </summary>
        [DataMember(Name = "Timestamp", Order = 3, IsRequired = false, EmitDefaultValue = true)]
        public DateTime Timestamp { get; set; }


        /// <summary>
        /// the data type of the Data e.g. Integer, String, Float, Decimal
        /// </summary>        
        ////[DataMember]
        ////public Type DataType { get; set; }
        /////// <summary>
        /////// the metric of the Data e.g. Celsius
        /////// </summary>
        ////[DataMember]
        ////public DataMetric DataMetric { get; set; }

    }
}
