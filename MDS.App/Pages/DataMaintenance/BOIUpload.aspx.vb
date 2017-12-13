
Imports System.Data.OleDb
'Imports System.Data.OracleClient
Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client
Imports ClosedXML.Excel
Imports System.IO
Imports System.Globalization

Public Class BOIUpload
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GetBOINumber()
            GetBOIGoodTypes()

        End If

    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click

        Dim dt As New DataTable()

        lblError.Text = ""
        pnlError.Visible = False

        'Use closedxml to read the excel content
        If fupBOIUpload.HasFile Then

            Using workbook As New XLWorkbook(fupBOIUpload.PostedFile.InputStream)

                Dim workSheet As IXLWorksheet = workbook.Worksheet(1)

                Dim firstRow As Boolean = True
                For Each row As IXLRow In workSheet.Rows

                    If row.Cell(1).Value.Trim() <> "" Then

                        If firstRow Then

                            'Create columns
                            For Each cell As IXLCell In row.Cells

                                dt.Columns.Add(cell.Value.ToString())

                            Next
                            firstRow = False

                        Else

                            'Add row to datatable
                            dt.Rows.Add()
                            Dim i As Int32 = 0
                            For Each cell As IXLCell In row.Cells
                                dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString
                                i = i + 1
                            Next

                        End If

                    End If

                Next


            End Using

            pnlResult.Visible = True
            'BOIUpload()
            BindTable(dt)
            SaveUploadFile(fupBOIUpload)

            Dim uploadResult As String = CheckDuplicates(dt)
            If uploadResult <> "" Then

                pnlError.Visible = True
                lblError.Text = uploadResult

            End If

        End If

    End Sub

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

#Region "Functions"

    Private Sub SaveUploadFile(fileUpload As FileUpload)

        Try
            'Rename filename before save
            Dim FileName As String = Path.GetFileName(fileUpload.FileName)
            FileName = FileName.Replace(".xlsx", "_" & System.DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx")

            Dim filePath As String = Server.MapPath("\Files\BOI\") + FileName
            fileUpload.SaveAs(filePath)

        Catch ex As Exception



        End Try

    End Sub

    Private Sub GetBOIGoodTypes()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT GOOD_TYPE_CODE, GOOD_TYPE_DESC, REQUIRE_INSPECTION FROM TBL_BOIGOODTYPESCODE"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlGoodTypes.DataSource = dsResult.Tables(0)
                ddlGoodTypes.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetBOINumber()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT BOI_NUMBER FROM TBL_BOINUMBER ORDER BY BOI_NUMBER"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlBOINo.DataSource = dsResult.Tables(0)
                ddlBOINo.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub BOIUpload()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIGOODTYPESCODE"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Dim testData = dsResult.Tables(0).Rows(0)("")
                gvResult.DataSource = dsResult
                gvResult.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub BindTable(dt As DataTable)

        gvResult.DataSource = dt
        ViewState("mydatasource") = dt
        gvResult.DataBind()

    End Sub

    Private Function CheckDuplicates(dt As DataTable) As String

        Dim listItem As String = ""
        Dim countRow As Int16

        Try

            If dt.Rows.Count > 0 Then

                countRow = 1

                For Each row As DataRow In dt.Rows

                    Dim invNo As String = row("InvoiceNo").ToString
                    Dim invItem As String = row("InvoiceItem").ToString

                    If CheckExistInvoice(invNo, invItem) Then
                        listItem = listItem & "(Row No: " & countRow & ", Invoice No: " & invNo & ", Invoice Item: " & invItem & " already exist. "
                    End If

                    countRow = countRow + 1
                Next

            End If

        Catch ex As Exception

            'Ignore

        End Try



        Return listItem

    End Function

    Private Function CheckExistInvoice(invNo As String, invItem As String) As Boolean

        Dim isExist As Boolean = False

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIINFO WHERE INVOICE_NUMBER = '" & invNo & "' AND INVOICE_ITEM = '" & invItem & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)
            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                isExist = True

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return isExist

    End Function

#End Region

    Protected Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click

        lblError.Text = ""
        pnlError.Visible = False

        'Save the datatable into oracle database
        Try

            Dim dtConfirm As DataTable = ViewState("mydatasource")
            Dim sSQL As String = ""

            For Each row As DataRow In dtConfirm.Rows

                Dim resultDate As DateTime

                'minus the year with 543 if the year is more than 2500
                Try
                    'Dim formats() As String = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                    '             "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                    '             "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                    '             "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                    '             "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                    '             "dd/MM/yyyy hh:mm:ss", "dd/M/yyyy hh:mm:ss"
                    '}

                    ''DateTime.Parse(row.Item(27)).ToString("yyyy-MM-dd")
                    'DateTime.TryParseExact(row.Item(27), formats, New CultureInfo("en-US"), DateTimeStyles.None, resultDate)
                    'If resultDate = Nothing Then
                    '    'Not a valid date
                    '    'use current date
                    '    resultDate = DateTime.Now
                    'Else
                    '    'Get the year and check if the year > 2500
                    '    If resultDate.Year > 2500 Then
                    '        resultDate = resultDate.AddYears(-543)
                    '    End If
                    'End If

                    Dim Dt As DateTime = DateTime.Now
                    Dim mFomatter As IFormatProvider = New System.Globalization.CultureInfo("en-GB")
                    Dt = DateTime.Parse(row.Item(27), mFomatter)

                    If Dt = Nothing Then
                        'Not a valid date
                        'use current date
                        resultDate = DateTime.Now
                    Else
                        ''Get the year and check if the year > 2500
                        'If resultDate.Year > 2500 Then
                        '    resultDate = resultDate.AddYears(-543)
                        'End If
                        Dim cal As New ThaiBuddhistCalendar()
                        resultDate = cal.ToDateTime(Dt.Year, Dt.Month, Dt.Day, Dt.Hour, Dt.Minute, Dt.Second, Dt.Millisecond).ToString("yyyy-MM-dd HH:mm:ss")
                    End If

                Catch ex As Exception
                    Dim err = ex.Message
                    resultDate = DateTime.Now   ' Just assign to today date
                End Try

                GetAppConfig()
                OpenConnection()

                sSQL = "INSERT INTO TBL_BOIINFO "
                sSQL = sSQL & "("
                sSQL = sSQL & "ITEM_NUMBER"
                sSQL = sSQL & ", XMLTYPE"
                sSQL = sSQL & ", DOCUMENT_NUMBER"
                sSQL = sSQL & ", DOCUMENT_DATE"
                sSQL = sSQL & ", BOI_TAX_REFERENCE"
                sSQL = sSQL & ", TAX_REFERENCE"
                sSQL = sSQL & ", BRANCH"
                sSQL = sSQL & ", TOTAL_ITEM"
                sSQL = sSQL & ", USER_ID"
                sSQL = sSQL & ", DECLARATION_LINE_NUMBER"
                sSQL = sSQL & ", IMPORT_DECLARATION_NUMBER"
                sSQL = sSQL & ", INVOICE_NUMBER"
                sSQL = sSQL & ", INVOICE_DATE"
                sSQL = sSQL & ", INVOICE_ITEM"
                sSQL = sSQL & ", DESCRIPTION"
                sSQL = sSQL & ", GOOD_TYPE"
                sSQL = sSQL & ", EXEMPT_TYPE"
                sSQL = sSQL & ", PRIVILEGE_TYPE"
                sSQL = sSQL & ", PRIVILEGE_CONDITION"
                sSQL = sSQL & ", CONDITION_DUTY_RATE"
                sSQL = sSQL & ", PERCENT_EXEMPT_DUTY"
                sSQL = sSQL & ", PERCENT_EXEMPT_VAT"
                sSQL = sSQL & ", UNIT_CODE"
                sSQL = sSQL & ", QUANTITY"
                sSQL = sSQL & ", PRIVILEGE_VALID_FROM"
                sSQL = sSQL & ", PRIVILEGE_VALID_UNTIL"
                sSQL = sSQL & ", REFERENCE_DOCUMENT_NUMBER"
                sSQL = sSQL & ", CREATE_DATE"
                sSQL = sSQL & ", BOI_NUMBER"
                sSQL = sSQL & ", GOOD_TYPE_CODE"
                sSQL = sSQL & ", INSPECTION_CODE"
                sSQL = sSQL & ", CER_NUMBER"
                sSQL = sSQL & ", CER_ITEM"
                sSQL = sSQL & ", CER_REMARK"
                sSQL = sSQL & ")"

                sSQL = sSQL & " VALUES "

                sSQL = sSQL & "('" & row.Item(0) & "' "
                sSQL = sSQL & ", '" & row.Item(1) & "' "
                sSQL = sSQL & ", '" & row.Item(2) & "' "
                sSQL = sSQL & ", TO_DATE('" & DateTime.Parse(row.Item(3)).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
                sSQL = sSQL & ", '" & row.Item(4) & "' "
                sSQL = sSQL & ", '" & row.Item(5) & "' "
                sSQL = sSQL & ", '" & row.Item(6) & "' "
                sSQL = sSQL & ", '" & row.Item(7) & "' "
                sSQL = sSQL & ", '" & row.Item(8) & "' "
                sSQL = sSQL & ", '" & row.Item(9) & "' "
                sSQL = sSQL & ", '" & row.Item(10) & "' "
                sSQL = sSQL & ", '" & row.Item(11) & "' "
                sSQL = sSQL & ", TO_DATE('" & DateTime.Parse(row.Item(12)).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
                sSQL = sSQL & ", '" & row.Item(13) & "' "
                sSQL = sSQL & ", '" & row.Item(14) & "' "
                sSQL = sSQL & ", '" & row.Item(15) & "' "
                sSQL = sSQL & ", '" & row.Item(16) & "' "
                sSQL = sSQL & ", '" & row.Item(17) & "' "
                sSQL = sSQL & ", '" & row.Item(18) & "' "
                sSQL = sSQL & ", '" & row.Item(19) & "' "
                sSQL = sSQL & ", '" & row.Item(20) & "' "
                sSQL = sSQL & ", '" & row.Item(21) & "' "
                sSQL = sSQL & ", '" & row.Item(22) & "' "
                sSQL = sSQL & ", '" & row.Item(23) & "' "

                sSQL = sSQL & ", TO_DATE('" & DateTime.Parse(row.Item(24)).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
                sSQL = sSQL & ", TO_DATE('" & DateTime.Parse(row.Item(25)).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
                sSQL = sSQL & ", '" & row.Item(26) & "' "
                sSQL = sSQL & ", TO_DATE('" & resultDate.ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "

                sSQL = sSQL & ", '" & ddlBOINo.SelectedValue & "'"
                sSQL = sSQL & ", '" & ddlGoodTypes.SelectedValue.ToString & "'"
                sSQL = sSQL & ", '01'"
                sSQL = sSQL & ", ''"
                sSQL = sSQL & ", ''"
                sSQL = sSQL & ", '') "

                Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)
                CloseConnection()

            Next

            Response.Redirect("~/Pages/DataMaintenance/BOIList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub
End Class