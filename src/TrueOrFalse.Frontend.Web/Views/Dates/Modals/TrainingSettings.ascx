<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalTraining" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                <button class="close" data-dismiss="modal">×</button>
                <div class="row">
                    <div class="col-md-12">
                        <h3 style="font-size: 22px; margin-top: 0px; display: inline-block;">
                            Lernplan
                            <select style="font-size: 15px; padding-left: 3px;" id="SelectTrainingDates">
                            </select>
                        </h3>
                        <div class="pull-right" style="font-size: 15px; margin-top: 5px;">
                            <a href="#" id="showSettings" data-action="showSettings">zeige Einstellungen <i class="fa fa-cogs"></i></a>
                            <a href="#" style="display:none" id="closeSettings" data-action="closeSettings">verberge Einstellungen <i class="fa fa-cogs"></i></a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-body" style="padding-top: 0px;">
                <div id="settings" style="display:none">
                    <div class="row" style="margin-top: 0px;">
                        <div class="col-md-12">
                            <h4 class="ColoredUnderline Date">
                                Einstellungen
                                <a href="#" class="pull-right" style="font-size: 11px;" data-action="closeSettings">x schließen</a>
                            </h4>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Lernstrategie:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <select id="selectLearningStrategy">
                                <option value="learningStrategyEnduring">Nachhaltig lernen</option>
                                <option value="learningStrategyMinimalEffort">Minimaler Aufwand</option>
                                <option value="learningStrategyMarginalBenefit">Grenznutzenoptimiert</option>
                            </select>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Ruhezeiten:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            jeden Tag 22:00-8:00 <a href="#" class="featureNotImplemented"><i class="fa fa-plus-square-o"></i></a>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4"></div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <a href="#" data-action="showAdvancedSettings">erweiterte Einstellungen zeigen <i class="fa fa-caret-down"></i></a>
                            <a href="#" data-action="hideAdvancedSettings" style="display: none">erweiterte Einstellungen verbergen <i class="fa fa-caret-up"></i></a>
                        </div>
                    </div>

                    <div id="divAdvancedSettings" style="display: none">
                        <div class="row" style="margin-top: 12px;">
                            <div class="col-md-4">
                                <div class="pull-right">Fragen pro Termin Ideal:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtQuestionsPerDateIdealAmount" style="width: 30px; height: 20px; font-size: 13px;" value="10"/>
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right">Mind.Abstand Sitzungen:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtMinSpacingBetweenSessionsInMinutes" style="width: 30px; height: 20px; font-size: 13px;" value="180"/>min
                                <i class="fa fa-question-circle show-tooltip" data-toggle="tooltip" title="Gibt den Abstand in Minuten an, der mindestens zwischen zwei Lernsitzungen liegen muss."></i>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 4px;">
                            <div class="col-md-4">
                                <div class="pull-right">Pro Termin mindestens:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtQuestionsPerDateMinimum" style="width: 30px; height: 20px; font-size: 13px;" value="7"/>
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right">Abstand anpassen:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="checkbox" id="chkbxEqualizeSpacingBetweenSessions" style="width: 30px;" value="true" checked="checked"/>
                                <i class="fa fa-question-circle show-tooltip" data-toggle="tooltip" title="Je größer der Abstand zum Termin, desto größer wird der Mindestabstand zwischen einzelnen Lernsitzungen."></i>
                            </div>

                        </div>
                        <div class="row" style="margin-top: 4px;">
                            <div class="col-md-4">
                                <div class="pull-right">Vergessensschwelle:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtAnswerProbabilityThreshold" style="width: 30px; height: 20px; font-size: 13px;" value="92"/>%
                                <i class="fa fa-question-circle show-tooltip" data-toggle="tooltip" title="Eine Frage wird wiederholt, wenn die Wahrscheinlichkeit einer korrekten Antwort unter diesen Wert sinkt."></i>
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right EqualizeSpacingOptions">Max. Multiplikator:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtEqualizedSpacingMaxMultiplier" style="width: 30px; height: 20px; font-size: 13px;" value="90" class="EqualizeSpacingOptions"/>
                                <i class="fa fa-question-circle show-tooltip" data-toggle="tooltip" title="Maximaler Multiplikator für Mindestabstand zwischen zwei Lernsitzungen. Je größer die Zahl, desto größer der Mindestabstand. (Empfohlen: 90)"></i>
                            </div>
                        </div>                        
                        <div class="row" style="margin-top: 4px;">
                            <div class="col-md-4">
                                &nbsp;
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                &nbsp;
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right EqualizeSpacingOptions">Tages-Verzögerer:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtEqualizedSpacingDelayerDays" style="width: 30px; height: 20px; font-size: 13px;" value="180" class="EqualizeSpacingOptions"/>
                                <i class="fa fa-question-circle show-tooltip" data-toggle="tooltip" title="Je kleiner die Zahl, desto länger ist der erhöhte Mindestabstand wirksam. (Genauer: Bei soviel Tagen Abstand zum Termin wird der Mindestabstand mit dem halben oben angegebenen Faktor multipliziert. Empfohlen: 180)"></i>
                            </div>

                        </div>                        
                    </div>

                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Übersicht</h4></div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <p>
                            Termin: <span id="DateOfDate"></span><br/>
                            Anzahl Fragen: <span id="QuestionCount"></span>
                        <p/>
                        Verbleibender Lernaufwand (geschätzt):<br/>
                        <i class="fa fa-calendar-o"></i> <span id="RemainingDates"></span> Lernsitzungen<br/>
                        <i class="fa fa-clock-o"></i> <span id="RemainingTime"></span> Lernzeit
                    </div>
                    <div class="col-md-8">
                        <div id="chartTrainingTime"></div>
                    </div>
                </div>
                <div class="row" id="divWarningLearningGoal" style="display: none;">
                <%--
                    <!-- do not show warning as long as it is shown in inappropriate cases -->
                    <div class="col-md-12">
                        <div class="alert alert-danger">
                            <p>
                                <strong>Warnung: Lernziele werden nicht erreicht</strong><br/>
                                Mit den aktuellen Einstellungen wirst du zum Termin vermutlich nicht alle Fragen sicher beherrschen. 
                                Bitte passe die erweiterten Einstellungen an (mehr Fragen pro Termin, geringerer Mindestabstand zwischen Lernsitzungen). 
                            </p>
                        </div>
                    </div>
                    --%>
                </div>
                
                <div class="row">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Vorgeschlagene Lernsitzungen</h4></div>
                </div>
                
                <div id="divTrainingPlanDetailsSpinner" class="row" style="display: none">
                    <div class="col-md-12" style="height: 200px">
                        <div style="display: block; text-align: center; margin-top: 80px;">
                            <i class="fa fa-refresh fa-spin" style="font-size: 30px;"></i>
                        </div>
                    </div>
                </div>
                
                <div id="divTrainingPlanDetails">
                    <div class="row" style="margin-top: 4px">
                        <div class="col-md-offset-8 col-md-2">Vorher</div>
                        <div class="col-md-2">Nachher</div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div style="height: 1px; background-color: rgb(219, 219, 219);"></div>
                        </div>
                    </div>
                    <div id="dateRows"></div>
                </div>
            </div>
            <div class="modal-footer" style="padding-top: 7px; padding-bottom: 7px;">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>
        </div>
    </div>
</div>