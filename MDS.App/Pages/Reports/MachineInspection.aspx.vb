Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class MachineInspection
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetFactory()
            GetInspectionDesc()
            GetEquipmentType()
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text, txtAssetTag.Text,
                   ddlFactory.SelectedValue, ddlInspection.SelectedValue, ddlEquipmentType.SelectedValue)
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

    Private Sub GetFactory()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct factory from tbl_boinumber "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlFactory.DataSource = dsResult.Tables(0)
                ddlFactory.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetInspectionDesc()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct inspection_desc from TBL_BOIINSPECTIONCODE "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlInspection.DataSource = dsResult.Tables(0)
                ddlInspection.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetEquipmentType()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct equipment_type from v_equipment "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlEquipmentType.DataSource = dsResult.Tables(0)
                ddlEquipmentType.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Function GetListing(boiNo As String, importDate As String, invoiceNo As String, desc As String,
                           equipmentId As String, assetTag As String, factory As String, inspectionDesc As String,
                           equipmentType As String) As DataSet
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
            sSQL = sSQL & "FROM TBL_BOIINFO a "
            sSQL = sSQL & "LEFT OUTER JOIN (SELECT t1.*,t2.ITEM_NUMBER,t2.ORIGIN_CONTRY , t2.AMOUNT, t2.TAX_VALUE, t2.VAT_VALUE, t2.TAX_RAT_PERCENT,t2.H_S_CODE ,t2.INVOICE_NUMBER,t2.INVOICE_ITEM "
            sSQL = sSQL & "FROM TBL_BOIIMPORTENTRY t1, TBL_BOIIMPORTSUBENTRY t2 WHERE t2.IMPORT_ENTRY_NUMBER = t1.IMPORT_ENTRY_NUMBER) b on a.INVOICE_NUMBER = b.INVOICE_NUMBER And a.INVOICE_ITEM = b.INVOICE_ITEM "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOINUMBER c ON a.boi_number = c.boi_number "
            sSQL = sSQL & "LEFT OUTER JOIN v_equipment d ON a.invoice_item = d.invoice_no_item And a.invoice_number = d.invoice_no "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIGOODTYPESCODE e ON a.good_type_code = e.good_type_code "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINVOICEINFO f ON a.invoice_number = f.invoice_number "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINSPECTIONCODE g ON a.inspection_code = g.inspection_code "
            sSQL = sSQL & "WHERE a.good_type_code = '02' AND 1=1 "
            'And ROWNUM <= 500 "

            'Filtering
            If boiNo.Trim <> "" Then
                sSQL = sSQL & "And c.BOI_NUMBER = '" & boiNo & "' "
            End If

            If importDate.Trim <> "" Then
                sSQL = sSQL & "AND to_char(b.IMPORT_DATE, 'yyyy-MM-dd') = '" & DateTime.Parse(importDate).ToString("yyyy-MM-dd") & "' "
            End If

            If invoiceNo.Trim <> "" Then
                sSQL = sSQL & "AND a.INVOICE_NUMBER = '" & invoiceNo & "' "
            End If

            If desc.Trim <> "" Then
                sSQL = sSQL & "AND UPPER(a.DESCRIPTION) LIKE '%" & desc.ToUpper & "%' "
            End If

            If equipmentId.Trim <> "" Then
                sSQL = sSQL & "AND d.equipment_id = '" & equipmentId & "' "
            End If

            If assetTag.Trim <> "" Then
                sSQL = sSQL & "AND d.asset_tag = '" & assetTag & "' "
            End If

            If factory.Trim <> "" Then
                sSQL = sSQL & "AND c.factory = '" & factory & "' "
            End If

            If inspectionDesc.Trim <> "" Then
                sSQL = sSQL & "AND g.inspection_desc = '" & inspectionDesc & "' "
            End If

            If equipmentType.Trim <> "" Then
                sSQL = sSQL & "AND d.equipment_type = '" & equipmentType & "' "
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
        Dim ds As DataSet = GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text, txtAssetTag.Text,
                   ddlFactory.SelectedValue, ddlInspection.SelectedValue, ddlEquipmentType.SelectedValue)

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
            Me.GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text,
                          txtAssetTag.Text, ddlFactory.SelectedValue, ddlInspection.SelectedValue, ddlEquipmentType.SelectedValue)

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