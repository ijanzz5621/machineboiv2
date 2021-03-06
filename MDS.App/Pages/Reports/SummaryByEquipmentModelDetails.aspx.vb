﻿Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class SummaryByEquipmentModelDetails
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
    Private Shared cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Web Methods"

    <WebMethod>
    Public Shared Function GetListing(equipmentModel As String, boiNumber As String, status As String) As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            'Dim sSQL = "select d.GOOD_TYPE_DESC as GOOD_TYPE_DESC , a.GOOD_TYPE , b.EQUIPMENT_BRAND , b.EQUIPMENT_MODEL , b.EQUIPMENT_ID, a.INVOICE_NUMBER , b.SERIAL_NO "
            'sSQL = sSQL & ", c.CAR_NUMBER as CAR_NO , c.PO_NUMBER as PO_NO , b.SERIAL_NO , c.UNIT_PRICE as UNIT_PRICE , c.CURRENCY as CURRENCY , c.TRADE_TERM as TRADE_TERM , b.ASSET_TAG "
            'sSQL = sSQL & ", c.VENDOR_NUMBER as VENDOR "
            'sSQL = sSQL & "from TBL_BOIINFO a, V_EQUIPMENT b, TBL_BOIINVOICEINFO c, TBL_BOIGOODTYPESCODE d "
            'sSQL = sSQL & "where a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item and a.INVOICE_NUMBER = c.INVOICE_NUMBER and a.GOOD_TYPE_CODE = d.GOOD_TYPE_CODE "
            'sSQL = sSQL & "and b.EQUIPMENT_MODEL = '" & equipmentModel & "' and a.BOI_NUMBER = '" & boiNumber & "'"
            Dim sSQL = "select d.GOOD_TYPE_DESC as GOOD_TYPE_DESC , a.GOOD_TYPE , b.EQUIPMENT_BRAND , b.EQUIPMENT_MODEL , b.EQUIPMENT_ID, a.INVOICE_NUMBER , "
            sSQL = sSQL & "b.SERIAL_NO , c.CAR_NUMBER as CAR_NO , c.PO_NUMBER as PO_NO , b.SERIAL_NO , c.UNIT_PRICE as UNIT_PRICE , c.CURRENCY as CURRENCY , "
            sSQL = sSQL & "c.TRADE_TERM as TRADE_TERM , b.ASSET_TAG , c.VENDOR_NUMBER as VENDOR "
            sSQL = sSQL & "from TBL_BOIINFO a "
            sSQL = sSQL & "left join V_EQUIPMENT b ON a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item "
            sSQL = sSQL & "left join TBL_BOIINVOICEINFO c ON a.INVOICE_NUMBER = c.INVOICE_NUMBER "
            sSQL = sSQL & "left join TBL_BOIGOODTYPESCODE d ON a.GOOD_TYPE_CODE = d.GOOD_TYPE_CODE "
            sSQL = sSQL & "where b.EQUIPMENT_MODEL = '" & equipmentModel & "' and a.BOI_NUMBER = '" & boiNumber & "' "

            If status IsNot Nothing And status <> "null" Then
                sSQL = sSQL & "and b.STATUS_CODE IN ('" & status.Replace(",", "','") & "') "
            End If

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