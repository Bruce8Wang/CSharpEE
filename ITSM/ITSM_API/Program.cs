using System;
using System.Configuration;
using Microsoft.Owin.Hosting;

namespace ITSM
{
	class Program
	{
static void Main(string[] args)
{
	using (WebApp.Start<Startup>(ConfigurationManager.AppSettings["url"]))
	{
		Console.WriteLine("Server is running , press exit to exit.");
		while (true)
		{
			if (Console.ReadLine() == "exit")
			{
				break;
			}
		}
	}
}
	}
}
