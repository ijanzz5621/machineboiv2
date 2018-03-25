'Imports System.Data.OracleClient
Imports Oracle.ManagedDataAccess.Client

Public Class Oracle
#Region "Database conneciton"
    Private Function IsConnectionTimeout(ByVal str As String) As Boolean
        Console.WriteLine("Time out retrying...")
        If str.ToLower().IndexOf("timeout") > -1 Then Return True
        Return False
    End Function

#Region "Oracle Connextion"
    Public Sub OpenOraConnection(ByRef cnnOra As OracleConnection, ByVal cnnString As String)
        cnnOra = New OracleConnection()
        cnnOra.ConnectionString = cnnString

        Try
Retry:
            If cnnOra.State = ConnectionState.Closed Then cnnOra.Open()

        Catch ex As Exception
            If IsConnectionTimeout(ex.ToString) Then GoTo Retry
            Throw New Exception("Error : " & ex.ToString, ex)
        End Try
    End Sub

    Public Sub CloseOraConnection(ByRef cnnOra As OracleConnection)
        If Not IsNothing(cnnOra) Then
            If cnnOra.State <> ConnectionState.Closed Then cnnOra.Close()
            If cnnOra IsNot Nothing Then
                cnnOra.Dispose()
            End If

        End If

        cnnOra = Nothing
    End Sub

    Public Function OraExecuteQuery(ByVal sSQL As String, ByRef cn As OracleConnection) As DataSet
        Dim cmd As OracleCommand = Nothing
        Dim da As OracleDataAdapter = Nothing

        Try
Retry:
            cmd = New OracleCommand(sSQL, cn)
            cmd.CommandTimeout = 600
            da = New OracleDataAdapter(cmd)
            Dim ds As DataSet = New DataSet()
            da.Fill(ds)

            Return ds
        Catch ex As Exception
            If IsConnectionTimeout(ex.ToString) Then GoTo Retry
            Throw New Exception("Error : " & ex.ToString, ex)
        Finally
            cmd = Nothing
            da = Nothing
        End Try
    End Function

    Public Function OraExecuteInsertUpdate(ByVal sSQL As String, ByRef cn As OracleConnection) As OracleCommand
        Dim cmd As OracleCommand = Nothing

        'Start a transaction
        Dim txn As OracleTransaction = cn.BeginTransaction(IsolationLevel.ReadCommitted)

        Try
Retry:
            cmd = New OracleCommand(sSQL, cn)
            cmd.CommandTimeout = 600
            'cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            txn.Commit()

            Return cmd
        Catch ex As Exception
            If IsConnectionTimeout(ex.ToString) Then GoTo Retry

            txn.Rollback()

            Throw New Exception("Error : " & ex.ToString, ex)
        Finally
            cmd = Nothing
        End Try
    End Function

#End Region
#End Region

End Class
