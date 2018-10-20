<%@ WebService Language="C#" Class="B01" %>
using System;
using System.Data;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class B01 : WebService
{
    [WebMethod]
    public DataTable GetUser(string name)
    {
        DataTable dt = new DataTable("dbo");
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Name", typeof(String));
        DataRow dr = dt.NewRow();
        dr["Id"] = 1;
        dr["Name"] = name;
        dt.Rows.Add(dr);
        return dt;
    }
}