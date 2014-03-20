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
        private static int ModerateTimespan = 1000;

        // timer is static to prevent garbage collection
        // see also http://stackoverflow.com/questions/477351/in-c-where-should-i-keep-my-timers-reference
        private static Timer timer;
        private static int sendCounter = 0;
        private static Network network;
        

        public static void Main()
        {
            Program.network = new Network();
            RepeatedlySendDataToWcfService();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void RepeatedlySendDataToWcfService()
        {
            timer = new Timer(Program.TimerCallback_SendSbcData, new object(), 0, Program.ModerateTimespan);            
        }

        private static void TimerCallback_SendSbcData(object stateInfo)
        {
            Debug.Print("Send");
            SbcData[] data = new SbcData[] 
                {
                    new SbcData() 
                    {
                        Data = Program.sendCounter.ToString(), 
                        SensorGuid = new Guid().ToString(),
                        Timestamp = DateTime.Now,
                        DataMetric = DataMetric.Celsius,
                        DataType = DataType.Long
                    }
                };
            Program.network.Send(data);
        }
    }
}