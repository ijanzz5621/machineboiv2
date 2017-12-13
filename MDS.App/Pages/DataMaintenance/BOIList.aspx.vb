Imports Oracle.ManagedDataAccess.Client

Public Class BOIList
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GetListing(txtBOINumber.Text, txtInvoiceNo.Text)

        End If

    End Sub

#Region "Functions"

    Private Sub GetListing(boiNo As String, invNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            ' Dim sSQL = "SELECT BOI_NUMBER ""BOI_NUMBER"",  INVOICE_NUMBER ""INVOICE_NUMBER"", INVOICE_ITEM ""INVOICE_ITEM"" "
            Dim sSQL = "SELECT * FROM TBL_BOIINFO WHERE 1=1 "
            'sSQL = sSQL & "FROM TBL_BOIINFO WHERE 1=1 "

            'Dim sSQL = "SELECT * FROM TBL_BOIINFO WHERE 1=1 "
            'sSQL = sSQL & "AND ('" & boiNo.Trim & "' = '' OR BOI_NUMBER = '" & boiNo & "') "
            'sSQL = sSQL & "AND ('" & invNo.Trim & "' = '' OR INVOICE_NUMBER = '" & invNo & "') "

            If boiNo.Trim <> "" Then
                sSQL = sSQL & "AND BOI_NUMBER = '" & boiNo & "' "
            End If

            If invNo.Trim <> "" Then
                sSQL = sSQL & "AND INVOICE_NUMBER = '" & invNo & "' "
            End If

            sSQL = sSQL & "ORDER BY CREATE_DATE DESC"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            'If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then
            gvListing.DataSource = dsResult
            gvListing.DataBind()
            'End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region

#Region "get App.Config"
    Private Sub GetAppConfig()
        cnnOraString = ConfigurationManager.ConnectionStrings("ORA_EQBConnString").ConnectionString
    End Sub
#End Region

#Region "Open Connection"
    Private Sub OpenConnection()
        oOra.OpenOraConnection(cnnOra, cnnOraString)
        'oMSSQL.OpenConnection(cnnBOP, cnnBOPString)
        'oMSSQL.OpenConnection(cnnZStart, cnnZStartString)
    End Sub

    Private Sub CloseConnection()
        oOra.CloseOraConnection(cnnOra)
        'oMSSQL.CloseConnection(cnnBOP)
        'oMSSQL.CloseConnection(cnnZStart)
    End Sub

#End Region

    Protected Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

        gvListing.PageIndex = e.NewPageIndex
        GetListing(txtBOINumber.Text, txtInvoiceNo.Text)

    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        GetListing(txtBOINumber.Text, txtInvoiceNo.Text)
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Response.Redirect("~/Pages/DataMaintenance/BOIUpload.aspx")
    End Sub
End Class