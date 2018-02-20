using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ITSM.Controllers
{
    public class UploadController : ApiController
    {

        //public async Task<HttpResponseMessage> PostFormData()
        public async Task<string> PostFormData()
        {
            // 检查该请求是否含有multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);
            string strFileNames = "";

            try
            {
                HttpFileCollection hfc = HttpContext.Current.Request.Files;

                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];
                    string[] arr = hpf.FileName.Split('.');
                    string fileType = arr[arr.Length - 1];
                    string strFileName = System.Guid.NewGuid().ToString() + "." + fileType;
                    //hpf.SaveAs(HttpContext.Current.Request.MapPath("/") + "App_Data\\" + strFileName + "." + fileType);
                    hpf.SaveAs(HttpContext.Current.Request.MapPath("/") + "Images\\" + strFileName);
                    strFileNames = strFileNames + "|" + strFileName;
                    //strFilePaths = strFilePaths + "|" + HttpContext.Current.Request.MapPath("/") + "App_Data\\" + strFileName + "." + fileType;
                }
                
                return strFileNames;
            }
            catch (System.Exception e)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return "Error:" + HttpStatusCode.InternalServerError;
            }
        }
    }
}
