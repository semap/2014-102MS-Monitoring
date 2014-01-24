using System;

namespace WcfServiceLibrary
{
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            Console.WriteLine(String.Format("GetData: {0}", value));

            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            Console.WriteLine(String.Format("GetDataUsingDataContract: {0}", composite));

            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
