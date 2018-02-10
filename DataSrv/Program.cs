using System;
using System.Configuration;
using System.Data.Services;

namespace DataSrv
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddresses = new Uri[] { new Uri(ConfigurationManager.AppSettings["url"].ToString()) };
            using (var host = new DataServiceHost(typeof(Startup), baseAddresses))
            {
                host.Open();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}