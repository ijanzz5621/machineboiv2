<%@ Page Title="BOI Info Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BOIList.aspx.vb" Inherits="MDS.App.BOIList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>BOI Info Listing</h3>

    <hr />
        
    <div class="row">

        <div class="col-md-2">
            <asp:Label ID="lblBOINo" runat="server" Text="BOI No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtBOINumber" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Label ID="lblInvNo" runat="server" Text="Invoice No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    <div class="row">

        <div class="col-md-6" style="padding-top:10px;">
            <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="form-control btn btn-warning" Width="100px" />
            &nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="form-control btn btn-info" Width="100px" />
        </div>
        
    </div>

    <hr />

    <div class="row">

        <div class="col-md-12">

            <asp:GridView ID="gvListing" runat="server" CssClass="table table-responsive table-bordered" 
                BackColor="White" BorderColor="#999999" 
                BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                AllowPaging="True" PageSize="25" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="gvListing_PageIndexChanging" 
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"  
                >
                <AlternatingRowStyle BackColor="#f0f0f0" />
                <Columns>
                    <asp:BoundField DataField="BOI_NUMBER" HeaderText="BOI NO#" />
                    <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="INVOICE NO#" />
                    <asp:BoundField DataField="INVOICE_ITEM" HeaderText="INVOICE ITEM" />
                    <asp:BoundField DataField="DOCUMENT_NUMBER" HeaderText="Document Number" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                    <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />
                    <asp:BoundField DataField="UNIT_CODE" HeaderText="Unit Code" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgViewDetails" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Pages/DataMaintenance/BOIDetails.aspx?INVNO={0}&INVITEM={1}",
                                                            HttpUtility.UrlEncode(Eval("INVOICE_NUMBER").ToString()),
                                                            HttpUtility.UrlEncode(Eval("INVOICE_ITEM").ToString()))%>' />
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

    

</asp:Content>
