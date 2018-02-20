using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using org.in2bits.MyXls;

namespace ITSM.Controllers
{
	public class ExportDataController : ApiController
    {
        [ResponseType(typeof(XlsDocument))]
        public async Task<XlsDocument> Export(ITSM.Models.Filter obj)
        {

            string excelName = "报修结果导出" + DateTime.Now.ToString("yyyyMMdd");

            string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["ITSMModel"].ToString();
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connectString);
            string filter = string.Empty;
            if (obj.BeginTime.Length > 0)
            {
                filter = filter + string.Format(" and a.BXDate >='{0}'", obj.BeginTime);
            }
            if (obj.EndTime.Length > 0)
            {
                filter = filter + string.Format(" and a.BXDate <='{0}'", obj.EndTime);
            }

            
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();              //创建SqlCommand对象
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select  a.BXDate, a.BXEmployee ,a.BXDept, c.Name as BXFaultName,AssetCode, a.Note as BXNote,BXDealNote ,BXDealEmployee, BXDealTime= case a.StatusId when 1 then NULL else a.BXDealTime end, b.Name as SatisfactionName, d.Name as StatusName from RepairApplyBills a  "
                                            + "inner join FaultTypes c on c.Id = a.FaultTypeId "
                                            + "inner join SatisfactionLevels b on b.Id = a.SatisfactionLevelId  "
                                            + "inner join Status d on d.Id = a.StatusId "
                                            + "where 1=1 " + filter;   //sql语句

            SqlDataAdapter myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = cmd;
            myDataAdapter.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("无可导出数据！");
            }
            XlsDocument xls = Common.DataTableExportToExcel(ds.Tables[0], excelName, "导出结果");

            conn.Close();
            return xls;

        }


    }
}