﻿<%@ Page Title="Summary By Equipment Model Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentModelDetails.aspx.vb" Inherits="MDS.App.SummaryByEquipmentModelDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>

        .paging{
            list-style: none;
            margin: 0;
            padding: 0;
            margin-bottom: 15px;
        }

        .paging li {
            display: inline-block;
            margin-right: 15px;
            cursor: pointer;
        }

        .selected {
            font-weight: bold;
            
        }

        .selected a {
            color:#000000 !important;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Details of total equipment by equipment model </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <input id="btnExport" type="button" value="Export" class="btn btn-primary" />

    <div class="row">

        <div id="divTablePaging" class="col-md-12">
            <ul id="ulTablePaging" class="paging"></ul>
        </div>

        <div class="col-md-12">

            <table id="tblListing" class="table table-bordered table-responsive">
                <thead>
                    <tr style="background-color:#444; color:#fff;">
                    </tr>
                </thead>
                <tbody></tbody>
            </table>

        </div>

        <div id="divExport" class="col-md-12" style="display:none;">

            <table id="tblExport" class="table table-bordered table-responsive">
                <thead>
                    <tr style="background-color:#444; color:#fff;">
                    </tr>
                </thead>
                <tbody></tbody>
            </table>

        </div>

    </div>

    <script src="/Scripts/table2excel.js"></script>
    <script>

        var gData = [];
        var gTotalRecords = 0;
        var gCurrentPage = 1;
        var gItemPerPage = 20;
        var gNext = 0;
        var gStartPaging = 1;
        var gEndPaging = 1;
        var gTotalPaging = 0;
        var gTotalCheck = 0;

        var gEquipModel = '<%= Request.QueryString("EquipmentModel") %>';
        var gBoiNo = '<%= Request.QueryString("BoiNo") %>';
        var gStatus = '<%= Request.QueryString("status") %>';

        $(document).ready(function () {

            loadReportDetails();

            $('#btnExport').on('click', function (e) {
                e.preventDefault();
                exportData();
            });

        });

        function exportData() {

            if (gData.length > 0) {

                for (var key in gData[0]) {
                    $('#tblExport thead tr').append("<td>" + key + "</td>");
                }

                $('#tblExport tbody').html("");

                var row = "";
                var rowCount = 0;
                $.each(gData, function (key, val) {

                    var keyName = "";
                    var rowColor = "transparent";
                    if (rowCount % 2 === 0)
                        rowColor = "#fff";

                    row = row + "<tr style='cursor:pointer; background-color:" + rowColor + "'>";
                    $.each(val, function (_, text) {
                        row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                    });
                    row = row + "</tr>";

                });

                $('#tblExport tbody').append(row);

                $("#tblExport").table2excel({
                    filename: "SummaryByEquipmentModelDetails.xls"
                });

            } else {
                alert("No data to export!");
            }
        }

        function changePage(_page) {
            gCurrentPage = _page;
            displayReport(gData);
        } 

        function displayPaging() {

            $('#ulTablePaging').empty();

            gStartPaging = 1 + gNext;
            gCurrentPage = gStartPaging;

            //alert(gTotalPaging);

            if (gStartPaging + 10 < gTotalPaging)
                gEndPaging = gStartPaging + (10 - 1);
            else
                gEndPaging = gTotalPaging;

            if (gStartPaging > 1)
                $('#ulTablePaging').append("<li id='" + i + "' class='" + ((gCurrentPage === i) ? "selected" : "") + "'><a onclick='prevPaging(); return false;'>Previous<a></li>")

            for (var i = gStartPaging; i <= gEndPaging; i++) {
                $('#ulTablePaging').append("<li id='" + i + "' class='" + ((gCurrentPage === i) ? "selected" : "") + "'><a onclick='changePage(\"" + i + "\"); return false;'>" + i + "<a></li>")
            }

            if (gEndPaging < gTotalPaging)
                $('#ulTablePaging').append("<li id='" + i + "' class='" + ((gCurrentPage === i) ? "selected" : "") + "'><a onclick='nextPaging(); return false;'>Next<a></li>")
        }

        function nextPaging() {
            gNext = gNext + 10;
            displayPaging();
            changePage(gCurrentPage);
        }

        function prevPaging() {
            gNext = gNext - 10;
            displayPaging();
            changePage(gCurrentPage);
        }

        function loadReportDetails() {

            $.ajax({
                url: "SummaryByEquipmentModelDetails.aspx/GetListing",
                data: "{ 'equipmentModel': '" + gEquipModel + "', 'boiNumber': '" + gBoiNo + "', 'status':'" + gStatus + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    gNext = 0;
                    gCurrentPage = 1;
                    gStartPaging = 1;
                    var tempData = JSON.parse(data.d);
                    gData = tempData;

                    // clear the table
                    $('#tblListing thead tr').html("");
                    $('#tblListing tbody').html("");

                    // clear the paging
                    $('#ulTablePaging').empty();

                    if (tempData.length > 0) {

                        // ********************** PAGING ***********************
                        gTotalRecords = tempData.length;
                        gTotalPaging = parseInt((gTotalRecords % gItemPerPage) > 0 ? ((gTotalRecords / gItemPerPage) + 1) : (gTotalRecords / gItemPerPage));
                        displayPaging();

                        for (var key in tempData[0]) {
                            $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        displayReport(gData);

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

        function displayReport(tempData) {

            $('#ulTablePaging li').removeClass("selected");
            $('#ulTablePaging li[id="' + gCurrentPage + '"]').addClass("selected");

            $('#tblListing tbody').html("");

            // *********************** DATA ROW ***********************
            var firstPage = ((gCurrentPage - 1) * gItemPerPage) + 1;
            var lastPage = firstPage + (gItemPerPage - 1);
            // alert("First page: " + firstPage + ", Last Page: " + lastPage);

            var row = "";
            var rowCount = 0;
            $.each(tempData, function (key, val) {

                if (rowCount + 1 >= firstPage && rowCount + 1 <= lastPage) {

                    var keyName = "";
                    var rowColor = "transparent";
                    if (rowCount % 2 === 0)
                        rowColor = "#fff";

                    row = row + "<tr style='cursor:pointer; background-color:" + rowColor + "'>";
                    $.each(val, function (_, text) {
                        row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                    });
                    row = row + "</tr>";
                }

                rowCount++;
            });
            $('#tblListing tbody').append(row);
        }

    </script>

</asp:Content>
