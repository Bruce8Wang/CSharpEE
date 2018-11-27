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
				//执行逻辑或调用其他进程
				process = Process.Start(ConfigurationManager.AppSettings["fileName"] ?? "", ConfigurationManager.AppSettings["arguments"] ?? "");
			}

			protected override void OnStop()
			{
				//执行逻辑或杀死其他进程
				process.Kill();
			}
		}
	}
	
}