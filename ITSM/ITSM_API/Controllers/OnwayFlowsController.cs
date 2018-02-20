using System;
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
    public class OnwayFlowsController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/onwayFlows
        //[EnableQuery]
        public IQueryable<OnwayFlowDTO> GetOnwayFlows()
        {
            var onwayFlows = from b in db.OnwayFlows
                             select new OnwayFlowDTO()
                             {
                                 Id = b.Id,
                                 RepairAppyBillNo = b.RepairAppyBill.BillNo,
                                 CurrentDealer = b.CurrentDealer,
                                 NextDealer = b.NextDealer,
                                 DealDate = b.DealDate,
                                 DealMethodName = b.DealMethod.Name,
                                 DealNote = b.DealMethodId == 4 ? b.DealProcess : b.DealNote

                             };
            return onwayFlows;
        }

        //Get:../api/onwayFlows/1
        [ResponseType(typeof(OnwayFlow))]
        public async Task<IHttpActionResult> GetOnwayFlow(int id)
        {
            OnwayFlow onwayFlow = await db.OnwayFlows.FindAsync(id);
            if (onwayFlow == null)
            {
                return NotFound();
            }
            return Ok(onwayFlow);
        }

        //PUT:../api/onwayFlows/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOnwayFlow(int id, OnwayFlow onwayFlow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != onwayFlow.Id)
            {
                return BadRequest();
            }

            db.Entry(onwayFlow).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OnwayFlowExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: ../api/OnwayFlows
        [ResponseType(typeof(OnwayFlow))]
        public async Task<IHttpActionResult> PostOnwayFlow(OnwayFlow onwayFlow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string message;
                RepairApplyBill RepairApplyBill = await db.RepairApplyBills.FindAsync(onwayFlow.RepairAppyBillId);

                //报修单号
                string billNo = RepairApplyBill.BillNo;
                //报修人手机号码
                string mobile = RepairApplyBill.Phone;
                //报修人
                string bxEmployee = RepairApplyBill.BXEmployee;
                //报修中心
                string bxDept = RepairApplyBill.BXDept;
                //报修主题
                string bxTitle = RepairApplyBill.Title;

                long repairApplyBillId = RepairApplyBill.Id;

                string ITMaintenanceMobile = ConfigurationManager.AppSettings["ITMaintenanceMobile"].ToString();

                db.OnwayFlows.Add(onwayFlow);
                await db.SaveChangesAsync();
                Common.updateNextDealer(onwayFlow);

                switch (onwayFlow.DealMethodId)
                {
                    case 2:
                        //处理
                        //发送短信给报修人
                        message = string.Format("<a href='http://itsm.sztechand.com/Page/RepairBill.html?billType=4&id={1}'>您的报修单[提单号：{0}]已处理完毕，请填写满意度。</a><br><br>感谢您对流程与信息中心的支持！<br>欢迎使用<a href='http://itsm.sztechand.com'>IT服务平台</a>", billNo, repairApplyBillId);
                        //message = string.Format("您的报修单[提单号：{0}]已处理完毕，请填写满意度。", billNo);
                        //Common.sendMessage(mobile.ToString(), message);
                        Common.sendMail(string.Format("您的报修单[提单号：{0}]已处理完毕，请填写满意度", billNo), message, RepairApplyBill.EMail, "流程与信息中心-应用运维部");
                        break;

                    case 3:
                    case 4:
                        //转发
                        //1.发送短信给报修人
                        message = string.Format("您的报修单[提单号：{0}]已由IT工程师[{1}]授理。", billNo, onwayFlow.NextDealer);
                        Common.sendMessage(mobile.ToString(), message);

                        //2.发送短信给IT工程师
                        //Common.sendMessage(ITMaintenanceMobile, string.Format("报修主题：{4},报修人：{1}[{2}],中心：{3}", billNo, bxEmployee, mobile, bxDept, bxTitle));

                        //Dictionary<string, string> dict = new Dictionary<string, string>();
                        //dict = Common.getUserInfo(onwayFlow.NextDealer);

                        //if (dict["Mobile"].Length == 0)
                        //{
                        //    Common.sendMessage("17722680637", onwayFlow.NextDealer + "的手机号码未维护");
                        //}
                        //else
                        //{
                        //    Common.sendMessage(dict["Mobile"], string.Format("你有待处理报修单，提单号：{0}，报修人：{1},联系方式：{2},中心：{3},报修主题：{4}", billNo, bxEmployee, mobile, bxDept, bxTitle));
                        //}
                        break;
                }

            }
            catch (Exception ex)
            { }
            return CreatedAtRoute("DefaultApi", new { id = onwayFlow.Id }, onwayFlow);
        }


        // DELETE: ../api/OnwayFlows/5
        [ResponseType(typeof(OnwayFlow))]
        public async Task<IHttpActionResult> DeleteOnwayFlow(int id)
        {
            OnwayFlow onwayFlow = await db.OnwayFlows.FindAsync(id);
            if (onwayFlow == null)
            {
                return NotFound();
            }

            db.OnwayFlows.Remove(onwayFlow);
            await db.SaveChangesAsync();

            return Ok(onwayFlow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool OnwayFlowExists(int id)
        {
            return db.OnwayFlows.Count(e => e.Id == id) > 0;
        }
    }
}