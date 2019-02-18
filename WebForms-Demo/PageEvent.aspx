<%@ Page Language="C#" %>
<script runat="server">

    protected override void OnPreInit(EventArgs e)
    {		
        Response.Write("1. PreInit</br>");
    }

    protected override void OnInit(EventArgs e)
    {
        Response.Write("2. Init</br>");
    }

    protected override void OnInitComplete(EventArgs e)
    {
        Response.Write("3. InitComplete</br>");
    }

    protected override void OnPreLoad(EventArgs e)
    {
        Response.Write("4. PreLoad</br>");
    }

    protected override void OnLoad(EventArgs e)
    {
        Response.Write("5. Load</br>");
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        Response.Write("6. LoadComplete</br>");
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.Write("7. PreRender</br>");
    }

    protected override void OnPreRenderComplete(EventArgs e)
    {
        Response.Write("8. PreRenderComplete</br>");
    }

    protected override void OnSaveStateComplete(EventArgs e)
    {
        Response.Write("9. SaveStateComplete</br></br>");
    }

</script>