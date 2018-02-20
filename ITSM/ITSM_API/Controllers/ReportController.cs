using System.Collections.Generic;
using System.Linq;
using ITSM.Models;

namespace ITSM.Controllers
{
    public class ReportController
    {
        private ITSMModel db = new ITSMModel();
        // GET: Report
        public List<RepairApplyBill> GetReport()
        {
            var repairApplyBills = from b in db.RepairApplyBills
                                   from c in db.OnwayFlows
                                   where c.RepairAppyBillId == b.Id
                                   select new RepairApplyBill()
                                   {
                                       Id = b.Id,
                                       Title = b.Title,
                                       BillNo = b.BillNo,
                                       BXDate = b.BXDate,
                                       BXEmployee = b.BXEmployee,
                                       BXDept = b.BXDept,
                                       StatusId = b.StatusId,

                                       SatisfactionLevelId = b.SatisfactionLevelId,

                                       ApplyEmployee = b.ApplyEmployee,
                                       ApplyDept = b.BXDept,

                                       AssetCode = b.AssetCode,
                                       ComputerName = b.ComputerName,
                                       Phone = b.Phone,
                                       FaultTypeId = b.FaultTypeId,

                                       Note = b.Note
                                   };

            return repairApplyBills.ToList();

        }
    }
}