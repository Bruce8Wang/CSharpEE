<%@ Application Language="C#" %>

<script RunAt="server">
    public override void Init()
    {
        PostAuthenticateRequest += (sender, e) =>
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        };
        base.Init();
    }
</script>
