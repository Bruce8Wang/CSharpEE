using System.Web.Services.Protocols;

public class MySoapHeader : SoapHeader
{
    public string UserName { get; set; }
    public string Ip { get; set; }
    public string Token { get; set; }
}