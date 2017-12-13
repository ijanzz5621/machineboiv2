<%@ Page Title="Home" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MainPage.aspx.vb" Inherits="MDS.App.MainPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container">              

        <div class="row">

            <div class="col-xs-12 col-sm-8 col-md-8 col-lg-8">

                <div class="row">
                    <h3>Summary & Log (Top 10)</h3>
                </div>  

                <div class="row" style="padding:10px;">
                    
                    <div class="log-container" style="padding:10px;">

                        <div class="row">

                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                Joanne Tan
                            </div>

                            <div class="col-xs-12 col-sm-4 col-sm-offset-4 col-md-4 col-md-offset-4 col-lg-4 col-lg-offset-4" style="text-align:right;">
                                2017-06-05 12:00:00
                            </div>

                        </div>

                        <div class="row" style="padding:15px; margin-top:10px;">

                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">

                                <span style="font-weight:bold; font-size:25px;">"</span>
                                <span style="font-style:italic; font-size:20px; color:#059f1d;">Action: Upload BOI, Status: Successfull</span> 
                                <span style="font-weight:bold; font-size:25px;">"</span>

                            </div>

                            

                        </div>                        

                    </div>

                </div>

                <div class="row" style="padding:10px;">

                    <div class="log-container" style="padding:10px;">

                        <div class="row">

                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                Sharizan Mohd Redzuan
                            </div>

                            <div class="col-xs-12 col-sm-4 col-sm-offset-4 col-md-4 col-md-offset-4 col-lg-4 col-lg-offset-4" style="text-align:right;">
                                2017-06-01 14:00:00
                            </div>

                        </div>

                        <div class="row" style="padding:15px; margin-top:10px;">

                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">

                                <span style="font-weight:bold; font-size:25px;">"</span>
                                <span style="font-style:italic; font-size:20px; color: #ff0000;">
                                    Action: Entry Import, Status: Failed. Column data not correct.
                                    <br />
                                    Failed column list:
                                    <br />
                                    MES Date: Not a valid date (row no: 12)                              

                                </span> 
                                <span style="font-weight:bold; font-size:25px;">"</span>

                            </div>

                            

                        </div>                        

                    </div>

                </div>
            
                <div class="row" style="padding:10px;"><div class="log-container"></div></div> 
                
            </div>

            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">

                <div class="row" style="margin-left:15px;">

                    <div class="row">
                        <h3>News & Events</h3>
                    </div>
                    
                    <div class="row" style="padding:10px;">
                    
                        <div class="log-container" style="padding:10px; min-height:350px;">
                        </div>

                    </div>                     

                </div>

            </div>

        </div>

    </div>    

</asp:Content>
