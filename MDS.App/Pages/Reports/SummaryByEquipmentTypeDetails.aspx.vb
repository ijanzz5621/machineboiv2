Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class SummaryByEquipmentTypeDetails
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
    Private Shared cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Web Methods"

    <WebMethod>
    Public Shared Function GetListing(equipmentType As String, boiNumber As String) As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            'Dim sSQL = "select '' as GOOD_TYPE_DESC , a.GOOD_TYPE , b.EQUIPMENT_BRAND , b.EQUIPMENT_MODEL , b.EQUIPMENT_ID , b.SERIAL_NO , '' as CAR_NO , '' as PO_NO , b.SERIAL_NO "
            'sSQL = sSQL & ", '' as UNIT_PRICE , '' as CURRENCY , '' as TRADE_TERM , b.ASSET_TAG , '' as VENDOR "
            'sSQL = sSQL & "from TBL_BOIINFO a, V_EQUIPMENT b where a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item "
            'sSQL = sSQL & "and b.EQUIPMENT_TYPE = '" & equipmentType & "' and a.BOI_NUMBER = '" & boiNumber & "'"
            Dim sSQL = "select d.GOOD_TYPE_DESC as GOOD_TYPE_DESC , a.GOOD_TYPE , b.EQUIPMENT_BRAND , b.EQUIPMENT_MODEL , b.EQUIPMENT_ID , b.SERIAL_NO "
            sSQL = sSQL & ", c.CAR_NUMBER as CAR_NO , c.PO_NUMBER as PO_NO , b.SERIAL_NO , c.UNIT_PRICE as UNIT_PRICE , c.CURRENCY as CURRENCY , c.TRADE_TERM as TRADE_TERM , b.ASSET_TAG "
            sSQL = sSQL & ", c.VENDOR_NUMBER as VENDOR "
            sSQL = sSQL & "from TBL_BOIINFO a, V_EQUIPMENT b, TBL_BOIINVOICEINFO c, TBL_BOIGOODTYPESCODE d "
            sSQL = sSQL & "where a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item and a.INVOICE_NUMBER = c.INVOICE_NUMBER and a.GOOD_TYPE_CODE = d.GOOD_TYPE_CODE "
            sSQL = sSQL & "and b.EQUIPMENT_TYPE = '" & equipmentType & "' and a.BOI_NUMBER = '" & boiNumber & "'"

            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return JsonConvert.SerializeObject(dsResult.Tables(0))

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