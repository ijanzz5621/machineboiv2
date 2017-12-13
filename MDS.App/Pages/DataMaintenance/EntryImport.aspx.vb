Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Public Class EntryImport
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            If Request.QueryString("IMPORTNO") <> Nothing And Request.QueryString("JOBNO") <> Nothing Then

                'Load Data
                'Get data from database
                GetData(Request.QueryString("IMPORTNO"), Request.QueryString("JOBNO"))
                GetTotalAmount(Request.QueryString("IMPORTNO"))

            End If

        End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            If DataCheckingBeforeSave() Then

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
                    filePath = Server.MapPath("\Files\Entry\") + filename

                End If

                'Check from database if the record already exist
                'If yes, just update and delete the old file
                sSQL = "SELECT * FROM TBL_BOIIMPORTENTRY WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND JOB_NUMBER = '" & txtJobNo.Text & "'"
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

                    'Update
                    sSQL = "UPDATE TBL_BOIIMPORTENTRY SET "
                    sSQL = sSQL & " IMPORT_DATE = TO_DATE('" & DateTime.Parse(txtImportDate.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
                    sSQL = sSQL & ", SHIP_FROM = '" & txtShipFrom.Text & "' "
                    ' sSQL = sSQL & ", ADDRESS = '" & txtAddress.Text & "' "
                    sSQL = sSQL & ", ADDRESS = '" & ddlAddress.SelectedValue & "' "
                    sSQL = sSQL & ", SUBMIT_DATE = TO_DATE('" & DateTime.Parse(txtSubmitDate.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "

                    If fupRawUpload.HasFile Then
                        sSQL = sSQL & ", RAW_FILEPATH = '" & filePath & "' "
                    End If

                    sSQL = sSQL & " WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND JOB_NUMBER = '" & txtJobNo.Text & "'"
                    Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

                Else
                    'Insert new
                    sSQL = "INSERT INTO TBL_BOIIMPORTENTRY (IMPORT_ENTRY_NUMBER, IMPORT_DATE, SHIP_FROM, ADDRESS, JOB_NUMBER, SUBMIT_DATE, RAW_FILEPATH) "
                    sSQL = sSQL & "VALUES ("
                    sSQL = sSQL & "'" & txtImportEntryNo.Text & "', "
                    sSQL = sSQL & "TO_DATE('" & DateTime.Parse(txtImportDate.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd'), "
                    sSQL = sSQL & "'" & txtShipFrom.Text & "', "
                    sSQL = sSQL & "'" & ddlAddress.SelectedValue & "',"
                    'sSQL = sSQL & " '" & txtAddress.Text & "', "
                    sSQL = sSQL & "'" & txtJobNo.Text & "', "
                    sSQL = sSQL & "TO_DATE('" & DateTime.Parse(txtSubmitDate.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd'), "
                    sSQL = sSQL & "'" & filePath & "' "
                    sSQL = sSQL & ")"

                    Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

                End If

                If fupRawUpload.HasFile Then

                    'Save the physical file to the server
                    fupRawUpload.SaveAs(filePath)

                End If

                Response.Redirect("~/Pages/DataMaintenance/EntryImportList.aspx")

            End If

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

    Private Function DataCheckingBeforeSave() As Boolean

        pnlError.Visible = False
        lblError.Text = ""
        Dim result As Boolean = True
        Dim resultDate As DateTime

        'Import Date
        DateTime.TryParse(txtImportDate.Text, resultDate)
        If resultDate = Nothing Then
            pnlError.Visible = True
            lblError.Text = lblError.Text & "Import Date is not in Date Format. "
            result = False
        End If

        'Submit Date
        DateTime.TryParse(txtSubmitDate.Text, resultDate)
        If resultDate = Nothing Then
            pnlError.Visible = True
            lblError.Text = lblError.Text & "Submit Date is not in Date Format. "
            result = False
        End If

        Return result

    End Function

    Private Sub GetData(importNo As String, jobNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIIMPORTENTRY WHERE IMPORT_ENTRY_NUMBER = '" & importNo & "' AND JOB_NUMBER = '" & jobNo & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtImportEntryNo.Text = dsResult.Tables(0).Rows(0)("IMPORT_ENTRY_NUMBER").ToString()
                txtImportEntryNo.ReadOnly = True
                txtImportDate.Text = DateTime.Parse(dsResult.Tables(0).Rows(0)("IMPORT_DATE").ToString()).ToString("yyyy-MM-dd")
                txtShipFrom.Text = dsResult.Tables(0).Rows(0)("SHIP_FROM").ToString()
                'txtAddress.Text = dsResult.Tables(0).Rows(0)("ADDRESS").ToString()

                For i As Integer = 0 To ddlAddress.Items.Count - 1
                    If ddlAddress.Items(i).Value = dsResult.Tables(0).Rows(0)("ADDRESS").ToString() Then
                        ddlAddress.Items(i).Selected = True
                    Else
                        ddlAddress.Items(i).Selected = False
                    End If
                Next

                'ddlAddress.SelectedValue = dsResult.Tables(0).Rows(0)("ADDRESS").ToString()
                'txtTotalAmount.Text = dsResult.Tables(0).Rows(0)("IMPORT_ENTRY_NUMBER").ToString()
                txtJobNo.Text = dsResult.Tables(0).Rows(0)("JOB_NUMBER").ToString()
                txtJobNo.ReadOnly = True
                txtSubmitDate.Text = DateTime.Parse(dsResult.Tables(0).Rows(0)("SUBMIT_DATE").ToString()).ToString("yyyy-MM-dd")

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

    Private Function FormatX(dblValue As Object) As String
        Return String.Format("{0:#,##0.00}", dblValue)
    End Function

    Private Sub GetTotalAmount(importEntryNo As String)

        Try

            GetAppConfig()
            OpenConnection()

            'Dim sSQL = "select sum(tax_value + vat_value) as TOTAL_AMOUNT from tbl_boiimportsubentry where import_entry_number = '" & importEntryNo & "' "
            Dim sSQL = "select sum(amount) as TOTAL_AMOUNT from tbl_boiimportsubentry where import_entry_number = '" & importEntryNo & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then
                txtTotalAmount.Text = FormatX(dsResult.Tables(0).Rows(0)("TOTAL_AMOUNT"))
            End If

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Try

            Dim sSQL As String = ""

            GetAppConfig()
            OpenConnection()

            sSQL = "DELETE FROM TBL_BOIIMPORTENTRY WHERE IMPORT_ENTRY_NUMBER = '" & txtImportEntryNo.Text & "' AND JOB_NUMBER = '" & txtJobNo.Text & "'"

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

            Response.Redirect("~/Pages/DataMaintenance/EntryImportList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

End Class