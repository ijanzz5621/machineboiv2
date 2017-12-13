<%@ Page Title="Buy Off List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BuyOffList.aspx.vb" Inherits="MDS.App.BuyOffList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Buy Off Listing</h3>

    <hr />

    <div class="row">

        <div class="col-md-2">
            <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Label ID="lblInvoiceItem" runat="server" Text="Invoice Item" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtInvoiceItem" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    <div class="row">

        <div class="col-md-6" style="padding-top:10px;">
            <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="form-control btn btn-warning" Width="100px" />
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
                    <asp:BoundField DataField="BOI_NUMBER" HeaderText="BOI No#" />
                    <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No#" />
                    <asp:BoundField DataField="INVOICE_ITEM" HeaderText="Invoice Item" />
                    <%-- %><asp:BoundField DataField="INSPECTION_DESC" HeaderText="Inspection Status" />--%>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="80px" CssClass="form-control btn btn-primary"
                                PostBackUrl='<%# String.Format("~/Pages/DataMaintenance/BuyOffUpdate.aspx?INVOICENO={0}&INVOICEITEM={1}",
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
