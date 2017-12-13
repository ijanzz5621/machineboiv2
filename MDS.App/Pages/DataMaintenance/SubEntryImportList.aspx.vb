Imports Oracle.ManagedDataAccess.Client

Public Class SubEntryImportList
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Public ImportEntryNo As String = String.Empty
    Public ItemNo2 As String = String.Empty
    Public InvoiceNo As String = String.Empty
    Public TaxRate As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ImportEntryNo = Request.QueryString("ImportEntryNo")
        ItemNo2 = Request.QueryString("ItemNo2")
        InvoiceNo = Request.QueryString("InvoiceNo")
        TaxRate = Request.QueryString("TaxRate")

        If Not IsPostBack Then
            'GetListing(txtImportEntryNo.Text, txtItemNo.Text
            txtImportEntryNo.Text = ImportEntryNo
            txtItemNo.Text = ItemNo2
            TxtInvoiceNo.Text = InvoiceNo
            TxtTaxRate.Text = TaxRate
            GetListing(txtImportEntryNo.Text, txtItemNo.Text, TxtInvoiceNo.Text, TxtTaxRate.Text)
        End If

    End Sub

    Protected Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListing.PageIndexChanging
        gvListing.PageIndex = e.NewPageIndex
        'GetListing(txtImportEntryNo.Text, txtItemNo.Text)
        GetListing(txtImportEntryNo.Text, txtItemNo.Text, TxtInvoiceNo.Text, TxtTaxRate.Text)
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

    ' Private Sub GetListing(importEntryNo As String, itemNo As String)
    ', invoiceNo As String, taxrate As String)
    Private Sub GetListing(importEntryNo As String, itemNo As String, invoiceNo As String, taxrate As String)

        Try

            GetAppConfig()
            OpenConnection()
            importEntryNo = UCase(importEntryNo).Trim
            Dim sSQL = "SELECT * "
            sSQL = sSQL & "FROM TBL_BOIIMPORTSUBENTRY WHERE 1=1 "

            If importEntryNo.Trim <> "" Then
                sSQL = sSQL & "And IMPORT_ENTRY_NUMBER = '" & importEntryNo & "' "
        End If

            If itemNo.Trim <> "" Then
                sSQL = sSQL & "AND ITEM_NUMBER = '" & itemNo & "' "
            End If

            If invoiceNo.Trim <> "" Then
                sSQL = sSQL & "AND INVOICE_NUMBER = '" & invoiceNo & "' "
            End If

            If taxrate.Trim <> "" Then
                sSQL = sSQL & "AND TAX_RAT_PERCENT = '" & taxrate & "' "
            End If


            sSQL = sSQL & "ORDER BY ITEM_NUMBER "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            gvListing.DataSource = dsResult
            gvListing.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        GetListing(txtImportEntryNo.Text, txtItemNo.Text, TxtInvoiceNo.Text, TxtTaxRate.Text)
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Response.Redirect("~/Pages/DataMaintenance/SubEntryImport.aspx")
    End Sub

#End Region

End Class