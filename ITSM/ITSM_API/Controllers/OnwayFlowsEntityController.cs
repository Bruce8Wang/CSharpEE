using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ITSM.Models;

namespace ITSM.Controllers
{
	public class OnwayFlowsEntityController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        // POST: ../api/OnwayFlowsEntity
        [ResponseType(typeof(OnwayFlowEntity))]
        //public async Task<IHttpActionResult> PostOnwayFlow(OnwayFlow onwayFlow)
        public async Task<IHttpActionResult> PostOnwayFlow(OnwayFlowEntity onwayFlowEntity)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
 
            

                //更新RepairApplyBill字段
                db.Entry(onwayFlowEntity.repairApplyBill).State = EntityState.Modified;

                #region 向OnwayFlow中增加记录
                string message;

                //报修单号
                string billNo = onwayFlowEntity.repairApplyBill.BillNo;
                //报修人手机号码
                string mobile = onwayFlowEntity.repairApplyBill.Phone;
                //报修人
                string bxEmployee = onwayFlowEntity.repairApplyBill.BXEmployee;
                //报修中心
                string bxDept = onwayFlowEntity.repairApplyBill.BXDept;
                //报修主题
                string bxTitle = onwayFlowEntity.repairApplyBill.Title;
                //邮箱
                string email = onwayFlowEntity.repairApplyBill.EMail;
                long repairApplyBillId = onwayFlowEntity.repairApplyBill.Id;
                string ITMaintenanceMobile = ConfigurationManager.AppSettings["ITMaintenanceMobile"].ToString();
                db.OnwayFlows.Add(onwayFlowEntity.onwayFlow);
                #endregion

                await db.SaveChangesAsync();
                Common.updateNextDealer(onwayFlowEntity.onwayFlow);
                switch (onwayFlowEntity.onwayFlow.DealMethodId)
                {
                    case 2:
                        //处理
                        //发送短信给报修人
                        message = string.Format("<a href='http://itsm.sztechand.com/Page/RepairBill.html?billType=4&id={1}'>您的报修单[提单号：{0}]已处理完毕，请填写满意度。</a><br><br>感谢您对流程与信息中心的支持！<br>欢迎使用<a href='http://itsm.sztechand.com'>IT服务平台</a>", billNo, repairApplyBillId);
                        //message = string.Format("您的报修单[提单号：{0}]已处理完毕，请填写满意度。", billNo);
                        //Common.sendMessage(mobile.ToString(), message);
                        Common.sendMail(string.Format("您的报修单[提单号：{0}]已处理完毕，请填写满意度", billNo), message, email, "流程与信息中心-应用运维部");
                        break;

                    case 3:
                        //转发
                        //1.发送短信给报修人
                        message = string.Format("您的报修单[提单号：{0}]已由IT工程师[{1}]授理。", billNo, onwayFlowEntity.onwayFlow.NextDealer);
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
                    case 4:
                        //接单
                        //1.发送短信给报修人
                        message = string.Format("您的报修单[提单号：{0}]已由IT工程师[{1}]授理。", billNo, onwayFlowEntity.onwayFlow.CurrentDealer);
                        Common.sendMessage(mobile.ToString(), message);
                        break;
                }

            }
            catch (System.Exception ex)
            { }
            return CreatedAtRoute("DefaultApi", new { id = onwayFlowEntity.onwayFlow.Id }, onwayFlowEntity.onwayFlow);
        }
    }
}