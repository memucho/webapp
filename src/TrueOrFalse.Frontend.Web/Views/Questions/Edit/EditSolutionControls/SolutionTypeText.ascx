﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionExact>" %>

<%= Html.HiddenFor(m => m.MetadataSolutionJson) %>

<div class="form-group">
    <label class="columnLabel control-label">Antwortformat</label>
    <div class="columnControlsFull" style="height: 28px;">
        <div class="btn-group" style="position: relative; width: 76px; float: left; margin-right: 10px;">
            <a class="btn btn-default" style="padding: 3px 4px; border-bottom-left-radius: 0" id="btnMenuItemText"><img src="/Images/textfield-16.png" /></a>
            <a class="btn btn-default" style="padding: 3px 4px" id="btnMenuItemNumber"><img src="/Images/numeric_stepper-16.png" /></a>
            <a class="btn btn-default" style="padding: 3px 4px; border-bottom-right-radius: 0" id="btnMenuItemDate"><img src="/Images/date-16.png" /></a>    
        </div>
            
        <%-- MenuItemText --%>
        <div class="contextMenuOuter" id="divMenuItemText" style="left: 5px;">
            <div class="well contextMenu">
                <div style="margin-bottom: 5px">Groß-/Kleinschreibung:</div>
                <div class="btn-group">
                    <a class="btn active">Ignorieren</a>
                    <a class="btn btn-default featureNotImplemented">Beachten</a>
                </div>
                <div style="margin-top:10px; height: 20px;">
                    <a href="#" class="featureNotImplemented">
                        <label class="checkbox" style="width: auto" disabled="disabled">
                            <input type="checkbox" disabled="disabled">Exakte Schreibweise
                        </label>
                        </a>
                    <i class="fa fa-question-circle cursor-hand" id="help"></i>
                </div>
                <div style="clear: both"></div>
            </div>
        </div>
            
        <%-- MenuItemNumber --%>
        <div class="contextMenuOuter" id="divMenuItemNumber" style="left: 30px; width: 160px;">
            <div class="well contextMenu">
                <div style="margin-bottom: 5px">
                    Abweichung:
                    <input id="numberAccuracy" value="0" style="width: 20px;" />%    
                </div>
                <div>
                    Einheit: 
                    <input type="text" style="width: 100px;" />
                </div>
            </div>
        </div>
            
        <%-- MenuItemDate --%>
        <div class="contextMenuOuter" id="divMenuItemDate" style="left: 55px">
            <div class="well contextMenu">
                <div style="margin-bottom: 5px">
                    genau auf
                    <span id="spanSliderValue"></span>
                </div>         
                <div id="sliderDate" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 120px; margin-left:5px;"> 
                    <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                    <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
                </div>
                <div style="clear: both"></div>
            </div>
        </div>
    
        <div id="infoMetaDate">

            <span class="show-tooltip tooltip-text-left tooltip-date" data-placement="top" 
                title="<div style='margin-bottom: 3px;'>Je genauer desto besser!</div>
                    <div style='font-size:larger; font-weight:bold;'>Gültige Eingaben sind u.a.: </div>
                    <b>Tagesgenau:</b> 24.3.1999 (24 März 1999)<br />
                    <b>Monatsgenau:</b> 3.1999 (März 1999) <br />
                    <b>Jahresgenau:</b> 1999 (1999) <br />
                    <b>Jahrzehntgenau:</b>  172x (1720-ziger) <br />
                    <b>Jahrhundertgenau:</b> 19 Jh <br />
                    <b>Jahrtausendgenau:</b> 3 Jt<br />
                    <div style='margin-top:3px;'>
                        <i>(für Daten vor Chr. Geburt, ein Minuszeichen voranstellen, z.B.: -32)</i>
                    </div>
                
                " 
                data-html="true">
                Erfasst <span id="spanEntryPrecision"></span>
                <i class="fa fa-exclamation-circle" id="iDateError" style="color:red; font-size: 16px;"></i> 
                <i class="fa fa-check-circle" id="iDateCorrect" style="color:green; font-size: 16px;"></i> 
            </span>
            <br />
            Antwortgenauigkeit: <b><span id="spanAnswerPrecision"></span></b>.
        </div>
        <p id="infoMetaText" class="form-control-static">
                Exakte Texteingabe
        </p>
        <p id="infoMetaNumber" class="form-control-static">
            Exakte Zahl
        </p>
    </div>
</div>

<div class="form-group">
    
    <%= Html.LabelFor(m => m.Text, new { @class = "RequiredField columnLabel control-label" })%>
    <div class="columnControlsFull">
        <%= Html.TextBoxFor(m => m.Text, new { @class="form-control", @id = "Answer", @style = "float: left;", placeholder = "Antwort eingeben." })%>
    </div>
    
</div>

<% /* MODAL-TAB-INFO****************************************************************/ %>
    
<div id="modalHelpSolutionType" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Erklärung Lösungseigenschaften</h3>
            </div>
            <div class="modal-body">
                <h2>Groß- und Kleinschreibung</h2>
                <p>
                    Wenn "ignorieren" gewählt, dann wird bei der Eingabe die Groß- und Kleinschreibung ignoriert.
                </p>
                <h2>Exakte Schreibweise</h2>
                <p>
                    Ist "Exakte Schreibweise" gewählt, dann muss für eine korrekte Beantwortung die Eingabe exakt der Antwort entsprechen.
                </p>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-warning" data-dismiss="modal">Mmh ok, nun gut.</a>
                <a href="#" class="btn btn-info" data-dismiss="modal">Danke, ich habe verstanden!</a>
            </div>
        </div>
    </div>
</div>

<script src="/Views/Questions/Edit/EditSolutionControls/SolutionTypeText.js" type="text/javascript"></script>