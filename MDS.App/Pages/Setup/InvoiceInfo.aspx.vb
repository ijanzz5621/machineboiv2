Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class InvoiceInfo
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Request.QueryString("INVOICENO") <> Nothing Then

                'Load Data
                'Get data from database
                GetData(Request.QueryString("INVOICENO"))

            End If

        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            GetAppConfig()
            OpenConnection()

            'Generate the filename
            Dim filename As String = ""
            Dim filePath As String = ""

            Dim sSQL As String = ""

            If fupRawUpload.HasFile Then

                filename = Path.GetFileName(fupRawUpload.PostedFile.FileName)
                'Rename the file. Add datetime
                Dim fi As New FileInfo(filename)
                filename = filename.Replace(fi.Extension, "_" & System.DateTime.Now.ToString("yyyyMMddHHmmss") & fi.Extension)
                filePath = Server.MapPath("\Files\Invoice\") + filename

            End If

            'Check from database if the record already exist
            'If yes, just update and delete the old file
            sSQL = "SELECT * FROM TBL_BOIINVOICEINFO WHERE INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' "
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                If fupRawUpload.HasFile And dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString() <> Nothing And dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString().Length > 0 Then

                    'Remove file
                    Try
                        Dim fileToRemove = dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString()
                        Dim fi As FileInfo = New FileInfo(fileToRemove)
                        fi.Delete()
                    Catch ex As Exception
                        'Ignore. The file may not exist
                    End Try


                End If

                'removing the formatting
                txtUnitPrice.Text = Replace(txtUnitPrice.Text, ",", "")

                'Update
                sSQL = "UPDATE TBL_BOIINVOICEINFO SET "
                sSQL = sSQL & "VENDOR_NUMBER = '" & txtVendorNo.Text & "' "
                sSQL = sSQL & ", PO_NUMBER = '" & txtPONo.Text & "' "
                sSQL = sSQL & ", CAR_NUMBER = '" & txtCarNo.Text & "' "
                sSQL = sSQL & ", UNIT_PRICE = '" & txtUnitPrice.Text & "' "
                sSQL = sSQL & ", CURRENCY = '" & txtCurrency.Text & "' "
                sSQL = sSQL & ", TRADE_TERM = '" & txtTradeTerm.Text & "' "

                If fupRawUpload.HasFile Then
                    sSQL = sSQL & ", RAW_FILEPATH = '" & filePath & "' "
                End If

                sSQL = sSQL & " WHERE INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' "
                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            Else
                'Insert new
                sSQL = "INSERT INTO TBL_BOIINVOICEINFO (INVOICE_NUMBER, VENDOR_NUMBER, PO_NUMBER, CAR_NUMBER, UNIT_PRICE, CURRENCY, TRADE_TERM, RAW_FILEPATH) "
                sSQL = sSQL & "VALUES ("
                sSQL = sSQL & "'" & txtInvoiceNo.Text & "', "
                sSQL = sSQL & "'" & txtVendorNo.Text & "', "
                sSQL = sSQL & "'" & txtPONo.Text & "', "
                sSQL = sSQL & "'" & txtCarNo.Text & "', "
                sSQL = sSQL & "'" & txtUnitPrice.Text & "', "
                sSQL = sSQL & "'" & txtCurrency.Text & "', "
                sSQL = sSQL & "'" & txtTradeTerm.Text & "', "
                sSQL = sSQL & "'" & filePath & "' "
                sSQL = sSQL & ")"

                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            End If

            If fupRawUpload.HasFile Then

                'Save the physical file to the server
                fupRawUpload.SaveAs(filePath)

            End If

            Response.Redirect("~/Pages/Setup/InvoiceInfoList.aspx")

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

            sSQL = "DELETE FROM TBL_BOIINVOICEINFO WHERE INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' "

            Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)
            CloseConnection()

            'Delete the file from server
            If hidFullFilePath.Value.ToString() <> "" Then

                'Remove file
                Try
                    Dim fileToRemove = hidFullFilePath.Value
                    Dim fi As FileInfo = New FileInfo(fileToRemove)
                    fi.Delete()
                Catch ex As Exception
                    'Ignore
                End Try

            End If

            Response.Redirect("~/Pages/Setup/InvoiceInfoList.aspx")

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
    Private Sub GetData(invoiceNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIINVOICEINFO WHERE INVOICE_NUMBER = '" & invoiceNo & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtInvoiceNo.Text = dsResult.Tables(0).Rows(0)("INVOICE_NUMBER").ToString()
                txtInvoiceNo.ReadOnly = True
                txtPONo.Text = dsResult.Tables(0).Rows(0)("PO_NUMBER").ToString()
                txtVendorNo.Text = dsResult.Tables(0).Rows(0)("VENDOR_NUMBER").ToString()
                txtCarNo.Text = dsResult.Tables(0).Rows(0)("CAR_NUMBER").ToString()
                txtUnitPrice.Text = FormatX(dsResult.Tables(0).Rows(0)("UNIT_PRICE"))
                'txtUnitPrice.Text = dsResult.Tables(0).Rows(0)("UNIT_PRICE").ToString()
                txtCurrency.Text = dsResult.Tables(0).Rows(0)("CURRENCY").ToString()
                txtTradeTerm.Text = dsResult.Tables(0).Rows(0)("TRADE_TERM").ToString()

                If dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString() <> Nothing And dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString().Length > 0 Then

                    hlRawFilename.Visible = True

                    Dim physicalPath = dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString()
                    Dim fi As FileInfo = New FileInfo(physicalPath)

                    Dim applicationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/")
                    Dim url = physicalPath.Substring(applicationPath.Length).Replace("\\", "/").Insert(0, "~/")

                    hlRawFilename.Text = fi.Name
                    hlRawFilename.NavigateUrl = url
                    hidFullFilePath.Value = dsResult.Tables(0).Rows(0)("RAW_FILEPATH").ToString()

                End If

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