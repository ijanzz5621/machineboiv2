Imports Oracle.ManagedDataAccess.Client

Public Class BuyOffUpdate
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            If Request.QueryString("INVOICENO") <> Nothing And Request.QueryString("INVOICEITEM") <> Nothing Then

                GetData(Request.QueryString("INVOICENO"), Request.QueryString("INVOICEITEM"))

            End If

        End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL As String = ""

            'Update
            sSQL = "UPDATE TBL_BOIINFO SET "
            sSQL = sSQL & " CER_NUMBER = '" & txtCerNo.Text & "' "
            sSQL = sSQL & ", CER_ITEM = '" & txtCerItem.Text & "' "
            sSQL = sSQL & ", CER_REMARK = '" & txtCerRemark.Text & "' "
            sSQL = sSQL & ", INSPECTION_CODE = '02' "

            sSQL = sSQL & " WHERE INVOICE_NUMBER = '" & Request.QueryString("INVOICENO") & "' AND INVOICE_ITEM = '" & Request.QueryString("INVOICEITEM") & "'"

            Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            Response.Redirect("~/Pages/DataMaintenance/BuyOffList.aspx")

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

    Private Sub GetData(invNo As String, invItem As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT CER_NUMBER, CER_ITEM, CER_REMARK FROM TBL_BOIINFO WHERE INVOICE_NUMBER = '" & invNo & "' AND INVOICE_ITEM = '" & invItem & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtCerNo.Text = dsResult.Tables(0).Rows(0)("CER_NUMBER").ToString()
                txtCerItem.Text = dsResult.Tables(0).Rows(0)("CER_ITEM").ToString()
                txtCerRemark.Text = dsResult.Tables(0).Rows(0)("CER_REMARK").ToString()

            End If

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region

End Class