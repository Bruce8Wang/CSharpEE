using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using ITSM.Models;

namespace ITSM.Controllers
{
    public class LoginController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        // GET: ../api/Login
        public LoginUserInfo GetLoginUser()
        {
            //Common.sendMail("测试附件", "测试附件", "yangyongjie@sztechand.com.cn", "杨勇杰", "E:\\Code\\自主研发\\ESB\\Export\\报修结果导出20150421.xls");
 
            //test();
            string userAccount = this.User.Identity.Name.Trim();
            //return getLoginUser(userAccount);
            int index = userAccount.IndexOf("\\");

            userAccount = userAccount.Substring(index + 1, userAccount.Length - index - 1);

            DirectoryEntry de = new DirectoryEntry("LDAP://sztechand.com");//域的根路径  
            de.UsePropertyCache = true;
            DirectorySearcher searcher = new DirectorySearcher();
            searcher.SearchRoot = de;
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = string.Format("(&(objectClass=user)(samAccountName={0}))", userAccount);
            SearchResult result = searcher.FindOne();
            ResultPropertyCollection myResultPropColl;
            myResultPropColl = result.Properties;
            string userName = myResultPropColl["displayname"][0].ToString();
            string mail = myResultPropColl.Contains("mail") == true ? myResultPropColl["mail"][0].ToString() : "";
            string mobile = myResultPropColl.Contains("mobile") == true ? myResultPropColl["mobile"][0].ToString() : "";
            string distinguishedname = myResultPropColl.Contains("distinguishedname") == true ? myResultPropColl["distinguishedname"][0].ToString() : "";

            string[] array = distinguishedname.Split(',');
            string deptName = array[array.Length - 4].ToString().Replace("OU=", "");

            int userType = (from b in db.SuperUsers where b.UserName == userName select b).Count() == 0 ? 0 : 1;

            var loginUserInfo = new LoginUserInfo()
            {

                UserType = userType,
                UserName = userName,
                DeptName = deptName,
                EMail = mail,
                Mobile = mobile

            };

            return loginUserInfo;
        }


        ///// <summary>
        ///// 写文件
        ///// </summary>
        ///// <param name="_filePath">文件路径</param>
        ///// <param name="TOEXCELLR">要写入的内容</param>
        //private void WriteFile(string _filePath, string TOEXCELLR)
        //{
        //    //检查是否创建文档成功
        //    if (CreateXmlFile(_filePath))
        //    {  //写文本，
        //        using (StreamWriter fs = new StreamWriter(_filePath, true, System.Text.Encoding.UTF8))
        //        {
        //            fs.Write(TOEXCELLR);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 创建文件 的方法
        ///// </summary>
        ///// <param name="filepath">路径</param>
        ///// <returns>文件存在返True否在为False</returns>
        //private static Boolean CreateXmlFile(string filepath)
        //{
        //    try
        //    {
        //        //记录成功时的记录
        //        if (!File.Exists(filepath))
        //        {
        //            using (StreamWriter xmlfs = new StreamWriter(filepath, true, System.Text.Encoding.UTF8))
        //            {
        //                //xmlfs.Write("");
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        public static void sendMessage(string mobile, string msg)
        {
            msg = HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("GB2312"));
            string url = string.Format("http://sms.mobset.com/SDK/Sms_Send.asp?CorpID=120079&LoginName=thekp&passwd=766772&send_no={0}&Timer=60&msg={1}", mobile, msg);
            WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            myWebRequest.GetResponse();
        }
    }
}
