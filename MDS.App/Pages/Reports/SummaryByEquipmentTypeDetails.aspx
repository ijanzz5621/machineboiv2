<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Report.Master" CodeBehind="SummaryByEquipmentTypeDetails.aspx.vb" Inherits="MDS.App.SummaryByEquipmentTypeDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Request.QueryString("EquipmentType") %>

</asp:Content>
