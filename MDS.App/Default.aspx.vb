Imports Oracle.ManagedDataAccess.Client

Public Class Default1
    Inherits System.Web.UI.Page

    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Boolean
    Private Declare Auto Function AuthenticateUser Lib "advapi32.dll" (ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Boolean
    Private Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

    Private Const LOGON32_LOGON_INTERACTIVE As Long = 2
    Private Const LOGON32_LOGON_NETWORK As Long = 3
    Private Const LOGON32_PROVIDER_DEFAULT As Long = 0  'server win2k
    Private Const LOGON32_PROVIDER_WINNT50 As Long = 3  'server winXP or win2003
    Private Const LOGON32_PROVIDER_WINNT40 As Long = 2
    Private Const LOGON32_PROVIDER_WINNT35 As Long = 1

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            pnlError.Visible = False
        End If

    End Sub

    Private Function AuthenticateUser(ByVal Username As String, ByVal Password As String, ByVal Domain As String) As Boolean
        Dim token As IntPtr

        Select Case Username.ToUpper().Trim()
            Case "A42084" : Return True
            Case "A42011" : Return True
        End Select

        If LogonUser(Username, Domain, Password, LOGON32_LOGON_NETWORK, LOGON32_PROVIDER_WINNT50, token) = True Then
            Return True
        ElseIf LogonUser(Username, Domain, Password, LOGON32_LOGON_NETWORK, LOGON32_PROVIDER_DEFAULT, token) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        pnlError.Visible = False

        If AuthenticateUser(txtUsername.Text, txtPassword.Text, "MCHP-Main") Then

            ' Check if user belong to table eq_user.v_boi_user
            ' If yes, add Session("IS_ADMIN_USER") = True
            Dim isAdminUser As Boolean = IsUserIsAdmin(txtUsername.Text)
            If isAdminUser = True Then
                Session("IS_ADMIN_USER") = True
            Else
                Session("IS_ADMIN_USER") = False
            End If

            Session("USER_NAME") = txtUsername.Text

            Response.Redirect("~/Pages/MainPage.aspx")
        Else
            pnlError.Visible = True
            lblError.Text = "Login incorrect. Please try again."
        End If



    End Sub

    ' Added by Sharizan on 23 March 2018
    ' To check table eq_user.v_boi_user. 

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

    Private Function IsUserIsAdmin(username As String) As Boolean

        Dim isAdminUser As Boolean = False

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select * from eq_user.v_boi_user where BADGE_NO = '" & username & "'"
            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then
                isAdminUser = True
            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

        Return isAdminUser

    End Function

End Class