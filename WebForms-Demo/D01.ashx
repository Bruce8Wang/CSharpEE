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
        dt.Rows.Add(2, "Anna");

        context.Response.ContentType = "application/json";
        context.Response.Write(DataTable2Json(dt));
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 将DataTable转换成JSON
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string DataTable2Json(DataTable dt)
    {
        IList list = new ArrayList();
        foreach (DataRow dataRow in dt.Rows)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
            }
            list.Add(dictionary);
        }
        return new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Serialize(list);
    }
}