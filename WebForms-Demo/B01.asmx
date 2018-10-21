<%@ WebService Language="C#" Class="B01" %>
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class B01 : WebService
{
    public MySoapHeader mySoapHeader { get; set; }

    [WebMethod]
    [SoapHeader("mySoapHeader")]
    public IList<User> GetUser(string name)
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