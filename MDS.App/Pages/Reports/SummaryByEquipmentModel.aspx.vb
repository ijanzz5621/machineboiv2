Imports System.Web.Services
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client

Public Class SummaryByEquipmentModel
    Inherits System.Web.UI.Page

    'Oracle
    Private Shared oOra As New Oracle
    Private Shared cnnOra As OracleConnection = Nothing
    Private Shared cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Web methods"

    <WebMethod>
    Public Shared Function GetListing(boiNumber As String, equipmentModel As String, statusCode As String) As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct BOI_NUMBER FROM TBL_BOIINFO order by BOI_NUMBER"
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                sSQL = "select EQUIPMENT_MODEL, AREA "

                If boiNumber <> "" Then

                    Dim strBOI As String() = boiNumber.Split(",")
                    For Each boi As String In strBOI
                        sSQL = sSQL & ", SUM(""" & boi & """) AS """ & boi & """ "
                    Next
                Else

                    For Each row As DataRow In dsResult.Tables(0).Rows
                        sSQL = sSQL & ", SUM(""" & row("BOI_NUMBER").ToString & """) AS """ & row("BOI_NUMBER").ToString & """ "
                    Next

                End If

                sSQL = sSQL & "FROM "
                sSQL = sSQL & "( "
                sSQL = sSQL & "SELECT b.EQUIPMENT_MODEL, "
                sSQL = sSQL & "CASE b.LINE "
                sSQL = sSQL & "WHEN 'AIPD' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'MC' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'MP' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'FLEX CELL' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'SMSC' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'STRIP MC' THEN 'STRIP' "
                sSQL = sSQL & "WHEN 'STRIP MP' THEN 'STRIP' ELSE NVL(b.LINE,'?') END AREA "

                If boiNumber <> "" Then

                    Dim strBOI As String() = boiNumber.Split(",")
                    For Each boi As String In strBOI
                        sSQL = sSQL & ", count( decode( a.BOI_NUMBER, '" & boi & "', 1 ) ) """ & boi & """ "
                    Next
                Else

                    For Each row As DataRow In dsResult.Tables(0).Rows
                        sSQL = sSQL & ", count( decode( a.BOI_NUMBER, '" & row("BOI_NUMBER").ToString & "', 1 ) ) """ & row("BOI_NUMBER").ToString & """ "
                    Next

                End If

                sSQL = sSQL & "FROM TBL_BOIINFO a, V_EQUIPMENT b "
                sSQL = sSQL & "WHERE a.INVOICE_NUMBER = b.INVOICE_NO AND a.INVOICE_ITEM = b.INVOICE_NO_ITEM "

                If equipmentModel <> "" Then
                    sSQL = sSQL & "and b.EQUIPMENT_MODEL = '" & equipmentModel & "' "
                End If

                If statusCode <> "" Then
                    sSQL = sSQL & "and b.STATUS_CODE = '" & statusCode & "' "
                End If

                sSQL = sSQL & "GROUP BY a.BOI_NUMBER, b.EQUIPMENT_MODEL, "
                sSQL = sSQL & "CASE b.LINE WHEN 'AIPD' THEN 'SINGULATE' WHEN 'MC' THEN 'SINGULATE' WHEN 'MP' THEN 'SINGULATE' "
                sSQL = sSQL & "WHEN 'FLEX CELL' THEN 'SINGULATE' WHEN 'SMSC' THEN 'SINGULATE' WHEN 'STRIP MC' THEN 'STRIP' "
                sSQL = sSQL & "WHEN 'STRIP MP' THEN 'STRIP' ELSE NVL(b.LINE,'?') END "
                sSQL = sSQL & "ORDER BY 2,1) Q1 "
                sSQL = sSQL & "GROUP BY Q1.EQUIPMENT_MODEL, Q1.AREA "
                sSQL = sSQL & "ORDER BY Q1.EQUIPMENT_MODEL, Q1.AREA "

                'sSQL = sSQL & "from TBL_BOIINFO a, V_EQUIPMENT b "
                'sSQL = sSQL & "where a.invoice_number = b.invoice_no and a.invoice_item = b.invoice_no_item "

                'If equipmentType <> "" Then
                '    sSQL = sSQL & "and b.EQUIPMENT_TYPE = '" & equipmentType & "' "
                'End If

                'If statusCode <> "" Then
                '    sSQL = sSQL & "and b.STATUS_CODE = '" & statusCode & "' "
                'End If

                'sSQL = sSQL & "group by b.EQUIPMENT_TYPE "
                'sSQL = sSQL & "order by b.EQUIPMENT_TYPE"

                dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

            End If

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return JsonConvert.SerializeObject(dsResult.Tables(0))

    End Function

    <WebMethod>
    Public Shared Function GetFilterEquipmentModel() As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct EQUIPMENT_MODEL from V_EQUIPMENT order by EQUIPMENT_MODEL"
            dsResult = oOra.OraExecuteQuery(sSQL, cnnOra)

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return JsonConvert.SerializeObject(dsResult.Tables(0))

    End Function

    <WebMethod>
    Public Shared Function GetFilterStatusCode() As Object

        Dim dsResult As DataSet = New DataSet
        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select distinct STATUS_CODE from V_EQUIPMENT order by STATUS_CODE"
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