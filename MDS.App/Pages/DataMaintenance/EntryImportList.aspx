<%@ Page Title="Entry Import Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EntryImportList.aspx.vb" Inherits="MDS.App.EntryImportList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Entry Import Listing</h3>

    <hr />

    <div class="row">

        <div class="col-md-2">
            <asp:Label ID="lblEntryNo" runat="server" Text="Import Entry No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtImportEntryNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Label ID="lblJobNo" runat="server" Text="Job No#" CssClass=""></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtJobNo" runat="server" CssClass="form-control"></asp:TextBox>
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
                    <asp:BoundField DataField="IMPORT_ENTRY_NUMBER" HeaderText="Import Entry No#" />
                    <asp:BoundField DataField="JOB_NUMBER" HeaderText="Job No#" />
                    <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                    <asp:BoundField DataField="IMPORT_DATE" HeaderText="Import Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="SHIP_FROM" HeaderText="Ship From" />
                    <asp:BoundField DataField="SUBMIT_DATE" HeaderText="Submit Date" DataFormatString="{0:d}"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgViewDetails" runat="server" ImageUrl="~/images/view.png" Width="35px" 
                                PostBackUrl='<%# String.Format("~/Pages/DataMaintenance/EntryImport.aspx?IMPORTNO={0}&JOBNO={1}",
                                                                                                            HttpUtility.UrlEncode(Eval("IMPORT_ENTRY_NUMBER").ToString()),
                                                                                                            HttpUtility.UrlEncode(Eval("JOB_NUMBER").ToString()))%>' />
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
