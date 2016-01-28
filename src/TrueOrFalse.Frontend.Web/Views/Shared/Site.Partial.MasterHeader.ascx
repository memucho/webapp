﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<header id="MasterHeader" class="<%= GetRandomLogoCssClass.Run() %>">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row HeaderMainRow">
                    <div class="col-xs-6 col-title">
                        <a id="MenuButton"><i class="fa fa-bars"></i><span class="caret"></span></a>
                        <a class="block title" href="/">
                            <h1><span id="m">m</span>em<span id="uch"><span>u</span><span>c</span><span>h</span></span>o</h1>
                        </a>
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