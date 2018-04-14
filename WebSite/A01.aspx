<%@ Page Language="C#" %>

<script runat="server">    
    protected override void OnInit(EventArgs f)
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
    }
</script>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Home Page</title>
</head>
<body>
    <form id="frmMain" runat="server" method="post" enctype="application/x-www-form-urlencoded">
        <asp:Button runat="server" ID="btnSubmit" Text="Submit"></asp:Button><br />
        <asp:Label runat="server" ID="lblShow"></asp:Label>
    </form>
</body>
</html>
