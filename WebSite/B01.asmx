<%@ WebService Language="C#" Class="B01" %>

using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class B01 : WebService
{
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
}