<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if(Request.Url.Host == "memucho.local" || Request.Url.Host == "memucho"){ %>
    <div class="" style="background-color: #afd534; z-index: 10000; position: fixed; top: 11px; right: -23px; width: 60px; text-align: center; font-size: 9px; padding: 2px; padding-left: 12px; transform: rotate(90deg); color:white">L O C A L</div>
<% } %>

<% if(Request.Url.Host == "future.memucho.de"){ %>
    <div class="container" style="width: 100%">
        <div class="row" style="background-color: lightpink; text-align: center; color:white"><div class="col-xs-12">F U T U R E</div></div>
    </div>
<% } %>

<% if(Request.Url.Host == "stage.memucho.de"){ %>
    <div class="container" style="width: 100%">
        <div class="row" style="background-color: orange; text-align: center; color:white"><div class="col-xs-12">S T A G E</div></div>
    </div>
<% } %>
<header id="MasterHeader">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row HeaderMainRow">
                    <div class="col-xs-6 col-Logo">
                        <a id="MenuButton"><i class="fa fa-bars"></i><span class="caret"></span></a>
                        <a id="LogoLink" href="/">
                            <div id="Pictogram">
                                <img src="/Images/Logo/LogoPictogram.png">
                            </div>
                            <div id="Wordmark">
                                <img src="/Images/Logo/LogoWordmarkWithTagline.png">
                            </div>
                            <div id="LogoSmall">
                                <img src="/Images/Logo/LogoSmall.png">
                            </div>
                        </a>
                        <a id="BetaLogo" href="<%= Links.BetaInfo() %>">
                            [beta]
                        </a>

                        <div class="input-group" id="HeaderSearchBoxDiv">
                            <input type="text" class="form-control" placeholder="Suche" id="headerSearchBox">
                            <div class="input-group-btn">
                                <button class="btn btn-default" type="submit"><i class="fa fa-search" aria-hidden="true"></i></button>
                            </div>
                        </div>

                    </div>
                    <div class="col-xs-6 col-LoginAndHelp">
            	        <div id="loginAndHelp" >
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>