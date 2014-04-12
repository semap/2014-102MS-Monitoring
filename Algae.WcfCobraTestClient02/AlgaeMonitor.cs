namespace Algae.WcfCobraTestClient02
{
    using System;
    using System.Threading;
    using schemas.datacontract.org.Algae.WcfServiceLibrary;

    public class AlgaeMonitor
    {
        private const int period = 1000;
        private int sendCounter = 0;
        private Network network;

        public AlgaeMonitor(Network network)
        {
            this.network = network;
        }

        public void SendContinuousTestDataAcrossNetwork()
        {
            // use a while loop with a sleep period
            // until we figure out how to propagate Exceptions from Timers and Threads to the main thread.            
            while (true)
            {
                Thread.Sleep(period);
                SbcData data = this.CreateSbcTestData();
                SbcData[] dataArray = new SbcData[] { data };
                this.network.Send(dataArray);
                SendComplete();
            }
        }

        private SbcData CreateSbcTestData()
        {
            SbcData data = new SbcData();
            data.Data = this.sendCounter.ToString();
            data.SensorGuid = new Guid().ToString();
            data.Timestamp = DateTime.Now;
            data.DataMetric = DataMetric.Celsius;
            data.DataType = DataType.Long;
            return data;
        }

        private void SendComplete()
        {
            this.sendCounter++;
            Flasher.Flash();
        }
    }
}
