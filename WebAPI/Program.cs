using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Threading;

namespace com.example.demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(ConfigurationManager.AppSettings["url"]))
            {
                Console.WriteLine("Server is running , press Ctrl + C to exit.");
                var _quitEvent = new ManualResetEvent(false);
                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    _quitEvent.Set();
                    eArgs.Cancel = true;
                };
                _quitEvent.WaitOne();
            }
        }
    }
}
