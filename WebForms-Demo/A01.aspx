<%@ Page Language="C#" %>

<script runat="server">    
    protected override void OnInit(EventArgs ea)
    {
        Load += (sender, e) =>
        {
            if (IsPostBack)
            {
                Response.Write("我爱你，中国！");
            }
        };
        btnSubmit.Click += (sender, e) =>
        {
            lblShow.Text = User.Identity.Name.Trim();
        };
        base.OnInit(ea);
    }
</script>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>A01</title>
</head>
<body>
    <form id="frmMain" runat="server">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" /><br />
        <asp:Label ID="lblShow" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
