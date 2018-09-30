<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BOITransfer.aspx.vb" Inherits="MDS.App.BOITransfer" MasterPageFile="~/Site.Master" %>
<%@ Register Src="~/Modals/_BOITransfer.ascx" TagPrefix="uc1" TagName="_BOITransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> BOI Transfer </h3>

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

        .popup {
            z-index:900;
        }

        .ui-datepicker-div {
            z-index: 999;
        }

        .datepicker {
            z-index:1151 !important;
            cursor: pointer;
        }

    </style>

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
            &nbsp; <br />
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="form-control btn btn-primary" Width="80px" Height="33px" />         
        </div>

    </div>

    <div id="divTransfer" style="display:none;">
        &nbsp; <br />
        <input id="btnShowTransfer" type="button" class="btn btn-success" value="Transfer" />
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

    <uc1:_BOITransfer runat="server" ID="_BOITransfer" />

    <script src="/Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">

        var gData = [];
        var gTotalRecords = 0;
        var gCurrentPage = 1;
        var gItemPerPage = 20;
        var gNext = 0;
        var gStartPaging = 1;
        var gEndPaging = 1;
        var gTotalPaging = 0;
        var gTotalCheck = 0;
        var gSelectedIDs = [];
        

        $(document).ready(function () {

            loadFilterEquipmentType();

            $('#<%=btnSearch.ClientID%>').on('click', function (e) {
                e.preventDefault();
                loadListing();
            });

            $('#btnShowTransfer').on('click', function (e) {

                populatePopupList();

                $('#modalBOITransfer').modal();
            });

            $('#modalBtnTransfer').on('click', function (e) {

                if ($('#modalTxtBOIDate').val().trim() === "") {
                    alert("Please select BOI Date!");
                    return;
                };

                $.confirm({
                    title: 'BOI Transfer',
                    content: 'Are you sure to transfer?',
                    buttons: {
                        confirm: {
                            text: 'PROCEED',
                            btnClass: 'btn-blue',
                            keys: ['enter', 'shift'],
                            action: function () {
                                transferBOI(function () {

                                    $('#modalBOITransfer').modal('hide');
                                    loadListing();
                                });
                            }
                        },
                        cancel: function () {
                        }
                    }
                });

            });

            $('.datepicker').datepicker();

        }); // end of document ready

        $(document).on('change', '#tblListing tbody tr td input[type="checkbox"]', function() {

            var totalCheck = 0;

            gSelectedIDs = [];
            $('#tblListing tbody tr').each(function (key, val) {
                if ($(this).find('td input[type="checkbox"]').is(':checked')) {

                    // save the id in the array
                    gSelectedIDs.push($(this).find('td input[type="checkbox"]').attr('id'));

                    totalCheck++;
                }
            });

            //alert("Total Selected ID are: " + gSelectedIDs);

            if (totalCheck > 0) {
                $('#divTransfer').show('slow');
            } else {
                $('#divTransfer').hide();
            }

        });

        function transferBOI(cb) {
            //alert("BOI Transfered!!");

            $.ajax({
                url: "BOITransfer.aspx/TransferBOI",
                data: "{ 'dataList': '" + gSelectedIDs + "', 'boiDate':'" + $('#modalTxtBOIDate').val() + "', 'boiNumber':'" + $('#modalTxtBOINumber').val() + "'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {


                    return cb();
                },
                error: function (a, b, c) {
                    console.log('error: ' + JSON.stringify(a));

                    return cb();
                }
            });
        }

        function populatePopupList() {

            $('#modalList tbody').empty();

            $.each(gSelectedIDs, function (key, val) {
                $('#modalList tbody').append("<tr><td>" + val.split("_")[0] + "</td><td>" + val.split("_")[1] + "</td><td>" + val.split("_")[2] + "</td><td>" + val.split("_")[3] + "</td></tr>");
            });
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

        function loadListing() {

            //alert('Loading report....');
            $.ajax({
                url: "BOITransfer.aspx/GetListing",
                data: "{ 'boiNumber': '" + $("#<%=txtBOINumber.ClientID%>").val() + "', 'equipmentType': '" + $("#<%=ddlEquipmentType.ClientID%>").val() + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //alert(JSON.stringify(data));

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
                        gTotalPaging = parseInt((gTotalRecords % gItemPerPage) > 0 ? (gTotalRecords / gItemPerPage) + 1 : gTotalRecords / gItemPerPage);
                        displayPaging();

                        // add checkbox as first column
                        $('#tblListing thead tr').append("<td>&nbsp;</td>");

                        // ********************** HEADER ***********************
                        for (var key in tempData[0]) {
                            ////console.log(key);
                            //if (key === "EQUIPMENT_TYPE" || key === "EQUIPMENT_MODEL" || key === "AREA")
                                $('#tblListing thead tr').append("<td>" + key + "</td>");
                        }

                        //for (var key2 in tempData[0]) {
                        //    //console.log(key);
                        //    if (key2 !== "EQUIPMENT_TYPE" && key2 !== "EQUIPMENT_MODEL" && key2 !== "AREA") {
                        //        $('#tblListing thead tr').append("<td>" + key2 + "</td>");
                        //    }
                                
                        //}

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

                    var keyName = "";
                    var rowColor = "transparent";
                    if (rowCount % 2 === 0)
                        rowColor = "#fff";

                    row = row + "<tr style='cursor:pointer; background-color:" + rowColor + "'>";

                    // add checkbox
                    row = row + "<td><input type='checkbox' class='chkItem' id='" + val.EQUIPMENT_ID + "_" + val.BOI_NUMBER + "_" + val.INVOICE_NUMBER + "_" + val.INVOICE_ITEM + "' /></td>";

                    $.each(val, function (_, text) {
                    //    if (_ === "EQUIPMENT_TYPE") {
                    //        keyName = text;
                            row = row + "<td>" + ((text === null) ? "" : text) + "</td>";
                    //    }
                    });

                    //$.each(val, function (_, text) {
                    //    if (_ !== "EQUIPMENT_TYPE") {
                    //        row = row + "<td" + ((text === null || text === 0) ? "" : " style='background-color:#ACECFF; text-align:center;font-weight:bold;'") + ">" + ((text === null || text === 0) ? "" : "<a href='#' onclick='loadDetails(\"" + keyName + "\", \"" + _ + "\");'>" + text + "</a>") + "</td>";
                    //    }
                    //});

                    row = row + "</tr>";
                }

                rowCount++;
            });
            $('#tblListing tbody').append(row);
        }

        function loadFilterEquipmentType() {

            $("#<%=ddlEquipmentType.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "BOITransfer.aspx/GetFilterEquipmentType",
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

    </script>


</asp:Content>
