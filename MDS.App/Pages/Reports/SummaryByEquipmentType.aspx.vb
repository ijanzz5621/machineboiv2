﻿Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class SummaryByEquipmentType
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
    Private Shared cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Web methods"

    <WebMethod>
    Public Shared Function GetListing(boiNumber As String, equipmentType As String, statusCode As String) As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct BOI_NUMBER FROM TBL_BOIINFO order by BOI_NUMBER"
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                sSQL = "select b.EQUIPMENT_TYPE "

                For Each row As DataRow In dsResult.Tables(0).Rows

                    sSQL = sSQL & ", count( decode( a.BOI_NUMBER, '" & row("BOI_NUMBER").ToString & "', 1 ) ) """ & row("BOI_NUMBER").ToString & """ "

                Next

                sSQL = sSQL & "from TBL_BOIINFO a, V_EQUIPMENT b "
                sSQL = sSQL & "where a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item "
                sSQL = sSQL & "group by b.EQUIPMENT_TYPE "
                sSQL = sSQL & "order by b.EQUIPMENT_TYPE"

                dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

            End If

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