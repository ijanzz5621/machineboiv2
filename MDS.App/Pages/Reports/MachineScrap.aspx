<%@ Page Title="Machine Scrap Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" 
    CodeBehind="MachineScrap.aspx.vb" Inherits="MDS.App.MachineScrap" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Machine Scrap Report </h3>

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

    <div class="row">

        <div class="col-md-1 col-sm-2" style="margin-top:10px;">  
            BOI No#          
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtBOINo" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-md-1 col-sm-2" style="margin-top:10px;"> 
            Import Date           
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtImportDate" runat="server" CssClass="form-control datepicker" ReadOnly="false" MaxLength="50" data-date-format="yyyy-mm-dd"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-1 col-sm-2" style="margin-top:10px;">  
            Invoice No#          
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-md-1 col-sm-2" style="margin-top:10px;"> 
            Description           
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="50"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-1 col-sm-2" style="margin-top:10px;">  
            Equipment ID         
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtEquipmentId" runat="server" CssClass="form-control" ReadOnly="false" TextMode="Number"></asp:TextBox>
        </div>
        <div class="col-md-1 col-sm-2" style="margin-top:10px;"> 
            Asset Tag           
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtAssetTag" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="10"></asp:TextBox>
        </div>        

    </div>  
    
    <div class="row">

        <div class="col-md-1 col-sm-2" style="margin-top:10px;">  
            Valid Date         
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            <asp:TextBox ID="txtValidDate" runat="server" CssClass="form-control datepicker" ReadOnly="false" MaxLength="50" data-date-format="yyyy-mm-dd"></asp:TextBox>
        </div>
        <div class="col-md-1 col-sm-2" style="margin-top:10px;"> 
            &nbsp;          
        </div>
        <div class="col-md-2 col-sm-4" style="margin-top:10px;">
            &nbsp;
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
                    <%--<asp:BoundField DataField="BOI_NUMBER2" HeaderText="BOI No#" />--%>
                    <asp:BoundField DataField="BOI_NUMBER" HeaderText="BOI No#" />
                    <asp:BoundField DataField="INVOICE_ITEM" HeaderText="Invoice Item" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                    <asp:BoundField DataField="asset_tag" HeaderText="Asset Tag" />
                    <asp:BoundField DataField="SERIAL_NO" HeaderText="Serial No#" />
                    <asp:BoundField DataField="EQUIPMENT_ID" HeaderText="Equipment ID" />
                    <asp:BoundField DataField="QUANTITY" HeaderText="Qty" />
                    <asp:BoundField DataField="UNIT_CODE" HeaderText="Unit Code" />
                    <asp:BoundField DataField="GOOD_TYPE_DESC" HeaderText="Goods Type" />
                    <asp:BoundField DataField="MANUFACTURER_YEAR" HeaderText="Manufacturer Year" />                    
                    <asp:BoundField DataField="IMPORT_DATE" HeaderText="Import Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No#" />
                    <asp:BoundField DataField="INVOICE_DATE" HeaderText="Invoice Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="DOCUMENT_NUMBER" HeaderText="Document No#" />
                    <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Document Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <%--<asp:BoundField DataField="AMOUNT" HeaderText="Amount (THB)" DataFormatString="{0:c}" />--%>
                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount (THB)" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="TAX_RAT_PERCENT" HeaderText="Tax Rate Percent" />
                    <asp:BoundField DataField="TAX_VALUE" HeaderText="Tax Value" DataFormatString="{0:#,##0.00}"/>
                    <asp:BoundField DataField="VAT_VALUE" HeaderText="Vat Value" DataFormatString="{0:#,##0.00}"/>
                    <asp:BoundField DataField="XMLTYPE" HeaderText="XML Type" />  
                    <asp:BoundField DataField="VALID_DATE" HeaderText="Valid Date" DataFormatString="{0:yyyy-MM-dd}" />   
                    <asp:BoundField DataField="EQUIPMENT_TYPE" HeaderText="Equip Type" />    
                    <asp:BoundField DataField="REMAINING_DAY" HeaderText="Remaining Day" />    
                    <asp:BoundField DataField="REMAINING_COST" HeaderText="Remaining Cost (THB)" DataFormatString="{0:#,##0.00}" />           
                    <asp:BoundField DataField="REMAINING_DUTY" HeaderText="Remaining Duty (THB)" DataFormatString="{0:#,##0.00}" />           
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
