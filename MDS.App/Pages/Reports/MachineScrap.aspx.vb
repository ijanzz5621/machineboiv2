Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class MachineScrap
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        'End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text,
                   txtAssetTag.Text, txtValidDate.Text)
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

    Private Function GetListing(boiNo As String, importDate As String, invoiceNo As String, desc As String, equipmentId As String,
                           assetTag As String, validDate As String) As DataSet
        Dim dsResult As DataSet = Nothing
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT BOI_NUMBER, BOI_NUMBER2, invoice_item, description, asset_tag, serial_no, equipment_id, QUANTITY, "
            sSQL = sSQL & "unit_code, good_type_desc, manufacturer_year, import_date, invoice_number, invoice_date, document_number, "
            sSQL = sSQL & "document_date, amount, tax_rat_percent, tax_value, vat_value, xmltype, valid_date, equipment_type, "
            sSQL = sSQL & "REMAINING_DAY,REMAINING_COST,REMAINING_DUTY, download_inv, download_import "
            sSQL = sSQL & "FROM ( "
            sSQL = sSQL & "SELECT c.BOI_NUMBER, concat(c.BOI_NUMBER, c.certificate_number) as BOI_NUMBER2, a.invoice_item, "
            sSQL = sSQL & "a.description, d.asset_tag, d.serial_no, d.equipment_id, 1 as QUANTITY, a.unit_code, e.good_type_desc, "
            sSQL = sSQL & "d.manufacturer_year, b.import_date, a.invoice_number, a.invoice_date, a.document_number, a.document_date, b.amount, "
            sSQL = sSQL & "b.tax_rat_percent, b.tax_value, b.vat_value, a.xmltype, "
            sSQL = sSQL & "TO_CHAR(add_months(b.import_date, 60), 'yyyy-MM-dd') AS valid_date, d.equipment_type, "
            sSQL = sSQL & "TO_CHAR(1825 - ROUND(sysdate - import_date)) AS remaining_day, "
            'sSQL = sSQL & "ROUND((b.amount + (b.amount * b.tax_rat_percent) + (b.amount * b.vat_value)) * (1825 - ROUND(sysdate - import_date)) / 1825, 2) AS REMAINING_COST, "
            sSQL = sSQL & "ROUND((b.amount *  (1825 - ROUND(sysdate - import_date)) / 1825)) AS REMAINING_COST, "
            sSQL = sSQL & "ROUND(b.vat_value + (((b.amount + (b.amount * b.tax_rat_percent) + (b.amount * b.vat_value)) * (1825 - ROUND(sysdate - import_date)) / 1825)*b.tax_rat_percent))  AS REMAINING_DUTY, "
            sSQL = sSQL & "replace(f.raw_filepath, 'D:\Application\AspNet\mds\Files\Invoice\', '') AS download_inv, "
            sSQL = sSQL & "replace(b.raw_filepath, 'D:\Application\AspNet\mds\Files\Entry\', '') AS download_import "
            sSQL = sSQL & "FROM TBL_BOIINFO a LEFT OUTER JOIN (SELECT t1.*,t2.ITEM_NUMBER,t2.ORIGIN_CONTRY , t2.AMOUNT, t2.TAX_VALUE, t2.VAT_VALUE, "
            sSQL = sSQL & "t2.TAX_RAT_PERCENT,t2.H_S_CODE ,t2.INVOICE_NUMBER,t2.INVOICE_ITEM FROM TBL_BOIIMPORTENTRY t1, TBL_BOIIMPORTSUBENTRY t2 "
            sSQL = sSQL & "WHERE t2.IMPORT_ENTRY_NUMBER = t1.IMPORT_ENTRY_NUMBER) b on a.INVOICE_NUMBER = b.INVOICE_NUMBER And a.INVOICE_ITEM = b.INVOICE_ITEM "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOINUMBER c ON a.boi_number = c.boi_number "
            sSQL = sSQL & "LEFT OUTER JOIN v_equipment d ON a.invoice_item = d.invoice_no_item And a.invoice_number = d.invoice_no "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIGOODTYPESCODE e ON a.good_type_code = e.good_type_code "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINVOICEINFO f ON a.invoice_number = f.invoice_number "
            sSQL = sSQL & ") "
            sSQL = sSQL & "WHERE 1=1 "
            'And ROWNUM <= 500 "

            'Filtering
            If boiNo.Trim <> "" Then
                sSQL = sSQL & "And BOI_NUMBER2 = '" & boiNo & "' "
            End If

            If importDate.Trim <> "" Then
                sSQL = sSQL & "AND to_char(IMPORT_DATE, 'yyyy-MM-dd') = '" & DateTime.Parse(importDate).ToString("yyyy-MM-dd") & "' "
            End If

            If invoiceNo.Trim <> "" Then
                sSQL = sSQL & "AND INVOICE_NUMBER = '" & invoiceNo & "' "
            End If

            If desc.Trim <> "" Then
                sSQL = sSQL & "AND UPPER(DESCRIPTION) LIKE '%" & desc.ToUpper & "%' "
            End If

            If equipmentId.Trim <> "" Then
                sSQL = sSQL & "AND equipment_id = '" & equipmentId & "' "
            End If

            If assetTag.Trim <> "" Then
                sSQL = sSQL & "AND asset_tag = '" & assetTag & "' "
            End If

            If validDate.Trim <> "" Then
                sSQL = sSQL & "AND valid_date = '" & DateTime.Parse(validDate).ToString("yyyy-MM-dd") & "' "
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
        Dim ds As DataSet = GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text,
                   txtAssetTag.Text, txtValidDate.Text)

        gvListing.PageIndex = e.NewPageIndex
        gvListing.DataSource = ds
        gvListing.DataBind()
    End Sub

    Protected Sub ExportToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=MachineScrap-" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            gvListing.AllowPaging = False
            Me.GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text,
                          txtAssetTag.Text, txtValidDate.Text)

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

#End Region

End Class