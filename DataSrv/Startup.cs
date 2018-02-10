using DataSrv.Models;
using System.Data.Services;
using System.Data.Services.Common;

namespace DataSrv
{
    public class Startup: DataService<NewDbContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
    }
}