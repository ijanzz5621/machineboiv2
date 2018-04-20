<%@ Page Title="Summary of Equipment Model" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentModel.aspx.vb" Inherits="MDS.App.SummaryByEquipmentModel" %>
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

    <h3> Summary of total equipment by equipment model </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <div class="row">

        <div class="col-md-4">
            BOI Number (Seperate by comma (,))<br />
            <asp:TextBox ID="txtBOINumber" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            Equipment Type <br />
            <asp:DropDownList ID="ddlEquipmentType" runat="server" CssClass="form-control" AppendDataBoundItems="true" multiple>
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2">
            Equipment Model <br />
            <asp:DropDownList ID="ddlEquipmentModel" runat="server" CssClass="form-control" AppendDataBoundItems="true" multiple>
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2">
            Status Code <br />
            <asp:DropDownList ID="ddlStatusCode" runat="server" CssClass="form-control" AppendDataBoundItems="true" multiple>
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2">   
            &nbsp; <br />
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="form-control btn btn-primary" Width="80px" Height="33px" />         
        </div>

    </div>

    <div class="row" style="margin-top:15px;">

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

    </div>


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

        loadFilterEquipmentModel();
        loadFilterEquipmentType();
        loadFilterStatusCode();

        $(document).ready(function () {

            $('#<%=btnSearch.ClientID%>').on('click', function (e) {

                e.preventDefault();

                loadReport();

            });

        });

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

        function loadReport() {

            //alert('Loading report....');
            $.ajax({
                url: "SummaryByEquipmentModel.aspx/GetListing",
                data: "{ 'boiNumber': '" + $("#<%=txtBOINumber.ClientID%>").val() + "', 'equipmentModel': '" + $("#<%=ddlEquipmentModel.ClientID%>").val() + "', 'equipmentType': '" + $("#<%=ddlEquipmentType.ClientID%>").val() + "', 'statusCode': '" + $("#<%=ddlStatusCode.ClientID%>").val() + "'}",
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

                        // ********************** HEADER ***********************
                        for (var key in tempData[0]) {
                            //console.log(key);
                            if (key === "EQUIPMENT_TYPE" || key === "EQUIPMENT_MODEL" || key === "AREA")
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        for (var key in tempData[0]) {
                            //console.log(key);
                            if (key !== "EQUIPMENT_TYPE" && key !== "EQUIPMENT_MODEL" && key !== "AREA") {
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                            }
                                
                        }

                        displayReport(gData);
                    }
                    else {
                        alert('No record found!');
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

                    //var keyName = "";
                    var equipmentModel = "";
                    var equipmentType = "";

                    var rowColor = "transparent";
                    if (rowCount % 2 === 0)
                        rowColor = "#fff";

                    row = row + "<tr style='cursor:pointer; background-color:" + rowColor + "'>";
                    $.each(val, function (_, text) {

                        if (_ === "EQUIPMENT_TYPE")
                            equipmentType = text;
                        else if (_ === "EQUIPMENT_MODEL")
                            equipmentModel = text;
                        
                        if (_ === "EQUIPMENT_TYPE" || _ === "EQUIPMENT_MODEL" || _ === "AREA") {
                            row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                        }
                    });

                    $.each(val, function (_, text) {
                        if (_ !== "EQUIPMENT_TYPE" && _ !== "EQUIPMENT_MODEL" && _ !== "AREA") {
                            row = row + "<td" + ((text === null || text === 0) ? "" : " style='background-color:#ACECFF; text-align:center;font-weight:bold;'") + ">" + ((text === null || text === 0) ? "" : "<a href='#' onclick='loadDetails(\"" + equipmentModel + "\", \"" + equipmentType + "\", \"" + _ + "\");'>" + text + "</a>") + "</td>";
                        }
                    });

                    row = row + "</tr>";

                }

                rowCount++;
            });
            $('#tblListing tbody').append(row);
        }

        function loadDetails(_equipmentModel, _equipmentType, _boiNo) {
            //alert("Equipment Type: " + _equipmentType + ", BOI No#: " + _boiNo);
            window.open('/Pages/Reports/SummaryByEquipmentModelDetails.aspx?EquipmentModel=' + _equipmentModel + '&EquipmentType=' + _equipmentType + '&BoiNo=' + _boiNo.replace('[', '').replace(']', '') + '&status=' + $('#<%=ddlStatusCode.ClientID%>').val(), '_blank');
        }

        function loadFilterEquipmentModel() {

            $("#<%=ddlEquipmentModel.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "SummaryByEquipmentModel.aspx/GetFilterEquipmentModel",
                //data: "{ '': ''}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //console.log(JSON.parse(data.d).length);

                    if (JSON.parse(data.d).length > 0) {

                        $.each(JSON.parse(data.d), function () {
                            $("#<%=ddlEquipmentModel.ClientID%>").append($("<option />").val(this.EQUIPMENT_MODEL).text(this.EQUIPMENT_MODEL));
                        });

                    }

                    $("#<%=ddlEquipmentModel.ClientID%>").removeAttr('disabled');
                    $('#<%=ddlEquipmentModel.ClientID%>').chosen();
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

        function loadFilterEquipmentType() {

            $("#<%=ddlEquipmentType.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "SummaryByEquipmentModel.aspx/GetFilterEquipmentType",
                //data: "{ '': ''}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (JSON.parse(data.d).length > 0) {

                        $.each(JSON.parse(data.d), function () {
                            $("#<%=ddlEquipmentType.ClientID%>").append($("<option />").val(this.EQUIPMENT_TYPE).text(this.EQUIPMENT_TYPE));
                        });

                    }

                    $("#<%=ddlEquipmentType.ClientID%>").removeAttr('disabled');
                    $('#<%=ddlEquipmentType.ClientID%>').chosen();
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

        function loadFilterStatusCode() {

            $("#<%=ddlStatusCode.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "SummaryByEquipmentModel.aspx/GetFilterStatusCode",
                //data: "{ '': ''}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //console.log(JSON.parse(data.d).length);

                    if (JSON.parse(data.d).length > 0) {

                        $.each(JSON.parse(data.d), function () {
                            $("#<%=ddlStatusCode.ClientID%>").append($("<option />").val(this.STATUS_CODE).text(this.STATUS_CODE));
                        });

                    }

                    $("#<%=ddlStatusCode.ClientID%>").removeAttr('disabled');
                    $('#<%=ddlStatusCode.ClientID%>').chosen();
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

    </script>

</asp:Content>
