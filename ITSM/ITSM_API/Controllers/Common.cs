using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.HtmlControls;
using ITSM.Models;
using org.in2bits.MyXls;

namespace ITSM.Controllers
{
	public class Common
    {
        /////
        /////域名
        /////
        //private static string DomainName = "sztechand.com";
        /////
        ///// LDAP 地址
        /////
        //private static string LDAPDomain = "DC=sztechand.com,DC=local";

        ///
        /// LDAP绑定路径
        ///
        private static string ADPath = "LDAP://sztechand.com";
        ///
        ///登录帐号
        ///
        private static string adminUser = "Administrator";
        ///
        ///登录密码
        ///
        private static string adminPwd = "password";


        public static XlsDocument DataTableExportToExcel(DataTable dataTable, string excelName, string sheetName)
        {

            XlsDocument xls = new XlsDocument();    //新建一个xls文档
            xls.FileName = excelName + ".xls";   //设定Excel文件名

            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);//填加名为"第一个Sheet Demo"的sheet页
            Cells cells = sheet.Cells;//Cells实例是sheet页中单元格(cell)集合

            cells.Add(1, 1, "报修时间");
            cells.Add(1, 2, "报修人");
            cells.Add(1, 3, "报修中心");
            cells.Add(1, 4, "故障类型");
            cells.Add(1, 5, "资产编号");
            cells.Add(1, 6, "报修内容");
            cells.Add(1, 7, "处理方法");
            cells.Add(1, 8, "处理人");
            cells.Add(1, 9, "处理时间");
            cells.Add(1, 10, "满意度");
            cells.Add(1, 11, "处理状态");
            //cells.Add(1, 10, "访问终端");            

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int column = 0; column < dataTable.Columns.Count; column++)
                {
                    object val = dataTable.Rows[row][column];

                    ////对于数字，需要将其转化为数字类型。否则将默认以字符串类型输出到excel中
                    //double doubleVal;
                    //if (double.TryParse(val.ToString(), out doubleVal))
                    //{
                    //    cells.Add(row + 2, column + 1, doubleVal);
                    //}
                    //else
                    //{
                    //    cells.Add(row + 2, column + 1, val.ToString());
                    //}
                    cells.Add(row + 2, column + 1, val.ToString());
                }
            }

            //xls.Save("E:\\2015年流程与信息中心\\0-0-2016-1-1\\ESB - 备份\\Export\\", true);
            xls.Save("E:\\Code\\自主研发\\ESB\\Export\\", true);
            //xls.Save("D:\\App_Site\\ITSM\\Export\\", true);
            //xls.Send();
            return xls;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="toaddress"></param>
        /// <param name="todisplayname"></param>
        public static void sendMail(string subject, string body, string toaddress, string todisplayname, string filename = "")
        {//简单邮件传输协议类
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //client.Host = "smtp.163.com";//邮件服务器
            client.Host = "mail.sztechand.com";
            client.Port = 25;//smtp主机上的端口号,默认是25.
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;//邮件发送方式:通过网络发送到SMTP服务器
            //client.Credentials = new System.Net.NetworkCredential("yongjiey@163.com", "Kingdee2012");//凭证,发件人登录邮箱的用户名和密码
            client.Credentials = new System.Net.NetworkCredential("ITService@sztechand.com", "sztech@nd");//凭证,发件人登录邮箱的用户名和密码

            //电子邮件信息类
            //System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("yongjiey@163.com", "杨勇杰");//发件人Email,在邮箱是这样显示的,[发件人:小明<panthervic@163.com>;]
            System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("ITService@sztechand.com", "流程与信息中心");//发件人Email,在邮箱是这样显示的,[发件人:小明<panthervic@163.com>;]
            System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress(toaddress, todisplayname);//收件人Email,在邮箱是这样显示的, [收件人:小红<43327681@163.com>;]
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);//创建一个电子邮件类

            if (filename.Length > 0)
            {
                System.Net.Mail.Attachment attch = new System.Net.Mail.Attachment(filename);
                mailMessage.Attachments.Add(attch);
            }

            mailMessage.Subject = subject;

            //string filePath = Server.MapPath("/index.html");//邮件的内容可以是一个html文本.
            //System.IO.StreamReader read = new System.IO.StreamReader(filePath, System.Text.Encoding.GetEncoding("GB2312"));
            //string mailBody = read.ReadToEnd();
            //read.Close();
            mailMessage.Body = body;//可为html格式文本
            //mailMessage.Body = "邮件的内容";//可为html格式文本
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;//邮件主题编码
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");//邮件内容编码
            mailMessage.IsBodyHtml = true;//邮件内容是否为html格式
            mailMessage.Priority = System.Net.Mail.MailPriority.High;//邮件的优先级,有三个值:高(在邮件主题前有一个红色感叹号,表示紧急),低(在邮件主题前有一个蓝色向下箭头,表示缓慢),正常(无显示).
            try
            {
                client.Send(mailMessage);//发送邮件
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        public static void sendMessage(string mobile, string msg)
        {
            string SMS = ConfigurationManager.AppSettings["SMS"].ToString();
            if (SMS == "0") return;
            string SMAccount = ConfigurationManager.AppSettings["SMAccount"].ToString();
            string SMPassword = ConfigurationManager.AppSettings["SMPassword"].ToString();

            msg = HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("GB2312"));

            string url = string.Format("http://sms.mobset.com/SDK/Sms_Send.asp?CorpID=120079&LoginName={2}&passwd={3}&send_no={0}&Timer=60&msg={1}", mobile, msg, SMAccount, SMPassword);
            WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            myWebRequest.GetResponse();
        }


        public static Dictionary<string, string> getUserInfo(string userName)
        {
            Dictionary<string, string> dct = new Dictionary<string, string>();

            DirectoryEntry de = new DirectoryEntry("LDAP://sztechand.com");//域的根路径  
            de.UsePropertyCache = true;
            DirectorySearcher searcher = new DirectorySearcher();
            searcher.SearchRoot = de;
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = string.Format("(&(objectClass=user)(displayname={0}))", userName);
            SearchResult result = searcher.FindOne();

            ResultPropertyCollection myResultPropColl;
            myResultPropColl = result.Properties;

            string mail = "", mobile = "";

            if (myResultPropColl.Contains("mail"))
            {
                mail = myResultPropColl["mail"][0].ToString();
            }

            if (myResultPropColl.Contains("mobile"))
            {
                mobile = myResultPropColl["mobile"][0].ToString();
            }

            string distinguishedname = myResultPropColl["distinguishedname"][0].ToString();
            string[] array = distinguishedname.Split(',');
            string deptName = array[array.Length - 4].ToString().Replace("OU=", "");

            dct.Add("EMail", mail);
            dct.Add("Mobile", mobile);
            dct.Add("DeptName", deptName);

            return dct;

        }


        public static Dictionary<string, string> getHelpDesk()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ITSMModel"].ToString();

            using (var context = new ITSMModel())
            {
                var info = context.Database.SqlQuery<string>("select top 1 Dealer from FlowConfigs order by Level").ToList();

                if (info.Count > 0)
                {
                    string userName = info[0].ToString();
                    Dictionary<string, string> dict1 = new Dictionary<string, string>();
                    dict1 = getUserInfo(userName);

                    dict.Add("Dealer", userName);
                    dict.Add("EMail", dict1["EMail"]);
                    dict.Add("Mobile", dict1["Mobile"]);
                    dict.Add("DeptName", dict1["DeptName"]);
                }

            }
            return dict;
        }

        public static void updateNextDealer(OnwayFlow onwayFlow)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ITSMModel"].ToString();
            long billId = onwayFlow.RepairAppyBillId;
            string NextDealer = onwayFlow.NextDealer;

            string sql = string.Format("update RepairApplyBills  set NextEmployee='{0}' where Id={1} ", NextDealer, billId);

            using (var context = new ITSMModel())
            {
                context.Database.ExecuteSqlCommand(sql);
            }
            //SqlHelper.ExecuteNonQuery(conStr, System.Data.CommandType.Text, sql);
        }

        public static void uploadFile(HtmlInputFile file, string uploadDir)
        {
            string fileName = getUniquelyString();
            //string fileOrginName = file.PostedFile.FileName.Substring(file.PostedFile.FileName.LastIndexOf("\\") + 1);
            if (file.PostedFile.ContentLength <= 0)
            {
            }
            string filePath = uploadDir;
            string path = filePath + "\\";
            int pos = file.PostedFile.FileName.LastIndexOf("\\") + 1;
            string postFileName = file.PostedFile.FileName.Substring(file.PostedFile.FileName.Length - pos);
            file.PostedFile.SaveAs(path + fileName + "." + postFileName);
        }

        private static string getUniquelyString()
        {
            string strTemp, strYear, strMonth, strDay, strHour, strMinute, strSecond, strMillSecond;
            DateTime dt = DateTime.Now;
            strYear = dt.Year.ToString();
            strMonth = (dt.Month > 9) ? dt.Month.ToString() : "0" + dt.Month.ToString();
            strDay = (dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString();
            strHour = (dt.Hour > 9) ? dt.Hour.ToString() : "0" + dt.Hour.ToString();
            strMinute = (dt.Minute > 9) ? dt.Minute.ToString() : "0" + dt.Minute.ToString();
            strSecond = (dt.Second > 9) ? dt.Second.ToString() : "0" + dt.Second.ToString();
            strMillSecond = dt.Millisecond.ToString();
            strTemp = strYear + strMonth + strDay + "_" + strHour + strMinute + strSecond + "_" + strMillSecond;
            return strTemp;
        }



        #region 操作AD账号
        ///
        ///获得DirectoryEntry对象实例,以管理员登陆AD
        ///
        ///
        private static DirectoryEntry GetDirectoryObject()
        {
            //DirectoryEntry entry = new DirectoryEntry(ADPath, ADUser, ADPassword, AuthenticationTypes.Secure);

            DirectoryEntry entry = new DirectoryEntry(ADPath);//域的根路径  

            return entry;
        }

        ///根据用户帐号称取得用户的 对象
        ///用户帐号名 
        ///如果找到该用户，则返回用户的 对象；否则返回 null
        public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        ///根据用户帐号和密码取得用户的 对象
        ///用户帐号
        ///用户密码 
        ///如果找到该用户，则返回用户的 对象；否则返回 null
        public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName, string password)
        {
            DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);
            if (de != null)
            {
                string commonName = de.Properties["cn"][0].ToString();

                if (GetDirectoryEntry(sAMAccountName, password) != null)
                    return GetDirectoryEntry(commonName, password);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        ///
        ///根据指定用户名和密码获得相应DirectoryEntry实体
        private static DirectoryEntry GetDirectoryObject(string userName, string password)
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath, userName, password, AuthenticationTypes.None);
            return entry;
        }


        ///设置指定 的属性值
        ///属性名称 
        ///属性值 
        public static void SetProperty(DirectoryEntry de, string propertyName, string propertyValue)
        {
            if (propertyValue != string.Empty || propertyValue != "" || propertyValue != null)
            {
                if (de.Properties.Contains(propertyName))
                {
                    de.Properties[propertyName][0] = propertyValue;
                }
                else
                {
                    de.Properties[propertyName].Add(propertyValue);
                }
            }
        }


        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ude">用户</param>
        /// <param name="password">旧密码</param>
        /// <param name="password">新密码</param>
        public static bool ChangePassword(string username, string oldpwd, string newpwd)
        {
            DirectoryEntry entry = FindObject("user", username);
            try
            {

                if (entry != null)
                {
                    // to-do: 需要解决密码策略问题
                    entry.Invoke("ChangePassword", new object[] { oldpwd, newpwd });
                    entry.CommitChanges();

                    DomainUser._success = "密码修改成功！";
                    return true;
                }
                else
                {
                    throw new FileNotFoundException("没找到用户：" + username);

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("指定的网络密码不正确"))
                    return true;
                else if (ex.InnerException.Message.Contains("密码不满足密码策略的要求。检查最小密码长度、密码复杂性和密码历史的要求"))
                    throw new Exception("密码修改失败!密码长度必须大于6位，且不能与前两次密码相同！");
                else
                    throw new Exception("密码修改失败!" + ex.InnerException.Message);
            }
            finally
            {
                entry.CommitChanges();
                entry.Close();
            }
        }
        #endregion

        #region 获取域路径
        /// <summary>
        /// 获取域路径
        /// </summary>
        /// <returns>路径</returns>
        public static string GetDomainPath()
        {
            using (DirectoryEntry root = new DirectoryEntry(ADPath, adminUser, adminPwd))
            {
                return root.Path;
            }
        }
        #endregion

        #region  依据类别用户名 查找目录项
        /// <summary>
        /// 查找目录项
        /// </summary>
        /// <param name="category">分类 users</param>
        /// <param name="name">用户名</param>
        /// <returns>目录项实体</returns>
        public static DirectoryEntry FindObject(string category, string name)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;
            DirectoryEntry userEntry = null;
            try
            {
                //de = new DirectoryEntry(ADPath, adminUser, adminPwd, AuthenticationTypes.Secure);
                de = new DirectoryEntry(ADPath);

                ds = new DirectorySearcher(de);
                string queryFilter = string.Format("(&(objectCategory=" + category + ")(sAMAccountName={0}))", name);
                ds.Filter = queryFilter;
                ds.Sort.PropertyName = "cn";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    userEntry = sr.GetDirectoryEntry();
                }
                return userEntry;
            }
            catch (Exception ex)
            {
                DomainUser._failed = ex.Message.ToString();
                return new DirectoryEntry();
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (de != null)
                {
                    de.Dispose();
                }
            }
        }
        #endregion

        public static bool CheckPwd(string username, string password)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;
            bool flag = false;
            try
            {
                de = new DirectoryEntry(ADPath, username, password, AuthenticationTypes.Secure);
                ds = new DirectorySearcher(de);
                SearchResult result = ds.FindOne();
                if (result != null) flag = true;
                else flag = false;
            }
            catch (Exception ex)
            {
                flag = false;

            }

            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (de != null)
                {
                    de.Dispose();
                }
            }

            return flag;
        }

        ///根据用户账号取得用户的 对象
        ///用户账号 
        ///如果找到该用户，则返回用户的 对象；否则返回 null
        public static DirectoryEntry GetDirectoryEntry(string sAMAccountName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + sAMAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        ///
        ///根据用户账号和密码取得用户的 对象。
        ///
        ///用户公共名称 
        ///用户密码 
        ///如果找到该用户，则返回用户的 对象；否则返回 null
        public static DirectoryEntry GetDirectoryEntry(string sAMAccountName, string password)
        {
            DirectoryEntry de = GetDirectoryObject(sAMAccountName, password);
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + sAMAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }

    public class DomainUser
    {
        public static string _failed;
        public static string _success;

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string UserPwd { get; set; }
        public string Department { get; set; }
        public string PhysicalDeliveryOfficeName { get; set; }
        public string sAMAccountName { get; set; }
        public string Mobile { get; set; }

    }
}