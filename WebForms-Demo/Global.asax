<%@ Language="C#" %>
<script RunAt="server">	
    public override void Init()
    {		
        PostAuthenticateRequest += (sender, e) =>
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            Console.WriteLine("哇塞，过滤器！");
        };
    }
</script>
