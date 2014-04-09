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

        // static to prevent garbage collection
        // see also http://stackoverflow.com/questions/477351/in-c-where-should-i-keep-my-timers-reference
        private static Timer timer;                

        private Network network;
        private int sendCounter = 0;
        private OutputPort led1 = new OutputPort(GHI.Hardware.G120.Pin.P1_15, true);

        public static void Main()
        {
            try
            {
                Program p = new Program();
            }
            catch (Exception ex)
            {
                SdCard.WriteException(ex.Message);
            }

            Thread.Sleep(Timeout.Infinite);
        }

        public Program()
        {
            Program.timer = new Timer(this.TimerCallback, new object(), 0, Program.ModerateTimespan);

            this.network = new Network();
            this.sendCounter = 0;
        }

        private void TimerCallback(object stateInfo)
        {
            try
            {
                // force garbage collection
                // to learn about what needs to be static
                Debug.GC(true);

                this.sendCounter++;
                this.FlashLed();
                this.SendSbcData();
            }
            catch (Exception ex)
            {
                SdCard.WriteException(ex.Message);
            }
        }

        private void SendSbcData()
        {
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

        private void FlashLed()
        {
            bool isOn = led1.Read();
            this.led1.Write(!isOn);
        }
    }
}
