Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class BOITransfer
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
    Private Shared trxOra As OracleTransaction = Nothing
    Private Shared cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Web Methods"

    <WebMethod>
    Public Shared Function GetListing(boiNumber As String, equipmentType As String)

        Dim dsResult As DataSet = New DataSet

        Try

            GetAppConfig()
            OpenConnection()

            ' Call the query to get list of BOI Transfer
            Dim sSQL = "SELECT d.EQUIPMENT_TYPE, b.EQUIPMENT_ID, a.BOI_NUMBER, a.INVOICE_NUMBER, a.INVOICE_ITEM, a.XMLTYPE, a.DESCRIPTION, b.MOVEMENT_DATE, c.MOVEMENT_DESCRIPTION, c.MOVEMENT_CODE_VALUE
                FROM TBL_BOIINFO a, TBL_EQUIPMENTMOVERECORD b, TBL_EQUIPMENTMOVEMENTTYPE c, V_EQUIPMENT d "
            sSQL = sSQL & "WHERE rownum <= 500 And a.INVOICE_NUMBER = b.INVOICE_NO AND a.INVOICE_ITEM = b.INVOICE_NO_ITEM AND b.MOVEMENT_CODE = c.MOVEMENT_CODE AND b.EQUIPMENT_ID = d.EQUIPMENT_ID AND b.SHOW_BOI_FLAG = 'Y' "

            'sSQL = sSQL & "AND a.BOI_NUMBER = '" & boiNumber & "' AND d.EQUIPMENT_TYPE = '" & equipmentType & "' "
            If boiNumber <> "" Then
                sSQL = sSQL & "and a.BOI_NUMBER IN ('" & boiNumber.Replace(",", "','") & "') "
            End If

            If equipmentType <> "" And equipmentType <> "null" Then
                sSQL = sSQL & "and d.EQUIPMENT_TYPE IN ('" & equipmentType.Replace(",", "','") & "') "
            End If

            sSQL = sSQL & "ORDER BY b.EQUIPMENT_ID, b.MOVEMENT_DATE"
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try


        Return JsonConvert.SerializeObject(dsResult.Tables(0))

    End Function

    <WebMethod>
    Public Shared Function GetFilterEquipmentType() As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct EQUIPMENT_TYPE from V_EQUIPMENT order by EQUIPMENT_TYPE"
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return JsonConvert.SerializeObject(dsResult.Tables(0))

    End Function

    <WebMethod>
    Public Shared Function TransferBOI(dataList As String, boiDate As String, boiNumber As String) As Object

        Dim myConnection As New OracleConnection(cnnOraString)
        myConnection.Open()

        Dim myCommand As OracleCommand = myConnection.CreateCommand()

        Dim myTrans As OracleTransaction = myConnection.BeginTransaction()

        myCommand.Transaction = myTrans

        Dim myDataAdapter As OracleDataAdapter = Nothing

        Try

            'Dim test = dataList
            Dim newInvoice = Nothing
            Dim sql As String = ""

            ' 1. Loop the selected data
            Dim data() = dataList.Split(",")
            For Each selData As String In data

                newInvoice = ""

                Dim machine = selData.Split("_")(0)
                Dim boi = selData.Split("_")(1)
                Dim invoiceNo = selData.Split("_")(2)
                Dim invoiceItem = selData.Split("_")(3)

                newInvoice = invoiceNo & "_" & machine & "_" & "F" & boi & "T" & boiNumber

                ' 1. Get the data FROM TBL_BOIINFO and save with the new INVOICE NUMBER into TBL_BOIINFO
                sql = "insert into TBL_BOIINFO (
                    ITEM_NUMBER, XMLTYPE, DOCUMENT_NUMBER, DOCUMENT_DATE, BOI_TAX_REFERENCE, TAX_REFERENCE, BRANCH, TOTAL_ITEM, USER_ID, 
                    DECLARATION_LINE_NUMBER, IMPORT_DECLARATION_NUMBER, INVOICE_NUMBER, INVOICE_DATE, INVOICE_ITEM, DESCRIPTION, GOOD_TYPE,
                    EXEMPT_TYPE, PRIVILEGE_TYPE, PRIVILEGE_CONDITION, CONDITION_DUTY_RATE, PERCENT_EXEMPT_DUTY, PERCENT_EXEMPT_VAT, UNIT_CODE,
                    QUANTITY, PRIVILEGE_VALID_FROM, PRIVILEGE_VALID_UNTIL, REFERENCE_DOCUMENT_NUMBER, CREATE_DATE, BOI_NUMBER, GOOD_TYPE_CODE,
                    INSPECTION_CODE, CER_NUMBER, CER_ITEM, CER_REMARK
                    )
                    select ITEM_NUMBER, XMLTYPE, DOCUMENT_NUMBER, DOCUMENT_DATE, BOI_TAX_REFERENCE, TAX_REFERENCE, BRANCH, TOTAL_ITEM, USER_ID, 
                    DECLARATION_LINE_NUMBER, IMPORT_DECLARATION_NUMBER, '" & newInvoice & "', INVOICE_DATE, INVOICE_ITEM, DESCRIPTION, GOOD_TYPE,
                    EXEMPT_TYPE, PRIVILEGE_TYPE, PRIVILEGE_CONDITION, CONDITION_DUTY_RATE, PERCENT_EXEMPT_DUTY, PERCENT_EXEMPT_VAT, UNIT_CODE,
                    QUANTITY, PRIVILEGE_VALID_FROM, PRIVILEGE_VALID_UNTIL, REFERENCE_DOCUMENT_NUMBER, CREATE_DATE, BOI_NUMBER, GOOD_TYPE_CODE,
                    INSPECTION_CODE, CER_NUMBER, CER_ITEM, CER_REMARK
                    from TBL_BOIINFO where INVOICE_NUMBER = '" & invoiceNo & "' and INVOICE_ITEM = '" & invoiceItem & "'"
                myCommand.CommandText = sql
                myCommand.ExecuteNonQuery()
                ' 2. Get the IMPORT_ENTRY_NUMBER from TBL_BOIIMPORTSUBENTRY before save into TBL_BOIIMPORTENTRY (save to TBL_BOIIMPORTSUBENTRY)
                sql = "select IMPORT_ENTRY_NUMBER from TBL_BOIIMPORTSUBENTRY where INVOICE_NUMBER = '" & invoiceNo & "' and INVOICE_ITEM = '" & invoiceItem & "'"
                myCommand.CommandText = sql
                Dim ds As DataSet = New DataSet()
                myDataAdapter = New OracleDataAdapter(myCommand)
                myDataAdapter.Fill(ds)

                Dim importEntryNumber As String = ""
                Dim newImportEntryNumber As String = ""

                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then

                    importEntryNumber = ds.Tables(0).Rows(0)("IMPORT_ENTRY_NUMBER")

                End If
                ' 3. Save into TBL_BOIIMPORTENTRY
                If importEntryNumber <> "" Then

                    newImportEntryNumber = importEntryNumber & "_" + machine & "F" & boi & "T" & boiNumber

                    'TBL_BOIIMPORTENTRY
                    sql = "insert into TBL_BOIIMPORTENTRY (IMPORT_ENTRY_NUMBER, IMPORT_DATE, SHIP_FROM, ADDRESS, JOB_NUMBER, SUBMIT_DATE, RAW_FILEPATH)
                        select '" & newImportEntryNumber & "', IMPORT_DATE, SHIP_FROM, ADDRESS, JOB_NUMBER, SUBMIT_DATE, RAW_FILEPATH
                        from TBL_BOIIMPORTENTRY WHERE IMPORT_ENTRY_NUMBER = '" & importEntryNumber & "'"
                    myCommand.CommandText = sql
                    myCommand.ExecuteNonQuery()

                    'TBL_BOIIMPORTSUBENTRY
                    sql = "INSERT INTO TBL_BOIIMPORTSUBENTRY (IMPORT_ENTRY_NUMBER, ITEM_NUMBER, ORIGIN_CONTRY, AMOUNT, TAX_VALUE, VAT_VALUE, TAX_RAT_PERCENT, H_S_CODE, INVOICE_NUMBER, INVOICE_ITEM)
                        select '" & newImportEntryNumber & "', ITEM_NUMBER, ORIGIN_CONTRY, AMOUNT, TAX_VALUE, VAT_VALUE, TAX_RAT_PERCENT, H_S_CODE, '" & newInvoice & "', INVOICE_ITEM
                        from TBL_BOIIMPORTSUBENTRY where INVOICE_NUMBER = '" & invoiceNo & "' and INVOICE_ITEM = '" & invoiceItem & "'"
                    myCommand.CommandText = sql
                    myCommand.ExecuteNonQuery()

                End If

                'TBL_BOIINVOICEINFO
                sql = "insert into TBL_BOIINVOICEINFO(INVOICE_NUMBER, VENDOR_NUMBER, PO_NUMBER, CAR_NUMBER, UNIT_PRICE, CURRENCY, TRADE_TERM, RAW_FILEPATH)
                    select '" & newInvoice & "', VENDOR_NUMBER, PO_NUMBER, CAR_NUMBER, UNIT_PRICE, CURRENCY, TRADE_TERM, RAW_FILEPATH
                    from TBL_BOIINVOICEINFO where INVOICE_NUMBER = '" & invoiceNo & "'"
                myCommand.CommandText = sql
                myCommand.ExecuteNonQuery()

                Dim userLogin = HttpContext.Current.Session("USER_NAME")
                ' 4. Add 2 new records into TBL_EQUIPMENTMOVERECORD
                sql = "insert into TBL_EQUIPMENTMOVERECORD(RECORD_ID, EQUIPMENT_ID, INVOICE_NO, INVOICE_NO_ITEM, MOVEMENT_DATE, MOVEMENT_CODE, ORIGIN_CODE, DESTINATION_CODE,
                    USER_COMMENT, WHO_UPDATE, TIMESTAMP, SHOW_BOI_FLAG)
                    VALUES (SYS_GUID(), '" & machine & "', '" & invoiceNo & "', '" & invoiceItem & "', sysdate, '007', '001', '001', 'Terminate BOI', '" & userLogin & "', CURRENT_TIMESTAMP, 'Y')"
                myCommand.CommandText = sql
                myCommand.ExecuteNonQuery()
                sql = "insert into TBL_EQUIPMENTMOVERECORD(RECORD_ID, EQUIPMENT_ID, INVOICE_NO, INVOICE_NO_ITEM, MOVEMENT_DATE, MOVEMENT_CODE, ORIGIN_CODE, DESTINATION_CODE,
                    USER_COMMENT, WHO_UPDATE, TIMESTAMP, SHOW_BOI_FLAG)
                    VALUES (SYS_GUID(), '" & machine & "', '" & newInvoice & "', '" & invoiceItem & "', sysdate, '004', '001', '001', 'Transfer BOI', '" & userLogin & "', CURRENT_TIMESTAMP, 'Y')"
                myCommand.CommandText = sql
                myCommand.ExecuteNonQuery()
            Next

            myTrans.Commit()
            'myTrans.Rollback()

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            myTrans.Rollback()


        Finally

            myConnection.Close()
            'CloseConnection()

        End Try

        Return JsonConvert.SerializeObject("SUCCESS")

    End Function

#End Region

#Region "get App.Config"
    Private Shared Sub GetAppConfig()
        cnnOraString = ConfigurationManager.ConnectionStrings("ORA_EQBConnString").ConnectionString
    End Sub
#End Region

#Region "Open Connection"
    Private Shared Sub OpenConnection()
        oOra.OpenOraConnection(cnnOra, cnnOraString)
    End Sub

    Private Shared Sub CloseConnection()
        oOra.CloseOraConnection(cnnOra)
    End Sub

#End Region

End Class