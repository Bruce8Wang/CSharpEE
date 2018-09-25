<%@ Page Language="C#" %>
<%@ Import Namespace="CrystalDecisions.CrystalReports.Engine" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<script runat="server">    
    protected override void OnInit(EventArgs e)
    {
		//获取数据源:建议使用LinQ获取List<T>
		SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
		if (cn.State == ConnectionState.Closed) cn.Open();
		string desc = Request.Form["desc"] != null ? Request.Form["desc"].ToString() : "地球";//可传入参数
		SqlCommand cmd = new SqlCommand("select CO, Item, Description, Unit, Quantity, Price from dbo.CustomerOrders where [Description]=@desc", cn);
		cmd.Parameters.Add(new SqlParameter("@desc", desc));
		IDataReader dr = cmd.ExecuteReader();

		//设置报表文档
		ReportDocument rptDoc = new ReportDocument();
		rptDoc.Load(Server.MapPath("../Report/CustomerOrders.rpt"));
		rptDoc.SetDataSource(dr);

		//绑定报表
		crViewer.ReportSource = rptDoc;
		
		base.OnInit(e);
    }
</script>

<!DOCTYPE html>
<html>
<head>
    <title>Crystal Report Demo</title>
</head>
<body>
    <form id="frmMain" runat="server">
        <div>
            <CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="true" />
        </div>
    </form>
</body>
</html>
