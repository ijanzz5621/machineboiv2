<%@ Page Title="BOI Upload" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BOIUpload.aspx.vb" Inherits="MDS.App.BOIUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>BOI Upload</h2>

    <span style="font-weight:bold; color:red;">*Note: This upload only support excel with .xlsx extension. If your file is in .xls, please open and "Save As" .xlsx </span>

    <hr style="border: 1px solid black;" />

    <div class="container">

        <div class="row">
            <div style="margin:10px;">

                <div class="col-md-2">BOI Number</div>
                <div class="col-md-4">
                    <%--<asp:TextBox ID="txtBOINumber" runat="server" CssClass="form-control" required></asp:TextBox>--%>
                    <asp:DropDownList ID="ddlBOINo" runat="server" CssClass="form-control" 
                        DataTextField="BOI_NUMBER" DataValueField="BOI_NUMBER" AppendDataBoundItems="true" required>
                        <asp:ListItem Value="">-- Please Select--</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
            
        </div>

        <div class="row">

            <div style="margin:10px;">
                <div class="col-md-2">Good Types</div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlGoodTypes" runat="server" CssClass="form-control" DataValueField="GOOD_TYPE_CODE" DataTextField="GOOD_TYPE_DESC">
                        <%--<asp:ListItem>New</asp:ListItem>
                        <asp:ListItem>Used</asp:ListItem>
                        <asp:ListItem>Demo/Rental</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
            </div>

            
        </div>

        <div class="row">


            <div style="margin:10px;" class="">

                <div class="col-md-2">BOI File</div>
                <div class="col-md-4">
                    <asp:FileUpload ID="fupBOIUpload" runat="server" CssClass="form-control" />
                </div>
                                
            </div>            

        </div>

        <div class="row">

            <div style="margin:10px;">

                <div class="col-md-2">
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-warning" Width="100px" />
                </div>
                
            </div>
            
        </div>

        <hr />

        <div class="row">

            <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="col-md-12">

                <div class="result" id="result">

                    <div class="col-md-12">
                        <asp:GridView ID="gvResult" runat="server" CssClass="table table-bordered" 
                            BackColor="White" BorderColor="#999999" 
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#f0f0f0" />
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="#ff0000" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </div>                

                    <br />                   

                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary" Width="100px" />
                                  

                </div>

            </asp:Panel>

        </div>

        


        <asp:Panel ID="pnlError" runat="server" Visible="true">

            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

        </asp:Panel>       


    </div>

    

</asp:Content>
