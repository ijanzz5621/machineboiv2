Imports Oracle.ManagedDataAccess.Client

Public Class BOI
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Request.QueryString("BOINO") <> Nothing Then

                'Load Data
                'Get data from database
                GetData(Request.QueryString("BOINO"))

            End If

        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL As String = ""

            'Check from database if the record already exist
            'If yes, just update and delete the old file
            sSQL = "SELECT * FROM TBL_BOINUMBER WHERE BOI_NUMBER = '" & txtBOINo.Text & "' "
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Update
                sSQL = "UPDATE TBL_BOINUMBER SET "
                sSQL = sSQL & "FACTORY = '" & ddlFactory.SelectedValue & "' "
                sSQL = sSQL & ", CERTIFICATE_NUMBER = '" & txtCertificateNo.Text & "' "
                sSQL = sSQL & " WHERE BOI_NUMBER = '" & txtBOINo.Text & "' "
                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            Else
                'Insert new
                sSQL = "INSERT INTO TBL_BOINUMBER (BOI_NUMBER, FACTORY, CERTIFICATE_NUMBER) "
                sSQL = sSQL & "VALUES ("
                sSQL = sSQL & "'" & txtBOINo.Text & "', "
                sSQL = sSQL & "'" & ddlFactory.SelectedValue & "', "
                sSQL = sSQL & "'" & txtCertificateNo.Text & "' "
                sSQL = sSQL & ")"

                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            End If

            Response.Redirect("~/Pages/Setup/BOIList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Try

            Dim sSQL As String = ""

            GetAppConfig()
            OpenConnection()

            sSQL = "DELETE FROM TBL_BOINUMBER WHERE BOI_NUMBER = '" & txtBOINo.Text & "' "

            Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)
            CloseConnection()

            Response.Redirect("~/Pages/Setup/BOIList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

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

    Private Sub GetData(boiNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOINUMBER WHERE BOI_NUMBER = '" & boiNo & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtBOINo.Text = dsResult.Tables(0).Rows(0)("BOI_NUMBER").ToString()
                txtBOINo.ReadOnly = True
                ddlFactory.SelectedValue = dsResult.Tables(0).Rows(0)("FACTORY").ToString()
                txtCertificateNo.Text = dsResult.Tables(0).Rows(0)("CERTIFICATE_NUMBER").ToString()

                btnDelete.Visible = True

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region

End Class