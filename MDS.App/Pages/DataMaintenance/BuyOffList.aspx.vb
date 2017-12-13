Imports Oracle.ManagedDataAccess.Client

Public Class BuyOffList
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetListing(txtInvoiceNo.Text, txtInvoiceItem.Text)
        End If
    End Sub

    Protected Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListing.PageIndexChanging
        gvListing.PageIndex = e.NewPageIndex
        GetListing(txtInvoiceNo.Text, txtInvoiceItem.Text)
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        GetListing(txtInvoiceNo.Text, txtInvoiceItem.Text)
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

    Private Sub GetListing(invoiceNo As String, invoiceItem As String)

        Try

            GetAppConfig()
            OpenConnection()

            'Dim sSQL = "SELECT a.BOI_NUMBER, a.INVOICE_NUMBER, a.INVOICE_ITEM, a.INSPECTION_CODE AS INSPECTION_STATUS, b.INSPECTION_DESC "
            'sSQL = sSQL & "FROM TBL_BOIINFO a LEFT JOIN TBL_BOIINSPECTIONCODE b ON a.INSPECTION_CODE = b.INSPECTION_CODE WHERE a.INSPECTION_CODE = '01' "

            Dim sSQL = "SELECT a.BOI_NUMBER, a.INVOICE_NUMBER, a.INVOICE_ITEM, a.INSPECTION_CODE AS INSPECTION_STATUS "
            sSQL = sSQL & "FROM TBL_BOIINFO a LEFT JOIN TBL_BOIGOODTYPESCODE b ON a.GOOD_TYPE_CODE = b.GOOD_TYPE_CODE WHERE a.GOOD_TYPE_CODE = '02' "

            If invoiceNo.Trim <> "" Then
                sSQL = sSQL & "AND INVOICE_NUMBER = '" & invoiceNo & "' "
            End If

            If invoiceItem.Trim <> "" Then
                sSQL = sSQL & "AND INVOICE_ITEM = '" & invoiceItem & "' "
            End If

            sSQL = sSQL & "ORDER BY CREATE_DATE "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            gvListing.DataSource = dsResult
            gvListing.DataBind()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region


End Class