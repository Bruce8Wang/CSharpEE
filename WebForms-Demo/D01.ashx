<%@ WebHandler Language="C#" Class="D01" %>
using System.Web;
public class D01 : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}