<%@ WebService Language="C#" Class="B01" %>
using System;
using System.Data;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class B01 : WebService
{
    public MySoapHeader mySoapHeader { get; set; }

    [WebMethod]
    [SoapHeader("mySoapHeader")]
    public DataTable GetUser(string name)
    {
        if (mySoapHeader == null)
        {
            DataTable dt = new DataTable("dbo");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(String));
            dt.Rows.Add(1, name);
            return dt;
        }
        else
        {
            return null;
        }
    }
}

public class MySoapHeader : SoapHeader
{
    public string UserName { get; set; }
    public string Ip { get; set; }
    public string Token { get; set; }
}