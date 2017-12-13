<%@ Page Title="BOI Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BOIDetails.aspx.vb" Inherits="MDS.App.BOIDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> BOI Info Details </h3>
    <hr style="border: 1px solid #e0e0e0;" />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            BOI No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <%--<asp:TextBox ID="txtBOINo" runat="server" CssClass="form-control" ReadOnly="false"></asp:TextBox>--%>
            <asp:DropDownList ID="ddlBOINo" runat="server" CssClass="form-control" 
                DataTextField="BOI_NUMBER" DataValueField="BOI_NUMBER" AppendDataBoundItems="true" required>
                <asp:ListItem Value="">-- Please Select--</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Invoice No#           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>        

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Invoice Date          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Invoice Item           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtInvoiceItem" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>        

    </div>

    <hr />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">   
            Item No#         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtItemNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">            
            XML Type
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtXMLType" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">
            Document No#            
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtDocNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">   
            Document Date         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtDocDate" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;">   
            BOI Tax Ref#         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtBOITaxRef" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">
            Tax Ref#            
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtTaxRef" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;"> 
            Branch           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">   
            Total Item         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtTotalItem" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;">
            User ID            
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Declare Line No#           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtDeclareLineNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Import Declare No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtImportDeclareNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">  
             Description         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    

        <%--<div class="col-md-2" style="margin-top:10px;">  
            Exempt Type          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtExemptType" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;"> 
            Privilege Type           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPrivilegeType" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">      
            Privilege Condition      
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPrivilegeCondition" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Condition Duty Rate          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtConditionDutyRate" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Percent Exempt Duty           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPercentExemptDuty" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;">    
            Percent Exempt VAT        
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPercentExemptVAT" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">         
            Ref Document No#   
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtRefDocumentNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">     
            Unit Code       
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtUnitCode" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">  
            Quantity          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        

    </div>
    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            Good Type          
        </div>
        <div class="col-md-4" style="margin-top:10px;">            
            <asp:DropDownList ID="ddlGoodType" runat="server" CssClass="form-control" 
                DataTextField="GOOD_TYPE_DESC" DataValueField="GOOD_TYPE_CODE">
            </asp:DropDownList>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            Privilege Type           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPrivilegeType" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>

      <div class="row">

            <div class="col-md-2" style="margin-top:10px;"> 
                Inspection Type           
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:DropDownList ID="ddlInspectionType" runat="server" CssClass="form-control" 
                    DataTextField="INSPECTION_DESC" DataValueField="INSPECTION_CODE" 
                    OnSelectedIndexChanged="ddlInspectionType_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-md-2" style="margin-top:10px;"> 
                Cer No#       
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div>

        <div class="row">

            <div class="col-md-2" style="margin-top:10px;"> 
                Cer Item           
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerItem" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2" style="margin-top:10px;">
                Cer Remark            
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerRemark" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div>
    

            

    <%--<div class="row">

        <div class="col-md-2" style="margin-top:10px;">   
            Privilege Valid From         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPrivilegeFrom" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;">   
            Privilege Valid Until         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtPrivilegeUntil" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>--%>





    <asp:Panel ID="pnlInspection" runat="server">
        <%--
        <div class="row">

            <div class="col-md-2" style="margin-top:10px;"> 
                Inspection Type           
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:DropDownList ID="ddlInspectionType" runat="server" CssClass="form-control" 
                    DataTextField="INSPECTION_DESC" DataValueField="INSPECTION_CODE" 
                    OnSelectedIndexChanged="ddlInspectionType_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-md-2" style="margin-top:10px;"> 
                Cer No#       
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div>

        <div class="row">

            <div class="col-md-2" style="margin-top:10px;"> 
                Cer Item           
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerItem" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2" style="margin-top:10px;">
                Cer Remark            
            </div>
            <div class="col-md-4" style="margin-top:10px;">
                <asp:TextBox ID="txtCerRemark" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div> --%>

    </asp:Panel>   

    <hr />

    <div class="row">

        <div class="col-md-6" style="margin-top:10px;">   
            
            <asp:Button ID="btnSave" runat="server" Text="Save & Submit" CssClass="form-control btn btn-primary" Width="120px" />
            &nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="form-control btn btn-danger" Width="80px" />
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
