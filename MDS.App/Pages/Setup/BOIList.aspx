<%@ Page Title="BOI Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BOIList.aspx.vb" Inherits="MDS.App.BOIList1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>BOI Master Listing</h3>

    <hr />

    <div class="row">

        <div class="col-md-2">
            <asp:Label ID="lblBOINo" runat="server" Text="BOI No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtBOINo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
        </div>
        <div class="col-md-4">
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
                    <asp:BoundField DataField="BOI_NUMBER" HeaderText="BOI No#" />
                    <asp:BoundField DataField="FACTORY" HeaderText="Factory" />
                    <asp:BoundField DataField="CERTIFICATE_NUMBER" HeaderText="Certificate No#" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgViewDetails" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Pages/Setup/BOI.aspx?BOINO={0}",
                                                                    HttpUtility.UrlEncode(Eval("BOI_NUMBER").ToString()))%>' />
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
