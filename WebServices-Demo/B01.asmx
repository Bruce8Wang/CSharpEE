<%@ WebService Language="C#" Class="B01" %>

using System.Collections.Generic;
using System.Web.Services;

[WebService]
public class B01 : WebService
{
    [WebMethod]
    public List<User> GetUser(string name)
    {
        return new List<User> { new User { Id = 1, Name = name }, new User { Id = 2, Name = "Anna" } };
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
