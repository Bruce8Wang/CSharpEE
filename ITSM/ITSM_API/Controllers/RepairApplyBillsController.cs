using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ITSM.Models;

namespace ITSM.Controllers
{
    public class RepairApplyBillsController : ApiController
    {
        private ITSMModel db = new ITSMModel();
        // GET: ../api/RepairApplyBills 
        //[EnableQuery]
        public IQueryable<RepairApplyBillDTO> GetRepairApplyBills()
        {
            var repairApplyBills = from b in db.RepairApplyBills
                                   select new RepairApplyBillDTO()
                                   {
                                       Id = b.Id,
                                       Title = b.Title,
                                       BillNo = b.BillNo,
                                       BXDate = b.BXDate,
                                       BXEmployee = b.BXEmployee,
                                       BXDept = b.BXDept,
                                       StatusId = b.StatusId,
                                       Status = b.Status,
                                       StatusName = b.Status.Name,
                                       SatisfactionLevelId = b.SatisfactionLevelId,
                                       SatisfactionLevel = b.SatisfactionLevel,
                                       SatisfactionLevelName = b.SatisfactionLevel.Name,
                                       ApplyEmployee = b.ApplyEmployee,
                                       ApplyDept = b.ApplyDept,
                                       BXDealEmployee = b.BXDealEmployee,
                                       BXDealTime = b.BXDealTime,
                                       BXDealNote = b.BXDealNote,
                                       BXDealProcess = b.BXDealProcess,
                                       NextEmployee = b.NextEmployee,
                                       AssetCode = b.AssetCode,
                                       ComputerName = b.ComputerName,
                                       Phone = b.Phone,
                                       EMail = b.EMail,
                                       FaultTypeId = b.FaultTypeId,
                                       FaultType = b.FaultType,
                                       FaultTypeName = b.FaultType.Name,
                                       Note = b.Note,
                                       PrevOperation = b.PrevOperation,
                                       PriorityId = b.Priority.Id,
                                       Priority = b.Priority,
                                       PriorityName = b.Priority.Name,
                                       HopeTime = b.HopeTime,
                                       ImagePath = b.ImagePath,
                                       VedioPath = b.VedioPath,
                                       DeviceType = b.DeviceType
                                   };

            return repairApplyBills;
        }

        //Get:../api/RepairApplyBills/1
        [ResponseType(typeof(RepairApplyBillDTO))]
        public async Task<IHttpActionResult> GetRepairApplyBill(int id)
        {
            var repairApplyBill = from b in db.RepairApplyBills
                                  where b.Id == id
                                  select new RepairApplyBillDTO()
                                  {
                                      Id = b.Id,
                                      Title = b.Title,
                                      BillNo = b.BillNo,
                                      BXDate = b.BXDate,
                                      BXEmployee = b.BXEmployee,
                                      BXDept = b.BXDept,
                                      StatusId = b.StatusId,
                                      StatusName = b.Status.Name,
                                      SatisfactionLevelId = b.SatisfactionLevelId,
                                      SatisfactionLevelName = b.SatisfactionLevel.Name,
                                      ApplyEmployee = b.ApplyEmployee,
                                      ApplyDept = b.ApplyDept,
                                      BXDealEmployee = b.BXDealEmployee,
                                      BXDealTime = b.BXDealTime,
                                      BXDealNote = b.BXDealNote,
                                      BXDealProcess = b.BXDealProcess,
                                      NextEmployee = b.NextEmployee,
                                      AssetCode = b.AssetCode,
                                      ComputerName = b.ComputerName,
                                      Phone = b.Phone,
                                      EMail = b.EMail,
                                      FaultTypeId = b.FaultTypeId,
                                      FaultTypeName = b.FaultType.Name,
                                      Note = b.Note,
                                      PrevOperation = b.PrevOperation,
                                      PriorityId = b.Priority.Id,
                                      PriorityName = b.Priority.Name,
                                      HopeTime = b.HopeTime,
                                      ImagePath = b.ImagePath,
                                      VedioPath = b.VedioPath,
                                      DeviceType = b.DeviceType
                                  };

            //RepairApplyBillDTO RepairApplyBill = await db.RepairApplyBills.FindAsync(id);
            //if (RepairApplyBill == null)
            //{
            //    return NotFound();
            //}
            if (repairApplyBill == null)
            {
                return NotFound();
            }
            return Ok(repairApplyBill);
        }

        //PUT:../api/RepairApplyBills/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRepairApplyBill(int id, RepairApplyBill RepairApplyBill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != RepairApplyBill.Id)
            {
                return BadRequest();
            }

            db.Entry(RepairApplyBill).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!RepairApplyBillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            { }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: ../api/RepairApplyBills
        [ResponseType(typeof(RepairApplyBill))]
        public async Task<IHttpActionResult> PostRepairApplyBill(RepairApplyBill RepairApplyBill)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            //process.StartInfo.FileName = "E:\\Project(杨勇杰)\\Oracle相关\\操作截图.jpg";
            //process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
            //process.StartInfo.UseShellExecute = true;
            //process.Start();

            string ITMaintenanceMobile = ConfigurationManager.AppSettings["ITMaintenanceMobile"].ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {


                //long? max = (from t in db.RepairApplyBills
                //             select (long?)t.Id).Max();
                //long m = 0;
                //if (!max.HasValue)
                //{
                //    m = 1;
                //}
                //else
                //    m = (long)max + 1; 

                //RepairApplyBill.BillNo = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();

                //string filepath = HttpContext.Current.Request.Form["fileField"];

                //HttpPostedFile hpf = HttpContext.Current.Request.Files[0];

                //hpf.SaveAs(HttpContext.Current.Request.MapPath("/") + "\\uploadFiles");

                db.RepairApplyBills.Add(RepairApplyBill);
                await db.SaveChangesAsync();
                Dictionary<string, string> dict = new Dictionary<string, string>();

                dict = Common.getHelpDesk();
                //if (dict["Mobile"].Length == 0)
                //{
                //    Common.sendMessage("17722680637", dict["Dealer"] + "的手机号码未维护");
                //}
                //else
                //{
                //    Common.sendMessage(dict["Mobile"], "你有待处理报修单，提单号：" + RepairApplyBill.BillNo);
                //}
                //应锤总要求,修改为：发短信给所有运维人员
                Common.sendMessage(ITMaintenanceMobile, string.Format("报修主题：{3},报修人：{0}[{1}],中心：{2}", RepairApplyBill.BXEmployee, RepairApplyBill.Phone, RepairApplyBill.BXDept, RepairApplyBill.Title));
                Common.sendMail("IT报修系统邮件通知", "<a href='http://app.sztechand.com/itsm/Page/RepairBillQueryMobile.html?listType=3'>" + "你已有待处理报修单，提单号：" + RepairApplyBill.BillNo + "</a>", dict["EMail"], dict["Dealer"]);
            }
            catch (Exception ex)
            {

            }
            return CreatedAtRoute("DefaultApi", new { id = RepairApplyBill.Id }, RepairApplyBill);
        }

        [ResponseType(typeof(RepairApplyBill))]
        public async Task<IHttpActionResult> DeleteRepairApplyBill(int id)
        {
            RepairApplyBill RepairApplyBill = await db.RepairApplyBills.FindAsync(id);
            if (RepairApplyBill == null)
            {
                return NotFound();
            }
            db.RepairApplyBills.Remove(RepairApplyBill);
            await db.SaveChangesAsync();
            return Ok(RepairApplyBill);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RepairApplyBillExists(int id)
        {
            return db.RepairApplyBills.Count(e => e.Id == id) > 0;
        }


    }
}


//using System; 
//using System.Collections.Generic; 
//using System.Linq; 
//using System.Text; 
//using System.IO; 
//namespace 文件操作 
//{ 
//class Program 
//{ 
//static void Main(string[] args) 
//{ 
////创建一个文本文件,最好先判断一下 
//StreamWriter sw; 
//if (!File.Exists("templog.txt")) 
//{ 
////不存在就新建一个文本文件,并写入一些内容 
//sw = File.CreateText("templog.txt"); 
//sw.Write("第一个字"); 
//sw.WriteLine(" 跟随老大的."); 
//sw.WriteLine("当前日期是:"); 
//sw.WriteLine(DateTime.Now); 
//} 
//else 
//{ 
////如果存在就添加一些文本内容 
//sw = File.AppendText("templog.txt"); 
//for (int i = 0; i < 10; i++) 
//{ 
//sw.WriteLine("可以像平时输出到屏幕一样输出{0}", i); 
//} 
//} 
//sw.Close(); 
////创建一个读取器 
//StreamReader sr = new StreamReader("templog.txt"); 
////一次性读取完 
//Console.WriteLine(sr.ReadToEnd()); 
//Console.ReadLine(); 
//} 
//} 
//} 

