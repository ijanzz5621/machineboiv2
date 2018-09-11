Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class BOITransfer
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
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
            sSQL = sSQL & "WHERE rownum <= 50 And a.INVOICE_NUMBER = b.INVOICE_NO AND a.INVOICE_ITEM = b.INVOICE_NO_ITEM AND b.MOVEMENT_CODE = c.MOVEMENT_CODE AND b.EQUIPMENT_ID = d.EQUIPMENT_ID AND b.SHOW_BOI_FLAG = 'Y' "

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
    Public Shared Function TransferBOI(dataList As String) As Object

        Try

            Dim test = dataList

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

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