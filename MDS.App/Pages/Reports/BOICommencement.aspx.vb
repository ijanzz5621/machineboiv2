Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class BOICommencement
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
        GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMultiFilter.Text)
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

    Private Function GetListing(boiNo As String, goodsType As String, xmlType As String, equipmentType As String, importDateFrom As String, ImportDateTo As String, multiFilter As String) As DataSet
        Dim dsResult As DataSet = Nothing
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT c.BOI_NUMBER, concat(c.BOI_NUMBER, c.certificate_number) as BOI_NUMBER2, c.certificate_number, a.invoice_item,a.description, d.asset_tag, d.serial_no, "
            sSQL = sSQL & "d.equipment_id, 1 as QUANTITY, a.unit_code, e.good_type_desc, d.manufacturer_year, b.ship_from, "
            sSQL = sSQL & "b.import_date, a.invoice_number, a.invoice_date, a.document_number, a.document_date, "
            sSQL = sSQL & "b.amount, b.job_number, a.xmltype, "
            sSQL = sSQL & "replace(f.raw_filepath, 'D:\Application\AspNet\mds\Files\Invoice\', '') AS download_inv, replace(b.raw_filepath, 'D:\Application\AspNet\mds\Files\Entry\', '') AS download_import "

            ' Added by Sharizan on 10Dec2017 1PM
            sSQL = sSQL & ", d.EQUIPMENT_TYPE, f.PO_NUMBER, CAR_NUMBER, b.H_S_CODE "

            sSQL = sSQL & "FROM TBL_BOIINFO a LEFT OUTER JOIN "
            sSQL = sSQL & "(SELECT t1.*,t2.ITEM_NUMBER,t2.ORIGIN_CONTRY , t2.AMOUNT, "
            sSQL = sSQL & "t2.TAX_VALUE, t2.VAT_VALUE, t2.TAX_RAT_PERCENT,t2.H_S_CODE ,t2.INVOICE_NUMBER,t2.INVOICE_ITEM "
            sSQL = sSQL & "FROM TBL_BOIIMPORTENTRY t1, TBL_BOIIMPORTSUBENTRY t2 "
            sSQL = sSQL & "WHERE t2.IMPORT_ENTRY_NUMBER = t1.IMPORT_ENTRY_NUMBER) b on a.INVOICE_NUMBER = b.INVOICE_NUMBER "
            sSQL = sSQL & "And a.INVOICE_ITEM = b.INVOICE_ITEM "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOINUMBER c ON a.boi_number = c.boi_number "
            sSQL = sSQL & "LEFT OUTER JOIN v_equipment d ON a.invoice_item = d.invoice_no_item And a.invoice_number = d.invoice_no "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIGOODTYPESCODE e ON a.good_type_code = e.good_type_code "
            sSQL = sSQL & "LEFT OUTER JOIN TBL_BOIINVOICEINFO f ON a.invoice_number = f.invoice_number "

            sSQL = sSQL & "WHERE 1=1 "
            'And ROWNUM <= 100 "

            'Filtering
            If boiNo.Trim <> "" Then
                sSQL = sSQL & "And a.BOI_NUMBER = '" & boiNo & "' "
            End If

            If importDateFrom.Trim <> "" And ImportDateTo.Trim <> "" Then
                sSQL = sSQL & "And to_char(b.IMPORT_DATE, 'yyyy-MM-dd') between '" & DateTime.Parse(importDateFrom).ToString("yyyy-MM-dd") & "' And '" & DateTime.Parse(ImportDateTo).ToString("yyyy-MM-dd") & "' "
            End If

            If goodsType.Trim <> "" Then
                sSQL = sSQL & "And a.good_type_code = '" & goodsType & "' "
            End If

            If xmlType.Trim <> "" Then
                sSQL = sSQL & "And a.xmltype = '" & xmlType & "' "
            End If

            If equipmentType.Trim <> "" Then
                sSQL = sSQL & "And d.EQUIPMENT_TYPE = '" & equipmentType & "' "
            End If

            If multiFilter.Trim <> "" Then

                sSQL = sSQL & "And ( "
                sSQL = sSQL & "UPPER(f.CAR_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(f.PO_NUMBER) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(a.description) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.asset_tag) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.serial_no) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(d.equipment_id) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(b.ship_from) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(a.invoice_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(a.document_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & "Or UPPER(b.job_number) Like '%" & multiFilter.ToUpper & "%' "
                sSQL = sSQL & ") "


            End If

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
        Dim ds As DataSet = GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMultiFilter.Text)

        gvListing.PageIndex = e.NewPageIndex
        gvListing.DataSource = ds
        gvListing.DataBind()
    End Sub


    Protected Sub ExportToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=BOICommencement-" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            gvListing.AllowPaging = False
            Me.GetListing(ddlBOINumber.SelectedValue, ddlGoodsType.SelectedValue, ddlXmlType.SelectedValue, ddlEquipmentType.SelectedValue, txtImportDateFrom.Text, txtImportDateTo.Text, txtMultiFilter.Text)

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