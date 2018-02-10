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
        //������дINI�ļ���API����
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        
        public static string setwin = "";
        public static IList<string[]> sname;

        //������������Ӧ��Dictionary
        public static readonly Dictionary<string, int> DictAwardNum = new Dictionary<string, int>();
        //�������н�������Ӧ��Dictionary
        public static readonly Dictionary<string, IList<string[]>> DictAwardWinner = new Dictionary<string, IList<string[]>>();
        public static string awardName;

        #region ���Ͷ���
        /// <summary>
        /// ���Ͷ���
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
            var byStr = Encoding.Default.GetBytes(str); //Ĭ��System.Text.Encoding.Default.GetBytes(str)
            foreach (var t in byStr)
            {
                sb.Append(@"%" + Convert.ToString(t, 16));
            }
            return (sb.ToString());
        }
        #endregion

        #region ��дini�����ļ�
        //��ȡINI�ļ�  
        private static string IniReadValue(string Section, string Key)
        {
            var buffer = new byte[32768];
            var strDir = Environment.CurrentDirectory + @"\config.ini";
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(Section, Key, "", temp, 255, strDir);
            return temp.ToString();
        }

        //д��INI�ļ�
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            var strDir = Environment.CurrentDirectory + @"\config.ini";
            WritePrivateProfileString(Section, Key, Value, strDir);
        }
        #endregion

        #region ����Excel
        /// <summary>
        /// ��Excel�ж�ȡ�齱��Ա���� 
        /// </summary>
        public static void ReadEmployeeList()
        {
            var strDir = Environment.CurrentDirectory + @"\�齱����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);

            try
            {
                sname = new List<string[]>();
                conn.Open();
                var select = "SELECT * FROM [����$] WHERE [�Ƿ��ѳ��] is null and [����] is not null";
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
                MessageBox.Show(ex.Message, "������");
            }

            finally
            {
                conn.Close();
                Console.ReadLine();
            }

        }

        /// <summary>
        /// �����ѳ����Ա����
        /// </summary>
        /// <param name="level"></param>
        public static void ReadEmployeeListWinner(string awardName)
        {
            var strDir = Environment.CurrentDirectory + @"\�н�����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);
            try
            {
                var select = $"SELECT * FROM [�н�����$] where [����] = '{awardName}'";
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
                MessageBox.Show(ex.Message, "������");
            }
            finally
            {
                conn.Close();
                Console.ReadLine();
            }
        }

        /// <summary>
        /// ����齱�����Excel��
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="rowIndex"></param>
        /// <param name="sheet"></param>
        /// <param name="singleQty"></param>
        public static void SaveToExcel(IList<string[]> winner, int rowIndex, string awardName, int singleQty)
        {
            var sql = string.Empty;
            var sb = new StringBuilder();
            var strDir = Environment.CurrentDirectory + @"\�н�����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);
            try
            {
                conn.Open();

                for (var i = 0; i < singleQty; i++)
                {
                    //sb.AppendLine(string.Format("insert into [�н�����$] values('{0}','{1}','{2}','{3}')", winner[rowIndex + i][0], winner[rowIndex + i][1], winner[rowIndex + i][2], awardName));
                    sql =
                        $"insert into [�н�����$] values('{winner[rowIndex + i][0]}','{winner[rowIndex + i][1]}','{winner[rowIndex + i][2]}','{awardName}')";
                    var myCommand = new OleDbCommand(sql, conn);
                    myCommand.ExecuteNonQuery();
                }
                //OleDbCommand myCommand = new OleDbCommand(sb.ToString(), conn);
                //myCommand.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������");
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// �ӳ齱�������Ƴ����н���Ա
        /// </summary>   
        /// <param name="empNo">����</param>
        public static void RemoveWinner(IList<string[]> empNo)
        {
            var sql = string.Empty;
            var strDir = Environment.CurrentDirectory + @"\�齱����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);
            try
            {
                conn.Open();
                foreach (var t in empNo)
                {
                    sql = $"update [����$] set [�Ƿ��ѳ��]='��' where [����]='{t[2]}' ";
                    var myCommand = new OleDbCommand(sql, conn);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region ���ý���
        public static void SaveAward(string name, int num)
        {
            var sql = string.Empty;
            var strDir = Environment.CurrentDirectory + @"\�齱����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=0'";
            var conn = new OleDbConnection(source);

            try
            {
                conn.Open();
                sql = $"insert into [����$] values('{name}','{num}')";
                var myCommand = new OleDbCommand(sql, conn);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������");
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region ��ȡ����
        public static DataTable ReadAward()
        {
            var strDir = Environment.CurrentDirectory + @"\�齱����.xls";
            var source = "Provider=Microsoft.Jet.OleDb.4.0;Data Source='" + strDir + "';Extended Properties='Excel 8.0;HDR=yes;IMEX=1'";
            var conn = new OleDbConnection(source);
            var dt = new DataTable();
            try
            {
                conn.Open();
                var select = "SELECT * FROM [����$]";
                var readCommand = new OleDbDataAdapter(select, conn);
                var readData = new DataSet("Data");
                readCommand.Fill(readData);
                dt = readData.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������");
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
