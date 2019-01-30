<%@ Language="VBScript" %>
<%
	for each x in Request.ServerVariables
		response.write(x & " : " & Request.ServerVariables(x) &"<br>")
	next
%>