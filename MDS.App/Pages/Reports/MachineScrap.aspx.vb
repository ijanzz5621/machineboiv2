Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class MachineScrap
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            GetBOINumberList()
            GetGoodsTypeList()
            GetXmlTypeList()
            GetEquipmentTypeList()

        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'GetListing(txtBOINo.Text, txtImportDate.Text, txtInvoiceNo.Text, txtDescription.Text, txtEquipmentId.Text,
        '           txtAssetTag.Text, txtValidDate.Text)
        GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtRemainDayFrom.Text, txtRemainDayTo.Text, txtMultiFilter.Text)
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

    Private Sub GetGoodsTypeList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select GOOD_TYPE_CODE, GOOD_TYPE_DESC from TBL_BOIGOODTYPESCODE order by GOOD_TYPE_DESC"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlGoodsType.DataSource = dsResult.Tables(0)
            ddlGoodsType.DataTextField = "GOOD_TYPE_DESC"
            ddlGoodsType.DataValueField = "GOOD_TYPE_CODE"
            ddlGoodsType.DataBind()

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

    Private Function GetListing(boiNo As String, goodsType As String, xmlType As String, equipmentType As String, importDateFrom As String, ImportDateTo As String, remainDayFrom As String, remainDayTo As String, multiFilter As String) As DataSet
        Dim dsResult As DataSet = Nothing
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT BOI_NUMBER, BOI_NUMBER2, invoice_item, description, asset_tag, serial_no, equipment_id, QUANTITY, "
            sSQL = sSQL & "unit_code, good_type_code, good_type_desc, manufacturer_year, import_date, invoice_number, invoice_date, document_number, "
            sSQL = sSQL & "document_date, amount, tax_rat_percent, tax_value, vat_value, xmltype, job_number, valid_date, equipment_type, "
            sSQL = sSQL & "REMAINING_DAY,REMAINING_COST,REMAINING_DUTY, download_inv, download_import "

            ' added by sharizan on 17 Dec 2017
            sSQL = sSQL & ", ship_from, po_number, car_number "

            sSQL = sSQL & "FROM ( "
            sSQL = sSQL & "SELECT c.BOI_NUMBER, concat(c.BOI_NUMBER, c.certificate_number) as BOI_NUMBER2, a.invoice_item, "
            sSQL = sSQL & "a.description, d.asset_tag, d.serial_no, d.equipment_id, 1 as QUANTITY, a.unit_code, a.good_type_code, e.good_type_desc, "
            sSQL = sSQL & "d.manufacturer_year, b.import_date, a.invoice_number, a.invoice_date, a.document_number, a.document_date, CAST(b.amount/a.QUANTITY AS NUMBER(15,2)) AMOUNT, "
            sSQL = sSQL & "b.tax_rat_percent, CAST(b.tax_value/a.QUANTITY AS NUMBER(15,2)) tax_value, CAST(b.vat_value/a.QUANTITY AS NUMBER(15,2)) vat_value, a.xmltype, b.job_number, "
            sSQL = sSQL & "TO_CHAR(add_months(b.import_date, 60), 'yyyy-MM-dd') AS valid_date, d.equipment_type, b.ship_from, f.po_number, car_number, "
            'sSQL = sSQL & "TO_CHAR(1825 - ROUND(sysdate - import_date)) AS remaining_day, "
            'sSQL = sSQL & "ROUND((b.amount *  (1825 - ROUND(sysdate - import_date)) / 1825)) AS REMAINING_COST, "
            'sSQL = sSQL & "ROUND(b.vat_value + (((b.amount + (b.amount * b.tax_rat_percent) + (b.amount * b.vat_value)) * (1825 - ROUND(sysdate - import_date)) / 1825)*b.tax_rat_percent))  AS REMAINING_DUTY, "
            sSQL = sSQL & "CASE WHEN (1825 - ROUND(sysdate - import_date)) > 0 THEN TO_CHAR(1825 - ROUND(sysdate - import_date)) ELSE '0' END AS remaining_day, "
            sSQL = sSQL & "CASE WHEN (1825 - ROUND(sysdate - import_date)) > 0 THEN ROUND(((b.amount/a.QUANTITY) *  (1825 - ROUND(sysdate - import_date)) / 1825)) ELSE 0 END AS REMAINING_COST, "
            sSQL = sSQL & "CASE WHEN (1825 - ROUND(sysdate - import_date)) > 0 THEN (ROUND(((b.amount/a.QUANTITY) *  (1825 - ROUND(sysdate - import_date)) / 1825)) *  b.tax_rat_percent)/100 ELSE 0 END  AS REMAINING_DUTY, "
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
                sSQL = sSQL & "And BOI_NUMBER = '" & boiNo & "' "
            End If

            If importDateFrom.Trim <> "" And ImportDateTo.Trim <> "" Then
                sSQL = sSQL & "And to_char(IMPORT_DATE, 'yyyy-MM-dd') between '" & DateTime.Parse(importDateFrom).ToString("yyyy-MM-dd") & "' And '" & DateTime.Parse(ImportDateTo).ToString("yyyy-MM-dd") & "' "
            End If

            If goodsType.Trim <> "" Then
                sSQL = sSQL & "And good_type_code = '" & goodsType & "' "
            End If

            If xmlType.Trim <> "" Then
                sSQL = sSQL & "And xmltype = '" & xmlType & "' "
            End If

            If equipmentType.Trim <> "" Then
                sSQL = sSQL & "And EQUIPMENT_TYPE = '" & equipmentType & "' "
            End If

            If remainDayFrom.Trim <> "" And remainDayTo.Trim <> "" Then
                sSQL = sSQL & "And REMAINING_DAY between " & remainDayFrom & " And " & remainDayTo & " "
            End If

            If multiFilter.Trim <> "" Then

                sSQL = sSQL & "And ( "
                sSQL = sSQL & "UPPER(CAR_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(PO_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(description) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(asset_tag) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(serial_no) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(equipment_id) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(ship_from) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(invoice_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(document_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(job_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & ") "


            End If

            sSQL = sSQL & "ORDER BY IMPORT_DATE, invoice_number, invoice_item "

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
        Dim ds As DataSet = GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtRemainDayFrom.Text, txtRemainDayTo.Text, txtMultiFilter.Text)

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
            Me.GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtRemainDayFrom.Text, txtRemainDayTo.Text, txtMultiFilter.Text)

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