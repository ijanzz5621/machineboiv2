<%@ Page Title="Invoice Info" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="InvoiceInfo.aspx.vb" Inherits="MDS.App.InvoiceInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Invoice Info </h3>

    <hr />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Invoice No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            PO No#           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPONo" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="50"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Vendor No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtVendorNo" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Car No#           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCarNo" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="50"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Invoice Value          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
           <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" ReadOnly="false" ></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Currency           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="10"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Trade Term         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtTradeTerm" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="200"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Raw File Upload          
        </div>
        <div class="col-md-4" style="margin-top:10px;">            
            <asp:FileUpload ID="fupRawUpload" runat="server" CssClass="form-control" Visible="true"  />
            <asp:HyperLink ID="hlRawFilename" runat="server" Visible="false"></asp:HyperLink>
            <asp:HiddenField ID="hidFullFilePath" runat="server" />
            <br />
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
