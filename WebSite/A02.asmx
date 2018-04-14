<%@ WebService Language="C#" Class="A002" %>

using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class A002 : WebService
{
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
}