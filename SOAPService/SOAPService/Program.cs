using CustomMiddleware;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel;

namespace SOAPService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => { services.AddScoped<CalculatorService>(); })
                .Configure(app=> { app.UseSOAPMiddleware<CalculatorService>("/CalculatorService", new BasicHttpBinding()); })
                .Build()
                .Run();
        }
    }
}
