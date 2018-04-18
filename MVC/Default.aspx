<%@ Page Language="C#" Inherits="demo.Controller.Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Home Page</title>
</head>
<body>
    <form id="frmMain" runat="server" method="post" action="./Default.aspx" enctype="application/x-www-form-urlencoded">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" /><br />
        <asp:Label ID="lblShow" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
