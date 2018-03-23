<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserList.aspx.vb" Inherits="MDS.App.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> User Listing</h3>

    <hr />

    <div class="row">

        <div class="col-md-2">
            <asp:Label ID="lblUserID" runat="server" Text="User ID" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Label ID="lblUsername" runat="server" Text="User Name" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
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
                    <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No#" />
                    <asp:BoundField DataField="PO_NUMBER" HeaderText="PO No#" />
                    <asp:BoundField DataField="VENDOR_NUMBER" HeaderText="Vendor No#" />
                    <asp:BoundField DataField="CAR_NUMBER" HeaderText="Car No#" />
                    <asp:BoundField DataField="UNIT_PRICE" HeaderText="Invoice Value" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="CURRENCY" HeaderText="Currency" />
                    <asp:BoundField DataField="TRADE_TERM" HeaderText="Trade Term" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgViewDetails" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Pages/Setup/InvoiceInfo.aspx?INVOICENO={0}",
                                                        HttpUtility.UrlEncode(Eval("INVOICE_NUMBER").ToString()))%>' />
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
