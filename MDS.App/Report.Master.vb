Public Class Report
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Session("USER_NAME") = Nothing Then
        '    Response.Redirect("~/Default.aspx")
        'End If

        ' Added by Sharizan on 23 March 2018
        ' Hide maintenance and setup for non admin user
        If Session("IS_ADMIN_USER") <> Nothing And Session("IS_ADMIN_USER") = True Then
            liMaintenance.Visible = True
            liSetup.Visible = True
        Else
            liMaintenance.Visible = False
            liSetup.Visible = False
        End If

    End Sub

    Protected Sub lbtnLogout_Click(sender As Object, e As EventArgs) Handles lbtnLogout.Click
        Session.Abandon()
        Response.Redirect("~/Default")
    End Sub
End Class