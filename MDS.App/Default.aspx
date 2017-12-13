<%@ Page Title="Login" Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="MDS.App.Default1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - Machine Database System</title>

    <script src="/Scripts/ie9/html5shiv.js"></script>
    <script src="/Scripts/ie9/respond.min.js"></script>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <style>

        body{
            background: url(../images/bg-cover.png) no-repeat center center fixed; 
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;

            font-family:'Century Gothic';

            /* For bubble animation */
            /*z-index:1;
            overflow-y:hidden;
            overflow-x:hidden;*/
        }

    </style>

</head>
<body>

    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>

        <div class="container body-content">

            <div class="jumbotron">
                <%--<h1 style="text-align:center;">Machine Database System</h1>--%>
                <img id="logo-title" src="images/login-title.png" style="text-align:center; margin: 0 auto;" class="img-responsive" />
        
            </div>

            <div class="row">

                <div class="col-md-4 col-md-offset-4 ">
            
                        <div class="card card-container" id="pop-up-123">
                            <!-- <img class="profile-img-card" src="//lh3.googleusercontent.com/-6V8xOA6M7BA/AAAAAAAAAAI/AAAAAAAAAAA/rzlHcD0KYwo/photo.jpg?sz=120" alt="" /> -->
                            <%--<img id="profile-img" class="profile-img-card" src="//ssl.gstatic.com/accounts/ui/avatar_2x.png" />
                            <p id="profile-name" class="profile-name-card"></p>--%>

                                <span id="reauth-email" class="reauth-email"></span>
                                Domain: <strong>MCHP-MAIN</strong>
                                <br /><br />                                
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="User Name" required autofocus style="margin-bottom:15px;"></asp:TextBox>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" required></asp:TextBox>
                                <div id="remember" class="checkbox">
                                    <%--<label>
                                        <input type="checkbox" value="remember-me"> Remember me
                                    </label>--%>
                                </div>
                                <%--<button class="btn btn-lg btn-primary btn-block btn-signin" type="submit">Sign in</button>--%>
                                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-lg btn-primary btn-block btn-signin" OnClick="btnLogin_Click" />

                            <%--<a href="#" class="forgot-password">
                                Forgot the password?
                            </a>--%>
                        </div><!-- /card-container -->


                </div>
        
            </div>

            <div class="row">

                <img src="images/Microchip_logo.png" width="200" style="text-align:center; margin: 0 auto;" class="img-responsive" />

            </div>

            <br /><br />

            <div class="row" style="text-align:center; margin: 0 auto;">

                <asp:Panel ID="pnlError" runat="server" Visible="false">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Large" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" BackColor="White" ></asp:Label>
                </asp:Panel> 

            </div>

        </div>        

    </form>

    <%--<ul class="bubbles">
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
    </ul>--%>
    

</body>
</html>
