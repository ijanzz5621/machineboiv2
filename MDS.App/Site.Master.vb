Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("USER_NAME") = Nothing Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    Protected Sub lbtnLogout_Click(sender As Object, e As EventArgs) Handles lbtnLogout.Click
        Session.Abandon()
        Response.Redirect("~/Default")
    End Sub
End Class