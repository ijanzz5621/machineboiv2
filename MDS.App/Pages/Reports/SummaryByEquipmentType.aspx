<%@ Page Title="Summary of Equipment Type" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentType.aspx.vb" Inherits="MDS.App.SummaryByEquipmentType" %>
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

            <div id="divDynatable" class="dynatable">

                <table id="dynatable">
                    <thead></thead>
                    <tbody></tbody>
                </table>

            </div>

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

        loadFilterEquipmentType();
        loadFilterStatusCode();

        $(document).ready(function () {

            $('#<%=btnSearch.ClientID%>').on('click', function (e) {

                e.preventDefault();

                loadReport();

            });

        });

        function loadReport() {

            $.ajax({
                url: "SummaryByEquipmentType.aspx/GetListing",
                data: "{ 'boiNumber': '" + $("#<%=txtBOINumber.ClientID%>").val() + "', 'equipmentType': '" + $("#<%=ddlEquipmentType.ClientID%>").val() + "', 'statusCode': '" + $("#<%=ddlStatusCode.ClientID%>").val() + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var tempData = JSON.parse(data.d);

                    // clear the table
                    $('#tblListing thead tr').html("");
                    $('#tblListing tbody').html("");

                    if (tempData.length > 0) {
                        for (var key in tempData[0]) {
                            //console.log(key);
                            if (key === "EQUIPMENT_TYPE")
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        for (var key in tempData[0]) {
                            console.log(key);
                            if (key !== "EQUIPMENT_TYPE") {
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                            }
                                
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
                                if (_ === "EQUIPMENT_TYPE") {
                                    keyName = text;
                                    row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                                }
                            });

                            $.each(val, function (_, text) {
                                if (_ !== "EQUIPMENT_TYPE") {
                                    row = row + "<td" + ((text === null || text === 0) ? "" : " style='background-color:#ACECFF; text-align:center;font-weight:bold;'") + ">" + ((text === null || text === 0) ? "" : "<a href='#' onclick='loadDetails(\"" + keyName + "\", \"" + _ + "\");'>" + text + "</a>") + "</td>";
                                }
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

        function loadDetails(_equipmentType, _boiNo) {
            window.open('/Pages/Reports/SummaryByEquipmentTypeDetails.aspx?EquipmentType=' + _equipmentType + '&BoiNo=' + _boiNo.replace('_', ''), '_blank');
        }

        function loadFilterEquipmentType() {

            $("#<%=ddlEquipmentType.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "SummaryByEquipmentType.aspx/GetFilterEquipmentType",
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
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

        function loadFilterStatusCode() {

            $("#<%=ddlStatusCode.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "SummaryByEquipmentType.aspx/GetFilterStatusCode",
                //data: "{ '': ''}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (JSON.parse(data.d).length > 0) {

                        $.each(JSON.parse(data.d), function () {
                            $("#<%=ddlStatusCode.ClientID%>").append($("<option />").val(this.STATUS_CODE).text(this.STATUS_CODE));
                        });

                    }

                    $("#<%=ddlStatusCode.ClientID%>").removeAttr('disabled');
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

    </script>

</asp:Content>
