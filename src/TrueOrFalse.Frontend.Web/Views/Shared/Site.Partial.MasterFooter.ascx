<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="col-xs-12">
    <div class="row">
        <div class="col-xs-12">
            <div class="partnersAndSponsors">
                <div class="row">
                    <div class="col-xs-4 col-md-2" style="text-align: left;">
                        <img class="partnerImage" src="/Images/LogosPartners/BMWi-Logo-t.png" width="142" height="120"/>
                    </div>
                    <div class="col-xs-4 col-md-2">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-EXIST-eps.png" width="115" height="73" style="margin-top: 23px"/>
                    </div>
                    <div class="col-xs-4 col-md-2 alignRightUntillMD">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-ESF-rgb-gif.png" width="115" height="57" style="margin-top: 31px"/>
                    </div>
                    <div class="clearfix visible-xs visible-sm"></div>
                    <div class="col-xs-4 col-md-2 alignLeftUntillMD">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-EU-cmyk-eps.png" width="62" height="69" style="margin-top: 30px"/>
                    </div>
                    <div class="col-xs-4 col-md-2">
                        <img class="partnerImage" src="/Images/LogosPartners/Claim-ESF-cmyk-eps.png" width="115" height="48" style="margin-top: 33px"/>
                    </div>
                    <div class="col-xs-4 col-md-2" style="text-align: right;">
                        <img class="partnerImage" src="/Images/LogosPartners/profund-innovation-logo-t.png" width="160" height="60" style="margin-top: 30px"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="row">
        
        <div class="FooterCol xxs-stack col-xs-6 col-md-4 col-xs-12">
            <b>Memucho</b><br/>
            <a href="<%=Links.AboutMemucho() %>">Über memucho</a><br/>
            <a href="<%=Links.ForTeachers() %>">memucho für Lehrer/Dozenten</a><br/>
            &nbsp;<br/>
            <a href="<%= Links.WidgetExamples() %>">Beispiele Widgets</a><br/>
            <a href="<%= Links.WidgetPricing() %>">Angebote und Preise für Widgets</a><br/>
            &nbsp;<br/>
            <a href="<%=Links.WelfareCompany() %>">Gemeinwohlökonomie</a><br/>
            <a href="<%=Links.FAQItem("Contact") %>">Kontakt</a><br/>
            <a href="<%=Links.Jobs() %>">Jobs</a><br/>
            <a href="<%=Links.Directions %>">Anfahrt</a><br/>
        </div>
        
        <div class="FooterCol xxs-stack col-xs-6 col-md-4 col-xs-12">        
            
            <b>Software</b><br/>
            <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github">&nbsp;</i>Github</a> <br/>
            <a href="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1">
                <i class="fa fa-cogs">&nbsp;</i>Teamcity
            </a><br/>
            <% if(Request.IsLocal){ %>
                <%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight")  %><br/>
            <% } %>
            <% var assembly = Assembly.Load("TrueOrFalse"); %>
            <span style="color:darkgray">
                (Build: <%= assembly.GetName().Version.Major %> am
                <%= Html.Raw(AssemblyLinkerTimestamp.Get(assembly).ToString("dd.MM.yyyy 'um' HH:mm")) %>)
            </span>
            <br/>&nbsp;
            <br />
            <b>Rechtliches</b><br />
            <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen (AGBs)</a><br/>
            <a href="<%=Links.Imprint %>">Impressum & Datenschutz</a><br/>
        </div>
        
        <div class="FooterCol xxs-stack col-xs-6 col-md-2 col-xs-12">
            <b>Hilfe</b><br/>
            <a href="<%=Links.HelpFAQ() %>">FAQ</a><br/>
            <a href="<%= Links.HelpWidget() %>">Hilfe zu Widgets</a><br/>
            Christof: <span style="white-space: nowrap;">+49-1577-6825707</span>
        </div>
        
        <div class="FooterCol xxs-stack col-xs-6 col-md-2 col-xs-12">
            <div class="FooterLastCol">
                <b>Mehr</b><br/>
                <div class="fb-like" data-href="https://www.facebook.com/MemuchoWissen" data-layout="button" data-action="like" data-size="small" data-show-faces="true" data-share="false"></div><br/>
                <a href="https://www.facebook.com/MemuchoWissen"><i class="fa fa-facebook-official" aria-hidden="true">&nbsp;</i>auf Facebook</a><br/>
                <a href="https://twitter.com/memuchoWissen"><i class="fa fa-twitter" aria-hidden="true">&nbsp;</i>auf Twitter</a><br/>
            </div>
        </div>
    </div>
    <div class="Clearfix"></div>
</div>