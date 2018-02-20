using System.Data.Entity;
using System.Diagnostics;

namespace ITSM.Models
{

    public partial class ITSMModel : DbContext
    {
        public ITSMModel()
            : base("name=ITSMModel")
        {
            this.Database.Log = s => Debug.WriteLine(s);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<DealMethod> DealMethods { get; set; }
        public DbSet<FlowConfig> FlowConfigs { get; set; }
        public DbSet<SatisfactionLevel> SatisfactionLevels { get; set; }
        public DbSet<FaultType> FaultTypes { get; set; }
        public DbSet<RepairApplyBill> RepairApplyBills { get; set; }
        public DbSet<OnwayFlow> OnwayFlows { get; set; }
        public DbSet<SuperUser> SuperUsers { get; set; }
        public DbSet<Priority> Prioritys { get; set; }
        public DbSet<PrinterPermission> PrinterPermissions { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }
        

        public System.Data.Entity.DbSet<Contact> Contacts { get; set; }

        public DbSet<IssueTrack> IssueTracks { get; set; }
    }
}
