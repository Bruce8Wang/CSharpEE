<<<<<<< HEAD:WebForms-Demo/C01.ascx
﻿<%@ Control Language="C#" Inherits="System.Web.UI.UserControl" %>

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
=======
﻿<%@ Control Language="C#" Inherits="System.Web.UI.UserControl" %>

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
>>>>>>> ab488c5599154966aed731b26c973d62f1f1bc0d:WebForms-Demo/A03.ascx
