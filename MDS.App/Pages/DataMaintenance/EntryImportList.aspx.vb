Imports Oracle.ManagedDataAccess.Client

Public Class EntryImportList
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GetListing(txtImportEntryNo.Text, txtJobNo.Text)

        End If

    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Response.Redirect("~/Pages/DataMaintenance/EntryImport.aspx")

    End Sub

    Protected Sub gvListing_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListing.PageIndexChanging
        gvListing.PageIndex = e.NewPageIndex
        GetListing(txtImportEntryNo.Text, txtJobNo.Text)
        'GetTotalAmount(txtImportEntryNo.Text)
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

    Private Sub GetListing(importEntryNo As String, jobNo As String)

        Try

            GetAppConfig()
            OpenConnection()
            importEntryNo = UCase(importEntryNo).Trim
            Dim sSQL = "SELECT * "
            sSQL = sSQL & "FROM TBL_BOIIMPORTENTRY WHERE 1=1 "

            If importEntryNo.Trim <> "" Then
                sSQL = sSQL & "AND IMPORT_ENTRY_NUMBER = '" & importEntryNo & "' "
            End If

            If jobNo.Trim <> "" Then
                sSQL = sSQL & "AND JOB_NUMBER = '" & jobNo & "' "
            End If

            sSQL = sSQL & "ORDER BY IMPORT_DATE DESC "

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

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        GetListing(txtImportEntryNo.Text, txtJobNo.Text)
    End Sub

End Class