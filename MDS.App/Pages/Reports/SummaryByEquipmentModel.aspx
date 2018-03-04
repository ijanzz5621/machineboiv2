<%@ Page Title="Summary of Equipment Model" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentModel.aspx.vb" Inherits="MDS.App.SummaryByEquipmentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            Equipment Model <br />
            <asp:DropDownList ID="ddlEquipmentModel" runat="server" CssClass="form-control" AppendDataBoundItems="true">
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

        loadFilterEquipmentType();
        loadFilterStatusCode();

        $(document).ready(function () {

            $('#<%=btnSearch.ClientID%>').on('click', function (e) {

                e.preventDefault();

                loadReport();

            });

        });

        function loadReport() {

            //alert('Loading report....');
            $.ajax({
                url: "SummaryByEquipmentModel.aspx/GetListing",
                data: "{ 'boiNumber': '" + $("#<%=txtBOINumber.ClientID%>").val() + "', 'equipmentModel': '" + $("#<%=ddlEquipmentModel.ClientID%>").val() + "', 'statusCode': '" + $("#<%=ddlStatusCode.ClientID%>").val() + "'}",
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
                            if (key === "EQUIPMENT_MODEL" || key === "AREA")
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        for (var key in tempData[0]) {
                            //console.log(key);
                            if (key !== "EQUIPMENT_MODEL" && key !== "AREA") {
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
                                if (_ === "EQUIPMENT_MODEL")
                                    keyName = text;
                                if (_ === "EQUIPMENT_MODEL" || _ === "AREA") {
                                    row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                                }
                            });

                            $.each(val, function (_, text) {
                                if (_ !== "EQUIPMENT_MODEL" && _ !== "AREA") {
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

        function loadDetails(_equipmentModel, _boiNo) {
            //alert("Equipment Type: " + _equipmentType + ", BOI No#: " + _boiNo);
            window.open('/Pages/Reports/SummaryByEquipmentModelDetails.aspx?EquipmentModel=' + _equipmentModel + '&BoiNo=' + _boiNo.replace('_', ''), '_blank');
        }

        function loadFilterEquipmentType() {

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
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));
                }
            });
        }

    </script>

</asp:Content>
