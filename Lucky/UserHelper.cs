using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Lucky
{
    internal static class UserHelper
    {
        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        
        public static string setwin = "";
        public static IList<string[]> sname;

        //奖项与数量对应到Dictionary
        public static readonly Dictionary<string, int> DictAwardNum = new Dictionary<string, int>();
        //奖项与中奖名单对应到Dictionary
        public static readonly Dictionary<string, IList<string[]>> DictAwardWinner = new Dictionary<string, IList<string[]>>();
        public static string awardName;

        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        public static void sendMessage(string mobile, string msg)
        {
            msg = UrlEncode(msg);
            var url =
                $"http://sms.mobset.com/SDK/Sms_Send.asp?CorpID=120079&LoginName=thekp&passwd=766772&send_no={mobile}&Timer=60&msg={msg}";
            WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            if (IniReadValue("Award", "EnableSM") != "1") return;
            try
            {
                myWebRequest.GetResponse();
            }
            catch (Exception)
            {

            }
        }

        private static string UrlEncode(string str)
        {
            var sb = new StringBuilder();
            var byStr = Encoding.Default.GetBytes(str); //默认System.Text.Encoding.Default.GetBytes(str)
            foreach (var t in byStr)
            {
                sb.Append(@"%" + Convert.ToString(t, 16));
            }
            return (sb.ToString());
        }
        #endregion

        #region 读写ini配置文件
        //读取INI文件  
        private static string IniReadValue(string Section, string Key)
        {
            var buffer = new byte[32768];
            var strDir = Environment.CurrentDirectory + @"\config.ini";
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(Section, Key, "", temp, 255, strDir);
            return temp.ToString();
        }

        //写入INI文件
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            var strDir = Environment.CurrentDirectory + @"\config.ini";
            WritePrivateProfileString(Section, Key, Value, strDir);
        }
        #endregion

        #region 操作Excel
        /// <summary>
        /// 从Excel中读取抽奖人员名单 
        /// </summary>
        public static void ReadEmployeeList()
        {
            var strDir = Environment.CurrentDirectory + @"\抽奖设置.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);

            try
            {
                sname = new List<string[]>();
                conn.Open();
                var select = "SELECT * FROM [名单$] WHERE [是否已抽出] is null and [姓名] is not null";
                var readCommand = new OleDbDataAdapter(select, conn);

                var readData = new DataSet("Data");
                readCommand.Fill(readData);

                foreach (DataTable dt in readData.Tables)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var s = new string[3];
                        s[0] = dt.Rows[i][1].ToString().Trim();
                        s[1] = dt.Rows[i][2].ToString().Trim();
                        s[2] = dt.Rows[i][3].ToString().Trim();
                        sname.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }

            finally
            {
                conn.Close();
                Console.ReadLine();
            }

        }

        /// <summary>
        /// 读出已抽出人员名单
        /// </summary>
        /// <param name="level"></param>
        public static void ReadEmployeeListWinner(string awardName)
        {
            var strDir = Environment.CurrentDirectory + @"\中奖名单.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);
            try
            {
                var select = $"SELECT * FROM [中奖名单$] where [奖项] = '{awardName}'";
                conn.Open();
                var readCommand = new OleDbDataAdapter(select, conn);
                var readData = new DataSet("Data");
                readCommand.Fill(readData);
                DictAwardWinner[awardName].Clear();

                foreach (DataTable dt in readData.Tables)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] s = new string[3];
                        s[0] = dt.Rows[i][0].ToString().Trim();
                        s[1] = dt.Rows[i][1].ToString().Trim();
                        s[2] = dt.Rows[i][2].ToString().Trim();
                        DictAwardWinner[awardName].Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }
            finally
            {
                conn.Close();
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 保存抽奖结果到Excel中
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="rowIndex"></param>
        /// <param name="sheet"></param>
        /// <param name="singleQty"></param>
        public static void SaveToExcel(IList<string[]> winner, int rowIndex, string awardName, int singleQty)
        {
            var sql = string.Empty;
            var sb = new StringBuilder();
            var strDir = Environment.CurrentDirectory + @"\中奖名单.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);
            try
            {
                conn.Open();

                for (var i = 0; i < singleQty; i++)
                {
                    //sb.AppendLine(string.Format("insert into [中奖名单$] values('{0}','{1}','{2}','{3}')", winner[rowIndex + i][0], winner[rowIndex + i][1], winner[rowIndex + i][2], awardName));
                    sql =
                        $"insert into [中奖名单$] values('{winner[rowIndex + i][0]}','{winner[rowIndex + i][1]}','{winner[rowIndex + i][2]}','{awardName}')";
                    var myCommand = new OleDbCommand(sql, conn);
                    myCommand.ExecuteNonQuery();
                }
                //OleDbCommand myCommand = new OleDbCommand(sb.ToString(), conn);
                //myCommand.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 从抽奖名单中移除已中奖人员
        /// </summary>   
        /// <param name="empNo">工号</param>
        public static void RemoveWinner(IList<string[]> empNo)
        {
            var sql = string.Empty;
            var strDir = Environment.CurrentDirectory + @"\抽奖设置.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);
            try
            {
                conn.Open();
                foreach (var t in empNo)
                {
                    sql = $"update [名单$] set [是否已抽出]='是' where [工号]='{t[2]}' ";
                    var myCommand = new OleDbCommand(sql, conn);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 设置奖项
        public static void SaveAward(string name, int num)
        {
            var sql = string.Empty;
            var strDir = Environment.CurrentDirectory + @"\抽奖设置.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);

            try
            {
                conn.Open();
                sql = $"insert into [奖项$] values('{name}','{num}')";
                var myCommand = new OleDbCommand(sql, conn);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region 读取奖项
        public static DataTable ReadAward()
        {
            var strDir = Environment.CurrentDirectory + @"\抽奖设置.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);
            var dt = new DataTable();
            try
            {
                conn.Open();
                var select = "SELECT * FROM [奖项$]";
                var readCommand = new OleDbDataAdapter(select, conn);
                var readData = new DataSet("Data");
                readCommand.Fill(readData);
                dt = readData.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了");
            }

            finally
            {
                conn.Close();
                Console.ReadLine();
            }
            return dt;

        }
        #endregion
    }
}
