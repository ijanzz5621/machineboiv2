Imports Oracle.ManagedDataAccess.Client

Public Class SubEntryImport
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

            If Request.QueryString("IMPORTNO") <> Nothing And Request.QueryString("ITEMNO") <> Nothing Then

                'Load Data
                'Get data from database
                GetData(Request.QueryString("IMPORTNO"), Request.QueryString("ITEMNO"))

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
            sSQL = "SELECT * FROM TBL_BOIIMPORTSUBENTRY WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND ITEM_NUMBER = '" & txtItemNo.Text & "'"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            'removing the formatting
            txtAmount.Text = Replace(txtAmount.Text, ",", "")
            txtTaxValue.Text = Replace(txtTaxValue.Text, ",", "")
            txtVatValue.Text = Replace(txtVatValue.Text, ",", "")
            txtTaxRatPercent.Text = Replace(txtTaxRatPercent.Text, "%", "")

            'add in calculation
            txtTaxValue.Text = (Convert.ToDouble(txtTaxRatPercent.Text) * Convert.ToDouble(txtAmount.Text)) / 100
            txtVatValue.Text = (Convert.ToDouble(txtTaxValue.Text) + Convert.ToDouble(txtAmount.Text)) * 0.07

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Update
                sSQL = "UPDATE TBL_BOIIMPORTSUBENTRY SET "
                sSQL = sSQL & " ORIGIN_CONTRY = '" & ddlOriginCountry.SelectedValue & "' "
                sSQL = sSQL & ", AMOUNT = '" & txtAmount.Text & "' "
                sSQL = sSQL & ", TAX_VALUE = '" & txtTaxValue.Text & "' "
                sSQL = sSQL & ", VAT_VALUE = '" & txtVatValue.Text & "' "
                sSQL = sSQL & ", TAX_RAT_PERCENT = '" & txtTaxRatPercent.Text & "' "
                sSQL = sSQL & ", H_S_CODE = '" & txtHSCode.Text & "' "
                sSQL = sSQL & ", INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' "
                sSQL = sSQL & ", INVOICE_ITEM = '" & txtInvoiceItemNo.Text & "' "

                sSQL = sSQL & " WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND ITEM_NUMBER = '" & txtItemNo.Text & "'"
                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            Else
                'Insert new
                sSQL = "INSERT INTO TBL_BOIIMPORTSUBENTRY (IMPORT_ENTRY_NUMBER, ITEM_NUMBER, ORIGIN_CONTRY, AMOUNT, TAX_VALUE, VAT_VALUE, TAX_RAT_PERCENT, H_S_CODE, INVOICE_NUMBER, INVOICE_ITEM) "
                sSQL = sSQL & "VALUES ("
                sSQL = sSQL & "'" & txtImportEntryNo.Text & "', "
                sSQL = sSQL & "'" & txtItemNo.Text & "', "
                sSQL = sSQL & "'" & ddlOriginCountry.SelectedValue & "', "
                sSQL = sSQL & "'" & txtAmount.Text & "', "
                sSQL = sSQL & "'" & txtTaxValue.Text & "', "
                sSQL = sSQL & "'" & txtVatValue.Text & "', "
                sSQL = sSQL & "'" & txtTaxRatPercent.Text & "', "
                sSQL = sSQL & "'" & txtHSCode.Text & "', "
                sSQL = sSQL & "'" & txtInvoiceNo.Text & "', "
                sSQL = sSQL & "'" & txtInvoiceItemNo.Text & "' "
                sSQL = sSQL & ")"

                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            End If

            'Response.Redirect("~/Pages/DataMaintenance/SubEntryImportList.aspx")
            GetData(Request.QueryString("IMPORTNO"), Request.QueryString("ITEMNO"))

        Catch ex As Exception

            Dim errorMsg As String = ex.ToString
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

            sSQL = "DELETE FROM TBL_BOIIMPORTSUBENTRY WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND ITEM_NUMBER = '" & txtItemNo.Text & "'"

            Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)
            CloseConnection()

            Response.Redirect("SubEntryImportList.aspx")    '~/Pages/DataMaintenance/

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

    Private Function FormatX(dblValue As Object) As String
        Return String.Format("{0:#,##0.00}", dblValue)
    End Function

    Private Function FormatY(dblValue As Object) As String
        Return String.Format("{0:0%}", dblValue / 100)
    End Function

    Private Sub GetData(importNo As String, itemNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIIMPORTSUBENTRY WHERE IMPORT_ENTRY_NUMBER = '" & importNo & "' AND ITEM_NUMBER = '" & itemNo & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtImportEntryNo.Text = dsResult.Tables(0).Rows(0)("IMPORT_ENTRY_NUMBER").ToString()
                txtImportEntryNo.ReadOnly = True
                txtItemNo.Text = dsResult.Tables(0).Rows(0)("ITEM_NUMBER").ToString()
                txtItemNo.ReadOnly = True
                ddlOriginCountry.SelectedValue = dsResult.Tables(0).Rows(0)("ORIGIN_CONTRY").ToString()
                txtAmount.Text = FormatX(dsResult.Tables(0).Rows(0)("AMOUNT"))
                txtTaxValue.Text = FormatX(dsResult.Tables(0).Rows(0)("TAX_VALUE"))
                txtVatValue.Text = FormatX(dsResult.Tables(0).Rows(0)("VAT_VALUE"))
                txtTaxRatPercent.Text = FormatY(dsResult.Tables(0).Rows(0)("TAX_RAT_PERCENT"))
                txtHSCode.Text = dsResult.Tables(0).Rows(0)("H_S_CODE").ToString()
                txtInvoiceNo.Text = dsResult.Tables(0).Rows(0)("INVOICE_NUMBER").ToString()
                txtInvoiceItemNo.Text = dsResult.Tables(0).Rows(0)("INVOICE_ITEM").ToString()

                btnDelete.Visible = True

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub RemoveData()



    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/Pages/DataMaintenance/SubEntryImportList.aspx")
    End Sub


#End Region

End Class