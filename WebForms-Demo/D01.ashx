<%@ WebHandler Language="C#" Class="D01" %>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;

public class D01 : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // 模拟一个DataTable
        DataTable dt = new DataTable("dbo");
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Name", typeof(String));
        dt.Rows.Add(1, context.Request.QueryString["name"]);
        // 将DataTable转换成JSON
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        javaScriptSerializer.MaxJsonLength = int.MaxValue;
        IList list = new ArrayList();
        foreach (DataRow dataRow in dt.Rows)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
            }
            list.Add(dictionary);
        }
        String result = javaScriptSerializer.Serialize(list);

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