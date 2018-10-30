<%@ Control Language="C#" Inherits="System.Web.UI.UserControl" %>

<script runat="server">
    public string Text { get; set; }

    protected override void OnInit(EventArgs e)
    {
        Load += (sender, e1) =>
        {
            if (IsPostBack)
            {
                Response.Write("我爱你，中国！");
            }
        };
        base.OnInit(e);
    }
</script>