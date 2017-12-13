Imports Oracle.ManagedDataAccess.Client

Public Class BOIList1
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetListing(txtBOINo.Text)
        End If
    End Sub

    Protected Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListing.PageIndexChanging
        gvListing.PageIndex = e.NewPageIndex
        GetListing(txtBOINo.Text)
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        GetListing(txtBOINo.Text)
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Response.Redirect("~/Pages/Setup/BOI.aspx")
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

    Private Sub GetListing(boiNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * "
            sSQL = sSQL & "FROM TBL_BOINUMBER WHERE 1=1 "

            If boiNo.Trim <> "" Then
                sSQL = sSQL & "AND BOI_NUMBER = '" & boiNo & "' "
            End If

            sSQL = sSQL & "ORDER BY BOI_NUMBER, FACTORY DESC "

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