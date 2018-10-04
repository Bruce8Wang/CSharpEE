<%@ Page Language="C#" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>
<html>
<head>
    <title>Project Detail</title>
</head>
<body>
    <form id="frmMain" runat="server">
        <div style="height: 100%; width: 100%">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="myReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" Height="100%">
                <LocalReport ReportPath="Report/ProjectDetail.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="objDS" Name="myDS" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="objDS" runat="server" SelectMethod="GetData" TypeName="PWADSTableAdapters.ProjectDetail_RDTableAdapter"></asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
