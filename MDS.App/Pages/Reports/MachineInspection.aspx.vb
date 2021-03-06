﻿Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class MachineInspection
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetBOINumberList()
            GetXmlTypeList()
            GetEquipmentTypeList()
            GetInspectionList()
            GetCERNumberList()
            ' GetAddressList()

        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text, txtAssetTag.Text,
        '           ddlFactory.SelectedValue, ddlInspection.SelectedValue, ddlEquipmentType.SelectedValue)
        GetListing(ddlBOINumber.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, ddlInspection.SelectedValue, ddlAddress.SelectedValue, ddlCERNo.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMachineAgeFrom.Text, txtMachineAgeTo.Text, txtMultiFilter.Text)
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportToExcel()
    End Sub

#Region "get App.Config"
    Private Sub GetAppConfig()
        cnnOraString = ConfigurationManager.ConnectionStrings("ORA_EQBConnString").ConnectionString
    End Sub
#End Region

#Region "Open Connection"
    Private Sub OpenConnection()
        oOra.OpenOraConnection(cnnOra, cnnOraString)
    End Sub

    Private Sub CloseConnection()
        oOra.CloseOraConnection(cnnOra)
    End Sub

#End Region

#Region "Functions"

    Private Sub GetBOINumberList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct BOI_NUMBER from TBL_BOINUMBER order by BOI_NUMBER"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlBOINumber.DataSource = dsResult.Tables(0)
            ddlBOINumber.DataTextField = "BOI_NUMBER"
            ddlBOINumber.DataValueField = "BOI_NUMBER"
            ddlBOINumber.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetInspectionList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select INSPECTION_CODE, INSPECTION_DESC FROM TBL_BOIINSPECTIONCODE ORDER BY INSPECTION_DESC"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlInspection.DataSource = dsResult.Tables(0)
            ddlInspection.DataTextField = "INSPECTION_DESC"
            ddlInspection.DataValueField = "INSPECTION_CODE"
            ddlInspection.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetXmlTypeList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct XMLTYPE from TBL_BOIINFO order by XMLTYPE"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlXmlType.DataSource = dsResult.Tables(0)
            ddlXmlType.DataTextField = "XMLTYPE"
            ddlXmlType.DataValueField = "XMLTYPE"
            ddlXmlType.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetEquipmentTypeList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct EQUIPMENT_TYPE from v_equipment order by EQUIPMENT_TYPE"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlEquipmentType.DataSource = dsResult.Tables(0)
            ddlEquipmentType.DataTextField = "EQUIPMENT_TYPE"
            ddlEquipmentType.DataValueField = "EQUIPMENT_TYPE"
            ddlEquipmentType.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetCERNumberList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct CER_NUMBER FROM TBL_BOIINFO ORDER BY CER_NUMBER"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlCERNo.DataSource = dsResult.Tables(0)
            ddlCERNo.DataTextField = "CER_NUMBER"
            ddlCERNo.DataValueField = "CER_NUMBER"
            ddlCERNo.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetAddressList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct replace(replace(address,CHR(13),''),CHR(10),'') as address from TBL_BOIIMPORTENTRY where address IS NOT NULL"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlAddress.DataSource = dsResult.Tables(0)
            ddlAddress.DataTextField = "ADDRESS"
            ddlAddress.DataValueField = "ADDRESS"
            ddlAddress.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Function GetListing(boiNo As String, xmlType As String, equipmentType As String, inspectionCode As String, address As String, cerNo As String, importDateFrom As String, ImportDateTo As String, ageFrom As String, ageTo As String, multiFilter As String) As DataSet
        Dim dsResult As DataSet = Nothing
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT c.BOI_NUMBER, concat(c.BOI_NUMBER, c.certificate_number) as BOI_NUMBER2, a.invoice_item, "
            sSQL = sSQL & "a.description, d.equipment_brand, d.equipment_model, d.asset_tag, d.serial_no, d.equipment_id, 1 as QUANTITY, a.unit_code, e.good_type_desc, "
            sSQL = sSQL & "d.manufacturer_year, b.origin_contry, b.import_date, a.invoice_number, g.inspection_desc, a.cer_number, a.xmltype, d.line, f.vendor_number, "
            sSQL = sSQL & "b.address, d.equipment_type, "
            sSQL = sSQL & "replace(f.raw_filepath, 'D:\Application\AspNet\mds\Files\Invoice\', '') AS download_inv, "
            sSQL = sSQL & "replace(b.raw_filepath, 'D:\Application\AspNet\mds\Files\Entry\', '') AS download_import "

            sSQL = sSQL & ", f.PO_NUMBER, CAR_NUMBER "
            sSQL = sSQL & ", to_number(EXTRACT(YEAR FROM b.IMPORT_DATE)) - to_number(d.manufacturer_year) AS MACHINE_AGE "

            sSQL = sSQL & "FROM TBL_BOIINFO a "
            sSQL = sSQL & "LEFT OUTER JOIN (SELECT t1.*,t2.ITEM_NUMBER,t2.ORIGIN_CONTRY , t2.AMOUNT, t2.TAX_VALUE, t2.VAT_VALUE, t2.TAX_RAT_PERCENT,t2.H_S_CODE ,t2.INVOICE_NUMBER,t2.INVOICE_ITEM "
            sSQL = sSQL & "FROM TBL_BOIIMPORTENTRY t1, TBL_BOIIMPORTSUBENTRY t2 WHERE t2.IMPORT_ENTRY_NUMBER = t1.IMPORT_ENTRY_NUMBER) b on a.INVOICE_NUMBER = b.INVOICE_NUMBER And a.INVOICE_ITEM = b.INVOICE_ITEM "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOINUMBER c ON a.boi_number = c.boi_number "
            sSQL = sSQL & "LEFT OUTER JOIN v_equipment d ON a.invoice_item = d.invoice_no_item And a.invoice_number = d.invoice_no "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIGOODTYPESCODE e ON a.good_type_code = e.good_type_code "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINVOICEINFO f ON a.invoice_number = f.invoice_number "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINSPECTIONCODE g ON a.inspection_code = g.inspection_code "
            sSQL = sSQL & "WHERE a.good_type_code = '02' AND 1=1 "

            'Filtering
            If boiNo.Trim <> "" Then
                sSQL = sSQL & "And a.BOI_NUMBER = '" & boiNo & "' "
            End If

            If importDateFrom.Trim <> "" And ImportDateTo.Trim <> "" Then
                sSQL = sSQL & "And to_char(b.IMPORT_DATE, 'yyyy-MM-dd') between '" & DateTime.Parse(importDateFrom).ToString("yyyy-MM-dd") & "' And '" & DateTime.Parse(ImportDateTo).ToString("yyyy-MM-dd") & "' "
            End If

            If xmlType.Trim <> "" Then
                sSQL = sSQL & "And a.xmltype = '" & xmlType & "' "
            End If

            If equipmentType.Trim <> "" Then
                sSQL = sSQL & "And d.EQUIPMENT_TYPE = '" & equipmentType & "' "
            End If

            If inspectionCode.Trim <> "" Then
                sSQL = sSQL & "And a.inspection_code = '" & inspectionCode & "' "
            End If

            If address.Trim <> "" Then
                sSQL = sSQL & "And b.address = '" & address & "' "
            End If

            If cerNo.Trim <> "" Then
                sSQL = sSQL & "And a.cer_number = '" & cerNo & "' "
            End If

            ' Machine age
            If ageFrom.Trim <> "" And ageTo.Trim <> "" Then
                sSQL = sSQL & " And to_number(EXTRACT(YEAR FROM b.IMPORT_DATE)) - to_number(d.manufacturer_year) between '" & ageFrom & "' and '" & ageTo & "' "
            End If

            If multiFilter.Trim <> "" Then

                sSQL = sSQL & "And ( "
                sSQL = sSQL & "UPPER(f.CAR_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(f.PO_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(a.description) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.asset_tag) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.serial_no) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.equipment_id) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(b.origin_contry) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(a.invoice_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.equipment_brand) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(f.vendor_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & ") "

            End If

            'Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

            gvListing.DataSource = dsResult
            gvListing.DataBind()

            If dsResult.Tables(0).Rows.Count > 0 Then
                btnExport.Visible = True
            Else
                btnExport.Visible = False
            End If

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try
        Return dsResult
    End Function

    Private Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListing.PageIndexChanging
        Dim ds As DataSet = GetListing(ddlBOINumber.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, ddlInspection.SelectedValue, ddlAddress.SelectedValue, ddlCERNo.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMachineAgeFrom.Text, txtMachineAgeTo.Text, txtMultiFilter.Text)

        gvListing.PageIndex = e.NewPageIndex
        gvListing.DataSource = ds
        gvListing.DataBind()
    End Sub

    Protected Sub ExportToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=MachineInspection-" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            gvListing.AllowPaging = False
            Me.GetListing(ddlBOINumber.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, ddlInspection.SelectedValue, ddlAddress.SelectedValue, ddlCERNo.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMachineAgeFrom.Text, txtMachineAgeTo.Text, txtMultiFilter.Text)

            ' Hide the last 2 column
            gvListing.Columns(gvListing.Columns.Count - 1).Visible = False
            gvListing.Columns(gvListing.Columns.Count - 2).Visible = False

            gvListing.HeaderRow.BackColor = System.Drawing.Color.White
            For Each cell As TableCell In gvListing.HeaderRow.Cells
                cell.BackColor = gvListing.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In gvListing.Rows
                row.BackColor = System.Drawing.Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = gvListing.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = gvListing.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                    'cell.CssClass = "text"
                Next
            Next

            gvListing.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            'Dim style As String = "<style> .text { mso-number-format:\@; } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'This function required for excel export
    End Sub

#End Region

    Protected Sub gvListing_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvListing.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim hidImport As HiddenField = e.Row.FindControl("hidRawFileImportEntry")
                'Dim imgImportEntry As ImageButton = e.Row.FindControl("imgViewDetails2")
                Dim hlViewRawImportEntry As HyperLink = e.Row.FindControl("hlViewRawImportEntry")
                If hidImport.Value <> Nothing Then

                    hlViewRawImportEntry.NavigateUrl = "~/Files/Entry/" & HttpUtility.UrlEncode(hidImport.Value)
                    hlViewRawImportEntry.Visible = True
                Else
                    hlViewRawImportEntry.Visible = False

                End If

                Dim hidInvoice As HiddenField = e.Row.FindControl("hidRawFileInvoice")
                'Dim imgInvoice As ImageButton = e.Row.FindControl("imgViewDetails1")
                Dim hlViewRawInvoice As HyperLink = e.Row.FindControl("hlViewRawInvoice")
                If hidInvoice.Value <> Nothing Then

                    hlViewRawInvoice.NavigateUrl = "~/Files/Invoice/" & HttpUtility.UrlEncode(hidInvoice.Value)
                    hlViewRawInvoice.Visible = True
                Else
                    hlViewRawInvoice.Visible = False

                End If

            End If

        Catch ex As Exception

        End Try



    End Sub

End Class