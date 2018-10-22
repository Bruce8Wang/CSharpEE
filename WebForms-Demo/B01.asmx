<%@ WebService Language="C#" Class="B01" %>
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService]
public class B01 : WebService
{
    public MySoapHeader mySoapHeader { get; set; }

    [WebMethod]
    [SoapHeader("mySoapHeader")]
    public List<User> GetUser(string name)
    {
        if (mySoapHeader == null)
        {
            return new List<User> { new User { Id = 1, Name = name }, new User { Id = 2, Name = "Anna" } };
        }
        else
        {
            return null;
        }
    }
}