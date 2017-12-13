<%@ Page Title="Buy Off Update" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BuyOffUpdate.aspx.vb" Inherits="MDS.App.BuyOffUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3> Certificate Info </h3>

    <hr />

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            CER No#          
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCerNo" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="50"></asp:TextBox>
        </div>
        <div class="col-md-2" style="margin-top:10px;"> 
            CER Item           
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCerItem" runat="server" CssClass="form-control" ReadOnly="false" required MaxLength="10"></asp:TextBox>
        </div>                

    </div>

    <div class="row">

        <div class="col-md-2" style="margin-top:10px;">  
            CER Remark         
        </div>
        <div class="col-md-4" style="margin-top:10px;">
            <asp:TextBox ID="txtCerRemark" runat="server" CssClass="form-control" ReadOnly="false" TextMode="MultiLine" Rows="4" MaxLength="100"></asp:TextBox>
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

</asp:Content>
