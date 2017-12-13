<%@ Page Title="BOI Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BOI.aspx.vb" Inherits="MDS.App.BOI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    

    <h3>BOI Master Setup</h3>

    <hr />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            BOI No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtBOINo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="30"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Factory          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:DropDownList ID="ddlFactory" runat="server" CssClass="form-control">
                <asp:ListItem Value="">-- Please Select --</asp:ListItem>
                <asp:ListItem Value="MTHAI">MTHAI</asp:ListItem>
                <asp:ListItem Value="MMT">MMT</asp:ListItem>
            </asp:DropDownList>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Certificate No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCertificateNo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            &nbsp;       
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            &nbsp;
        </div>        

    </div>

    <hr />

    <div class="row">

        <div class="col-md-6" style="margin-top:10px;">   
            
            <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="form-control btn btn-primary" Width="100px" />
            &nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="form-control btn btn-danger" Width="80px" Visible="false" />
            &nbsp;
            <input type="button" title="Back" class="form-control btn btn-warning" onclick="history.go(-1)" value="Back" style="width:80px;" />
                     
        </div>
        <div class="col-md-2" style="margin-top:10px;">            
        </div>
        <div class="col-md-4" style="margin-top:10px;">
        </div>

    </div>

    <div class="row">

        <div class="col-md-12" style="margin-top:10px;">    
            
            <asp:Panel ID="pnlError" runat="server" Visible="false">

                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>

            </asp:Panel> 
                    
        </div>        

    </div>

    <script type="text/javascript">

        $('#MainContent_btnDelete').on('click', function (e) {

            e.preventDefault();

            $.confirm({
                title: 'Data Removal Confirmation',
                content: 'Are you sure to remove?',
                buttons: {
                    <%--confirm: function () {
                        __doPostBack($('#<%= btnDelete.ClientID %>').attr('name'), '');
                    },--%>
                    confirm: {
                        text: 'PROCEED',
                        btnClass: 'btn-blue',
                        keys: ['enter', 'shift'],
                        action: function () {
                            __doPostBack($('#<%= btnDelete.ClientID %>').attr('name'), '');
                        }
                    },
                    cancel: function () {
                    }
                }
            });
        });

    </script>

</asp:Content>
