using Microsoft.Owin.Hosting;
using System;
using System.Configuration;

namespace PI.API
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(ConfigurationManager.AppSettings["url"]))
            {
                Console.WriteLine("Server is running , press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
