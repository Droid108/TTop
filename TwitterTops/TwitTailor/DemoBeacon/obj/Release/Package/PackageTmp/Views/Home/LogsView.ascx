<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
    if (Model != null)
    {

        foreach (DemoBeacon.Models.tblDemoBeacon obj in Model)
        {
            string typeName = obj.Type == 1 ? "IN TIME" : "OUT TIME";
            string Dates = obj.LogTime.AddHours(12).AddMinutes(30).ToString();
%>
<tr>
    <td><%= typeName %></td>
    <td><%= obj.DeviceName %></td>
    <td><%= obj.ZoneName %></td>
    <td><%= Dates %></td>
</tr>
<%
        }
    }
    else
    { %>
<tr>
    No data found . please Hit Refresh.
</tr>
<%} %>
