using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using org.in2bits.MyXls;

namespace ITSM.Controllers
{
	public class ExportPrinterController : ApiController
    {
        [ResponseType(typeof(XlsDocument))]
        public async Task<XlsDocument> Export()
        {

            string excelName = "打印机代码导出" + DateTime.Now.ToString("yyyyMMdd");

            string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["ITSMModel"].ToString();
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();              //创建SqlCommand对象
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Id, No, Name, Center, case Color when 1 then '有' else '无' end as Color, "
                            + "case FiveFloor when 1 then '有' else '无' end as FiveFloor, "
                            + "case SixFloor when 1 then '有' else '无' end as SixFloor, "
                            + "case SevenFloor when 1 then '有' else '无' end as SevenFloor, "
                            + "case EighthFloor when 1 then '有' else '无' end as EighthFloor, "
                            + "Note from PrinterPermissions";   //sql语句

            SqlDataAdapter myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = cmd;
            myDataAdapter.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("无可导出数据！");
            }
            XlsDocument xls = DataTableExportToExcel(ds.Tables[0], excelName, "导出结果");
            conn.Close();
            return xls;

        }

        private XlsDocument DataTableExportToExcel(DataTable dataTable, string excelName, string sheetName)
        {

            XlsDocument xls = new XlsDocument();    //新建一个xls文档
            xls.FileName = excelName + ".xls";   //设定Excel文件名

            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);//填加名为"第一个Sheet Demo"的sheet页
            Cells cells = sheet.Cells;//Cells实例是sheet页中单元格(cell)集合

            cells.Add(1, 1, "代码");
            cells.Add(1, 2, "名称");
            cells.Add(1, 3, "中心");
            cells.Add(1, 4, "彩色");
            cells.Add(1, 5, "五楼权限");
            cells.Add(1, 6, "六楼权限");
            cells.Add(1, 7, "七楼权限");
            cells.Add(1, 8, "八楼权限");
            cells.Add(1, 9, "备注");           

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int column = 0; column < dataTable.Columns.Count; column++)
                {
                    object val = dataTable.Rows[row][column];
                    cells.Add(row + 2, column + 1, val.ToString());
                }
            }

            //xls.Save("E:\\Code\\ESB\\自主研发\\ESB\\Export\\", true);
            xls.Save("D:\\App_Site\\ITSM\\Export\\", true);
            //xls.Send();
            return xls;
        }
    }
}
