using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using ITSM.Models;

namespace ITSM.Controllers
{
	public class Trans2OAController : ApiController
    {
        public async Task<IHttpActionResult> Post(RepairApplyBillDTO obj)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string userAccount = this.User.Identity.Name.Trim();
                //return getLoginUser(userAccount);
                int index = userAccount.IndexOf("\\");
                userAccount = userAccount.Substring(index + 1, userAccount.Length - index - 1);

                //获取OA表单字段
                string itsmTemplateId = ConfigurationManager.AppSettings["itsmTemplateId"].ToString();
                //string itsmBillId = ConfigurationManager.AppSettings["itsmBillId"].ToString();
                string itsmBillNo = ConfigurationManager.AppSettings["itsmBillNo"].ToString();
                string itsmApplyEmployee = ConfigurationManager.AppSettings["itsmApplyEmployee"].ToString();
                string itsmApplyDept = ConfigurationManager.AppSettings["itsmApplyDept"].ToString();
                string itsmBXEmployee = ConfigurationManager.AppSettings["itsmBXEmployee"].ToString();
                string itsmBXDept = ConfigurationManager.AppSettings["itsmBXDept"].ToString();
                string itsmPhone = ConfigurationManager.AppSettings["itsmPhone"].ToString();
                string itsmBXDate = ConfigurationManager.AppSettings["itsmBXDate"].ToString();
                string itsmAssetCode = ConfigurationManager.AppSettings["itsmAssetCode"].ToString();
                string itsmPriority = ConfigurationManager.AppSettings["itsmPriority"].ToString();
                string itsmFaultType = ConfigurationManager.AppSettings["itsmFaultType"].ToString();
                string itsmApplyEMail = ConfigurationManager.AppSettings["itsmApplyEMail"].ToString();
                string itsmPrevOperation = ConfigurationManager.AppSettings["itsmPrevOperation"].ToString();
                string itsmNote = ConfigurationManager.AppSettings["itsmNote"].ToString();
                string itsmDealer = ConfigurationManager.AppSettings["itsmDealer"].ToString();
                string itsmDealDate = ConfigurationManager.AppSettings["itsmDealDate"].ToString();
                string itsmDealProcess = ConfigurationManager.AppSettings["itsmDealProcess"].ToString();
                string itsmDealNote = ConfigurationManager.AppSettings["itsmDealNote"].ToString();
                string itsmDocCreator = userAccount + "@sztechand.com";




                kmReviewParamterForm form = new kmReviewParamterForm();
                form.fdTemplateId = itsmTemplateId;
                form.docSubject = obj.Title;
                form.docCreator = @"{'LoginName': '" + itsmDocCreator + "'}";
                form.formValues = @"{'" + itsmBillNo + "':'" + obj.BillNo
                                + "','" + itsmApplyEmployee + "':'" + obj.ApplyEmployee
                                + "','" + itsmApplyDept + "':'" + obj.ApplyDept
                                + "','" + itsmBXEmployee + "':'" + obj.BXEmployee
                                + "','" + itsmBXDept + "':'" + obj.BXDept
                                + "','" + itsmPhone + "':'" + obj.Phone
                                + "','" + itsmBXDept + "':'" + obj.BXDept
                                + "','" + itsmBXDate + "':'" + obj.BXDate
                                + "','" + itsmNote + "':'" + obj.Note
                                + "','" + itsmDealDate + "':'" + obj.BXDealTime
                                + "','" + itsmAssetCode + "':'" + obj.AssetCode
                                + "','" + itsmPriority + "':'" + obj.PriorityName
                                + "','" + itsmFaultType + "':'" + obj.FaultTypeName
                                + "','" + itsmApplyEMail + "':'" + obj.EMail
                                + "','" + itsmPrevOperation + "':'" + obj.PrevOperation
                                + "','" + itsmDealer + "':'" + obj.BXDealEmployee
                                + "','" + itsmDealNote + "':'" + obj.BXDealNote
                                + "'}";
                form.flowParam = @"{'auditNode':'请审核'}";
                KmReviewWebserviceServiceClient c = new KmReviewWebserviceServiceClient();
                c.addReview(form);



            }
            catch (System.Exception ex)
            { }
            return CreatedAtRoute("DefaultApi", new { id = obj.Id }, obj);
        }
    }
}
