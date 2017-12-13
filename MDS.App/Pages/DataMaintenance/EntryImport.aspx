<%@ Page Title="Entry Import" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EntryImport.aspx.vb" Inherits="MDS.App.EntryImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Entry Import </h3>

    <hr />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Import Entry No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtImportEntryNo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="50"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Job No#           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtJobNo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="50"></asp:TextBox>
        </div>                

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Ship From          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtShipFrom" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="50"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Address           
        </div>
    
       <%-- <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" ReadOnly="false" TextMode="MultiLine" Rows="4" MaxLength="200"></asp:TextBox>
        </div>--%>
        
            <%-- <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="50"></asp:TextBox> --%>
        <div class="col-md-4" style="margin-top:10px;">
              <asp:DropDownList ID="ddlAddress" runat="server" CssClass="form-control">
                <asp:ListItem Value="">-- Please Select --</asp:ListItem>
                <asp:ListItem Value="MTAI">MTAI</asp:ListItem>
                <asp:ListItem Value="MMT">MMT</asp:ListItem>
                </asp:DropDownList>
        </div>
    </div> 

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Total Amount (THB)        
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Import Date           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtImportDate" runat="server" CssClass="form-control datepicker" ReadOnly="false" data-date-format="yyyy-mm-dd"></asp:TextBox>
        </div>      

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Submit Date          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtSubmitDate" runat="server" CssClass="form-control datepicker" ReadOnly="false" data-date-format="yyyy-mm-dd"></asp:TextBox>
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

    <script src="/Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

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
