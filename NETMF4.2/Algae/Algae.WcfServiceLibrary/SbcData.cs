using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Algae.WcfServiceLibrary
{
    /// <summary>
    /// The metric of data.
    /// </summary>
    [DataContract(Name = "DataMetric", IsReference = false)]
    public enum DataMetric
    {
        [EnumMember]
        Celsius,
        [EnumMember]
        OnOff,
        [EnumMember]
        Moles
    }

    /// <summary>
    /// The C# data type of the data.
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ya5y69ds.aspx"/>
    [DataContract(Name = "DataType", IsReference = false)]
    public enum DataType
    {
        /// <summary>
        /// System.Boolean
        /// </summary>
        [EnumMember]
        Bool,
        /// <summary>
        /// System.Byte
        /// </summary>
        [EnumMember]
        Byte,
        /// <summary>
        /// System.SByte
        /// </summary>
        [EnumMember]
        Sbyte,
        /// <summary>
        /// System.Char
        /// </summary>
        [EnumMember]
        Char,
        /// <summary>
        /// System.Decimal
        /// </summary>
        [EnumMember]
        Decimal,
        /// <summary>
        /// System.Double
        /// </summary>
        [EnumMember]
        Double,
        /// <summary>
        /// System.Single
        /// </summary>
        [EnumMember]
        Float,
        /// <summary>
        /// System.Int32
        /// </summary>
        [EnumMember]
        Int,
        /// <summary>
        /// System.UInt32
        /// </summary>
        [EnumMember]
        Uint,
        /// <summary>
        /// System.Int64
        /// </summary>
        [EnumMember]
        Long,
        /// <summary>
        /// System.UInt64
        /// </summary>
        [EnumMember]
        Ulong,
        /// <summary>
        /// System.Object
        /// </summary>
        [EnumMember]
        Object,
        /// <summary>
        /// System.Int16
        /// </summary>
        [EnumMember]
        Short,
        /// <summary>
        /// System.UInt16
        /// </summary>
        [EnumMember]
        Ushort,
        /// <summary>
        /// System.String
        /// </summary>
        [EnumMember]
        String
    }

    /// <summary>   
    /// </summary>
    [DataContract(Name = "SbcData", IsReference = false)]
    public class SbcData
    {
        /// <summary>
        /// The unique identifier of the sensor
        /// </summary>
        [DataMember(Name = "SensorGuid", Order = 1, IsRequired = false, EmitDefaultValue = true)]
        public string SensorGuid { get; set; }
        /// <summary>
        /// The actual data collected        
        /// </summary>
        /// <remarks>
        /// In the future, a byte[] might be better than a string
        /// </remarks>        
        [DataMember(Name = "Data", Order = 2, IsRequired = false, EmitDefaultValue = true)]
        public string Data { get; set; }
        /// <summary>
        /// The time at which the sensor collected data
        /// </summary>
        [DataMember(Name = "Timestamp", Order = 3, IsRequired = false, EmitDefaultValue = true)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The data type of the Data e.g. Int, Long, String
        /// </summary>        
        [DataMember(Name = "DataType", Order = 4, IsRequired = false, EmitDefaultValue = true)]
        public DataType DataType { get; set; }

        /// <summary>
        /// The metric of the Data e.g. Celsius
        /// </summary>
        [DataMember(Name = "DataMetric", Order = 5, IsRequired = false, EmitDefaultValue = true)]
        public DataMetric DataMetric { get; set; }

    }
}
