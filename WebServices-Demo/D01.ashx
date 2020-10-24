<%@ WebHandler Language="C#" Class="D01" %>
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

public class D01 : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        IList users = new List<User> { new User { Id = 1, Name = context.Request.QueryString["name"] }, new User { Id = 2, Name = "Anna" } };
        string result = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Serialize(users);

        context.Response.ContentType = "application/json";
        context.Response.Write(result);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
            
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
