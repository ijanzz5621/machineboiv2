<%@ Page Title="Machine Inspection Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" 
    CodeBehind="MachineInspection.aspx.vb" Inherits="MDS.App.MachineInspection" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Machine Inspection Report </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <div class="row">

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            BOI No# <br />
            <asp:DropDownList ID="ddlBOINumber" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <%--<div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Goods Type <br />
            <asp:DropDownList ID="ddlGoodsType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>--%>

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            XML Type <br />
            <asp:DropDownList ID="ddlXmlType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Equipment Type <br />
            <asp:DropDownList ID="ddlEquipmentType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Inspection <br />
            <asp:DropDownList ID="ddlInspection" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Address <br />
            <asp:DropDownList ID="ddlAddress" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
                <asp:ListItem Text="MTHAI" Value="MTHAI"></asp:ListItem>
                <asp:ListItem Text="MMT" Value="MMT"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            CER No# <br />
            <asp:DropDownList ID="ddlCERNo" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

    </div>

    <div class="row">

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Import Date From <br />
            <asp:TextBox ID="txtImportDateFrom" runat="server" CssClass="form-control datepicker" ReadOnly="false" MaxLength="50" data-date-format="yyyy-mm-dd"></asp:TextBox>
        </div> 
        
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Import Date To <br />
            <asp:TextBox ID="txtImportDateTo" runat="server" CssClass="form-control datepicker" ReadOnly="false" MaxLength="50" data-date-format="yyyy-mm-dd"></asp:TextBox>
        </div> 

        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Machine Age From <br />
            <asp:TextBox ID="txtMachineAgeFrom" runat="server" CssClass="form-control" ReadOnly="false" TextMode="Number"></asp:TextBox>
        </div> 
        
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            Machine Age To <br />
            <asp:TextBox ID="txtMachineAgeTo" runat="server" CssClass="form-control" ReadOnly="false" TextMode="Number"></asp:TextBox>
        </div> 

    </div>

    <div class="row">

        

    </div>

    <div class="row">

        <div class="col-md-6 col-sm-12" style="margin-top:10px;">

            Car Number, PO, Description, Asset Tag, Serial No, Equipment ID, Country Origin Invoice No, Equipment Brand, Vendor <br />
            <asp:TextBox ID="txtMultiFilter" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="100"></asp:TextBox>

        </div>

    </div>

    <div class="row">

        <div class="col-md-6" style="margin-top:10px;">   
            
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="form-control btn btn-primary" Width="100px" />
            &nbsp;
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="form-control btn btn-info" Width="80px" Visible="false" />
                    
        </div>
        <div class="col-md-1 col-sm-2" style="margin-top:10px;">            
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
        </div>

    </div>

    <div class="row" style="padding:10px;">

        <div style="width:100%; ">

            <asp:GridView ID="gvListing" runat="server" CssClass="table table-responsive table-bordered" 
                BackColor="White" BorderColor="#999999" 
                BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Font-Size="10px"  
                >
                <AlternatingRowStyle BackColor="#f0f0f0" />
                <Columns>
                    <%-- %><asp:BoundField DataField="BOI_NUMBER2" HeaderText="BOI No#" />--%>
                    <asp:BoundField DataField="BOI_NUMBER" HeaderText="BOI No#" />
                    <asp:BoundField DataField="INVOICE_ITEM" HeaderText="Invoice Item" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="EQUIPMENT_BRAND" HeaderText="Equip Brand" />
                    <asp:BoundField DataField="EQUIPMENT_MODEL" HeaderText="Equip Model" />
                    <asp:BoundField DataField="asset_tag" HeaderText="Asset Tag" />
                    <asp:BoundField DataField="SERIAL_NO" HeaderText="Serial No#" />
                    <asp:BoundField DataField="EQUIPMENT_ID" HeaderText="Equipment ID" />
                    <asp:BoundField DataField="QUANTITY" HeaderText="Qty" />
                    <asp:BoundField DataField="UNIT_CODE" HeaderText="Unit Code" />
                    <asp:BoundField DataField="GOOD_TYPE_DESC" HeaderText="Goods Type" />
                    <asp:BoundField DataField="MANUFACTURER_YEAR" HeaderText="Manufacturer Year" />
                    <asp:BoundField DataField="ORIGIN_CONTRY" HeaderText="Origin Country" />
                    <asp:BoundField DataField="IMPORT_DATE" HeaderText="Import Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No#" />
                    <asp:BoundField DataField="INSPECTION_DESC" HeaderText="Inspection Desc" />
                    <asp:BoundField DataField="CER_NUMBER" HeaderText="CER No#" />
                    <asp:BoundField DataField="XMLTYPE" HeaderText="XML Type" />
                    <asp:BoundField DataField="LINE" HeaderText="Line" />
                    <asp:BoundField DataField="VENDOR_NUMBER" HeaderText="Vendor" />
                    <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                    <asp:BoundField DataField="EQUIPMENT_TYPE" HeaderText="Equip Type" />  
                    <asp:BoundField DataField="PO_NUMBER" HeaderText="PO No#" />
                    <asp:BoundField DataField="CAR_NUMBER" HeaderText="Car No#" />
                    <asp:BoundField DataField="MACHINE_AGE" HeaderText="Machine Age (Year)" />
                    <asp:TemplateField HeaderText="Invoice File">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidRawFileInvoice" Value='<%# Eval("download_inv").ToString() %>' runat="server" />
                            <%--<asp:ImageButton ID="imgViewDetails1" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Files/Entry/{0}", HttpUtility.UrlEncode(Eval("download_inv").ToString()))%>' />--%>
                            <asp:HyperLink ID="hlViewRawInvoice" runat="server" Target="_blank">
                                <asp:Image ID="imgDownload1" runat="server" ImageUrl="~/images/view.png" Width="35" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Import Entry File">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidRawFileImportEntry" Value='<%# Eval("download_import").ToString() %>' runat="server" />
                            <%--<asp:ImageButton ID="imgViewDetails2" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Files/Entry/{0}", HttpUtility.UrlEncode(Eval("download_import").ToString()))%>' />--%>
                            <asp:HyperLink ID="hlViewRawImportEntry" runat="server" Target="_blank">
                                <asp:Image ID="imgDownload2" runat="server" ImageUrl="~/images/view.png" Width="35" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="#ff0000" Font-Bold="True" ForeColor="White" />
                <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                <PagerStyle BackColor="#e0e0e0" ForeColor="Black" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>

        </div>             

    </div>

    <script src="/Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.datepicker').datepicker();
        });        

    </script>


</asp:Content>
