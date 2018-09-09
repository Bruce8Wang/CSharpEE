<%@ Page Language="C#" %>

<script runat="server">    
    protected override void OnInit(EventArgs e)
    {
        Load += (sender, e1) =>
        {
            if (IsPostBack)
            {
                Response.Write("我爱你，中国！");
            }
        };
        btnSubmit.Click += (sender, e2) =>
        {
            lblShow.Text = User.Identity.Name.Trim();
        };
    }
</script>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Home Page</title>
</head>
<body>
    <form id="frmMain" runat="server">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" /><br />
        <asp:Label ID="lblShow" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
