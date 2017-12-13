﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentType.aspx.vb" Inherits="MDS.App.SummaryByEquipmentType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Summary of total equipment by equipment type </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <div class="row">

        <div class="col-md-4">
            BOI Number (Seperate by comma (,))<br />
            <asp:TextBox ID="txtBOINumber" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            Equipment Type <br />
            <asp:DropDownList ID="ddlEquipmentType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2">
            Status Code <br />
            <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2">   
            &nbsp; <br />
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="form-control btn btn-primary" Width="80px" Height="33px" />         
        </div>

    </div>

    <div class="row" style="margin-top:15px;">

        <div class="col-md-12">

            <table id="tblListing" class="table table-bordered table-responsive">
                <thead>
                    <tr style="background-color:#444; color:#fff;">
                    </tr>
                </thead>
                <tbody></tbody>
            </table>

        </div>

    </div>


    <script>

        $(document).ready(function () {

            $('#<%=btnSearch.ClientID%>').on('click', function (e) {

                e.preventDefault();

                loadReport();

            });

        });

        function loadReport() {

            //alert('Loading report....');
            $.ajax({
                url: "SummaryByEquipmentType.aspx/GetListing",
                data: "{ 'boiNumber': '', 'equipmentType': '', 'statusCode': ''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    alert(data.d);

                    // clear the table
                    $('#tblListing thead tr').html("");
                    $('#tblListing tbody').html("");

                    if (JSON.parse(data.d).length > 0) {

                        for (var key in JSON.parse(data.d)[0]) {
                            $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }
                        var row = "";
                        var rowCount = 0;
                        $.each(JSON.parse(data.d), function (key, val) {

                            var rowColor = "transparent";
                            if (rowCount % 2 === 0)
                                rowColor = "#fff";

                            row = row + "<tr style='cursor:pointer; background-color:" + rowColor + "'>";
                            $.each(val, function (_, text) {
                                row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                            });
                            row = row + "</tr>";

                            rowCount++;
                        });
                        $('#tblListing tbody').append(row);
                    } else {
                        //showPopupMessage("No data found!");
                    }

                },
                beforeSend: function (request) {
                    HoldOn.open({ theme: "sk-rect" });
                }
                , complete: function () {
                    HoldOn.close();
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });

        }



    </script>

</asp:Content>