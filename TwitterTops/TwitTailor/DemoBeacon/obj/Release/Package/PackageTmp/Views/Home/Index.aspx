<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">

    Home Page - IBeacon - Demo
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
        <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <section class="featured">
        <div class="content-wrapper">
            <a class="badge alert-danger" href="#" onclick="loadData(); return false;">Reload Data</a>
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <table data-toggle="table" class="table table-striped">
            <thead>
                <tr>
                    <th data-field="id">Type</th>
                    <th data-field="name">Device Name</th>
                    <th data-field="price">Zone Name</th>
                    <th data-field="price">Log Time</th>
                </tr>
            </thead>
            <tbody id="divResult">
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadData();
        });

        function loadData() {
            var url = '/Home/GetLog';
            $('#divResult').html("loading data .. please wait...");
            $.get(url, function (data, status) {
                $('#divResult').html(data);
            });
        }
    </script>
</asp:Content>
