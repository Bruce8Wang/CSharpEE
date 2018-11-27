using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;

namespace WinSrv
{
	class Program
	{
		public static void Main(string[] args)
		{
			ServiceBase.Run(new WindowsService());
		}
		
		public class WindowsService : ServiceBase
		{
			private Process process = null;
			
			protected override void OnStart(string[] args)
			{
				// 调用其他进程
				string fileName = ConfigurationManager.AppSettings["fileName"] ?? "";
				string arguments = ConfigurationManager.AppSettings["arguments"] ?? "";
				process = Process.Start(fileName, arguments);
			}

			protected override void OnStop()
			{
				// 杀死其他进程
				process.Kill();
			}
		}
	}
	
}