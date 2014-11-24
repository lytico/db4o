<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Db4oDoc.WebApp.Models.Pilot>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                Points
            </th>
        </tr>

    <!-- #example: In the view use the ids to identify the objects# -->
    <% foreach (var pilot in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=pilot.ID }) %> |
                <%= Html.ActionLink("Delete", "Delete", new { id = pilot.ID })%>
            </td>
            <td>
                <%= Html.Encode(pilot.Name) %>
            </td>
            <td>
                <%= Html.Encode(pilot.Points) %>
            </td>
        </tr>
    
    <% } %>
    <!-- #end example -->

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

