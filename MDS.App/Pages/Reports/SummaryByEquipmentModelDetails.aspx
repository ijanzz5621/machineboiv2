<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentModelDetails.aspx.vb" Inherits="MDS.App.SummaryByEquipmentModelDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Details of total equipment by equipment model </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <div class="row">

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

        var gEquipModel = '<%= Request.QueryString("EquipmentModel") %>';
        var gBoiNo = '<%= Request.QueryString("BoiNo") %>';

        $(document).ready(function () {

            loadReportDetails();

        });

        function loadReportDetails() {

            $.ajax({
                url: "SummaryByEquipmentModelDetails.aspx/GetListing",
                data: "{ 'equipmentModel': '" + gEquipModel + "', 'boiNumber': '" + gBoiNo + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //alert(data.d);
                    var tempData = JSON.parse(data.d);

                    // clear the table
                    $('#tblListing thead tr').html("");
                    $('#tblListing tbody').html("");

                    if (tempData.length > 0) {
                        for (var key in tempData[0]) {
                            $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        var row = "";
                        var rowCount = 0;
                        $.each(tempData, function (key, val) {

                            var keyName = "";
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
