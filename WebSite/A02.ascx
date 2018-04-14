<%@ Control Language="C#" %>

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
    }
</script>