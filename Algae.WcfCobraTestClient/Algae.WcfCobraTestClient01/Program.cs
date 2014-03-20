//-----------------------------------------------------------------------
// <copyright file="SbcProgram.cs" company="Singular Biogenics">
//     Copyright (c) Singular Biogenics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Algae.WcfCobraTestClient01
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using GHI.Hardware.G120;
    using GHI.Premium.Net;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using schemas.datacontract.org.Algae.WcfServiceLibrary;
    using tempuri.org;
    using Ws.Services.Binding;

    public class Program
    {
        private const int ModerateTimespan = 1000;

        // timer is static to prevent garbage collection
        // see also http://stackoverflow.com/questions/477351/in-c-where-should-i-keep-my-timers-reference
        private static Timer timer;
        private int sendCounter = 0;
        private Network network;
        
        public static void Main()
        {
            Program p = new Program();
            p.network = new Network();
            p.RepeatedlySendDataToWcfService();
            Thread.Sleep(Timeout.Infinite);
        }

        private void RepeatedlySendDataToWcfService()
        {
            Program.timer = new Timer(this.TimerCallback_SendSbcData, new object(), 0, Program.ModerateTimespan);            
        }

        private void TimerCallback_SendSbcData(object stateInfo)
        {
            Debug.Print("Send");
            SbcData[] data = new SbcData[] 
                {
                    new SbcData() 
                    {
                        Data = this.sendCounter.ToString(), 
                        SensorGuid = new Guid().ToString(),
                        Timestamp = DateTime.Now,
                        DataMetric = DataMetric.Celsius,
                        DataType = DataType.Long
                    }
                };
            this.network.Send(data);
        }
    }
}
