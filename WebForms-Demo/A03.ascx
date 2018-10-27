<%@ Control Language="C#" Inherits="System.Web.UI.UserControl" %>

<script runat="server">
    public string Text { get; set; }

    protected override void OnInit(EventArgs ea)
    {
        Load += (sender, e) =>
        {
            if (IsPostBack)
            {
                Response.Write("我爱你，中国！");
            }
        };
        base.OnInit(ea);
    }
</script>