﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%= Styles.Render("~/bundles/QuestionList") %>
<%= Styles.Render("~/bundles/switch") %>
<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>
<div id="BorderQuestionList" style=""> </div>
    <div id="QuestionListApp" class="row">
    <div class="col-xs-12 drop-down-question-sort">
            <div>Du lernst {{questionsCount}} Fragen aus diesem Thema (<%=Model.AllQuestionsInCategory %>)</div>
        <session-config-component inline-template @update="updateQuestionsCount">
                <div>
            <div id="CustomSessionConfigBtn" @click="openModal()"><button class="btn btn-primary"><i class="fa fa-cog" aria-hidden="true"></i> Lernoptionen</button></div>
            <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header" id="SessionConfigHeader">
                            <h5>LERNOPTIONEN</h5>
                            <h4 class="modal-title" >{{title}} personalisieren</h4>
                        </div>
                        <div class="modal-body">
                            <transition name="fade">
                                <div class="restricted-options" v-show="!isLoggedIn && isHoveringOptions" @mouseover="isHoveringOptions = true" transition="fade">
                                    <div class="info-content" style="">Diese Optionen sind nur für eingeloggte Nutzer verfügbar.</div>
                                    <div class="restricted-options-buttons">                            
                                        <div type="button" class="btn btn-link" @click="goToLogin()">Ich bin schon Nutzer!</div>
                                        <a type="button" class="btn btn-primary" href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>">Jetzt Registrieren!</a>
                                    </div>
                                </div>
                            </transition>
                            <div ref="radioSection" class="must-logged-in" :class="{'disabled-radios' : !isLoggedIn}" @mouseover="isHoveringOptions = true" @mouseleave="isHoveringOptions = false">
                                    <transition name="fade">
                                        <div v-show="!isLoggedIn && isHoveringOptions" class="blur" :style="{maxWidth: radioWidth + 'px', maxHeight: radioHeight + 'px'}"></div>
                                    </transition>
                                    <div class="modal-section-label">Prüfungsmodus</div>
                                    <div class="test-mode">
                                        <div class="center">
                                            <input type="checkbox" id="cbx" style="display:none" v-model="isTestMode" />
                                            <label for="cbx" class="toggle">
                                                <span></span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="test-mode-info">
                                        Du willst es Wissen? Im Prüfungsmodus kannst Du Dein Wissen realistisch testen: zufällige Fragen ohne Antworthilfe und Wiederholungen. Viel Erfolg!
                                    </div>
                                    <div class="modal-divider"></div>
                                    <div id="CheckboxesLearnOptions" class="row">
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="AllQuestions" type="checkbox" v-model="allQuestions" :disabled="!isLoggedIn" value="False"/>
                                            Alle Fragen
                                        </label> <br />
                                        <label class="checkbox-label">
                                            <input id="QuestionInWishknowledge" type="checkbox" v-model="inWishknowledge" :disabled="!isLoggedIn" value="False"/>
                                            In meinem Wunschwissen
                                        </label>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="UserIsAuthor" type="checkbox" v-model="createdByCurrentUser" :disabled="!isLoggedIn" value="False"/>
                                            Von mir erstellt
                                        </label> <br />
                                        <label class="checkbox-label">
                                            <input id="IsNotQuestionInWishKnowledge" type="checkbox" v-model="isNotQuestionInWishKnowledge" :disabled="!isLoggedIn" value="False"/>
                                            Nicht in meinem Wunschwissen
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="sliders row">
                                <div class="col-sm-6">
                                    <label class="sliderLabel">Deine Antwortwahrscheinlichkeit</label>
                                    <div class="sliderContainer">
                                        <div class="leftLabel">gering</div>
                                        <div class="vueSlider">                            
                                            <vue-slider direction="ltr" :lazy="true" v-model="probabilityRange" :tooltip-formatter="percentages"></vue-slider>
                                        </div>
                                        <div class="rightLabel">hoch</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label class="sliderLabel"> Maximale Anzahl an Fragen</label>
                                    <div v-if="maxSelectableQuestionCount > 0" class="sliderContainer">
                                        <div class="leftLabel">0</div>
                                        <div class="vueSlider">                            
                                            <vue-slider :max="maxSelectableQuestionCount" v-model="selectedQuestionCount"></vue-slider>
                                        </div>
                                        <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                                    </div>
                                    <div v-else class="alert alert-warning" role="alert">Leider sind keine Fragen mit diesen Einstellungen verfügbar. Bitte ändere die Antwortwahrscheinlichkeit oder wähle "Alle Fragen" aus.</div>
                                    <div class="alert alert-warning" v-if="(selectedQuestionCount == 0) && maxSelectableQuestionCount > 0">Du musst mindestens 1 Frage auswählen.</div>
                                </div>
                            </div>
                            <div class="row modal-more-options" @click="displayNone = !displayNone">
                                <div class="more-options class= col-sm-12">
                                    <span>Mehr Optionen</span>
                                    <span class="angle">
                                        <i class="fas fa-angle-down"></i>
                                    </span>
                                </div>
                            </div>
                            <div id="QuestionSortSessionConfig">
                                <div class="center">
                                    <input type="checkbox" id="randomQuestions" style="display:none" v-model="randomQuestions" />
                                    <label for="randomQuestions" class="toggle">
                                        <span></span>
                                    </label>
                                </div>
                                Zufällige Fragen
                                <div class="center">
                                    <input type="checkbox" id="answerHelp" style="display:none" v-model="answerHelp" />
                                    <label for="answerHelp" class="toggle">
                                        <span></span>
                                    </label>
                                </div>
                                Antworthilfe
                                <div class="center">
                                    <input type="checkbox" id="repititions" style="display:none" v-model="repititions" />
                                    <label for="repititions" class="toggle">
                                        <span></span>
                                    </label>
                                </div>
                                Wiederholungen
                            </div>
                            <div class="themes-info" v-bind:class="{displayNone: displayNone}">
                                <p> Du lernst <b>113 Fragen</b> aus dem Thema Allgmeinwissen(4.112)</p>
                            </div>
                            <div class="row" v-bind:class="{displayNone: displayNone}">
                                <div id="SafeLearnOptions">
                                    <div class="col-sm-12 safe-settings">
                                        <label>
                                            <input type="checkbox" id="safeOptions" v-model="safeLearningSessionOptions" :disabled="!isLoggedIn"/>
                                            Diese Einstellungen für zukünftiges Lernen speichern.
                                        </label>
                                    </div>
                                    <div class="info-options col-sm-12">
                                        Ein Neustart deiner Lernsitzung setzt deinen Lernfortschritt zurück. Die Antwortwahrscheinlichkeit der bisher beantworteten Fragen bleibt erhalten.
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                            <div type="button" class="btn btn-primary" :class="{ 'disabled' : maxQuestionCountIsZero }" @click="loadCustomSession()"><i class="fas fa-play"></i> Anwenden</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </session-config-component>
            <div class="Button dropdown">
                <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right">
                    <li><a href="<%= Links.CreateQuestion(Model.CategoryId) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                    <li><a href="#" @click="toggleQuestionsList()"><i class="fa fa-angle-double-down"></i>&nbsp;Alle Fragen erweitern</a></li>
                    <li><a href="#" data-allowed="logged-in" @click="loadCustomSession()"><i class="fa fa-play"></i>&nbsp;Fragen jetzt lernen </a></li>
                </ul>
            </div>
        </div>
        <question-list-component 
            inline-template 
            category-id="<%= Model.CategoryId %>" 
            :all-question-count="questionsCount" 
            is-admin="<%= Model.IsInstallationAdmin %>"  
            :is-question-list-to-show="isQuestionListToShow"
            :active-question ="activeQuestion"
            :selected-page-from-parent="selectedPageFromParent">
            <div class="col-xs-12">
                <question-component inline-template
                                    v-for="(q, index) in questions"
                                    :question-id="q.Id" 
                                    :question-title="q.Title" 
                                    :question-image="q.ImageData" 
                                    :knowledge-state="q.CorrectnessProbability" 
                                    :is-in-wishknowledge="q.IsInWishknowledge" 
                                    :url="q.LinkToQuestion" 
                                    :has-personal-answer="q.HasPersonalAnswer" 
                                    :is-admin="isAdmin"
                                    :is-question-list-to-show ="isQuestionListToShow"
                                    :question-index="index"
                                    :all-questions-count="allQuestionCount"
                                    :active-question ="activeQuestion">
                    
                    <div class="singleQuestionRow" :class="[{ open: showFullQuestion}, backgroundColor]">
                        <div class="questionSectionFlex col-auto">
                            <div class="questionContainer">
                                <div class="questionBodyTop row">
                                    <div class="questionImg col-xs-1" @click="expandQuestion()">
                                        <img :src="questionImage"></img>
                                    </div>
                                    <div class="questionContainerTopSection col-xs-11" >
                                        <div class="questionHeader row">
                                            <div class="questionTitle col-xs-10" ref="questionTitle" :id="questionTitleId" :class="{ trimTitle : !showFullQuestion }" @click.self="expandQuestion()">{{questionTitle}}</div>
                                            <div class="questionHeaderIcons col-xs-2 row"  @click.self="expandQuestion()">
                                                <div class="iconContainer col-xs-4 float-right" @click="expandQuestion()">
                                                    <i class="fas fa-angle-down rotateIcon" :class="{ open : showFullQuestion }"></i>
                                                </div>
                                                <div class="">
                                                    <div :id="pinId" class="Pin" :data-question-id="questionId"></div>
                                                </div>
                                                <div class="go-to-question iconContainer col-xs-4">
                                                    <span class="fas fa-play" :class="{ 'activeQ': questionIndex === activeQuestion }" :data-question-id="questionId" @click="loadSpecificQuestion()">
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                                            <div class="questionBody">
                                                <div class="RenderedMarkdown extendedQuestion">
                                                    <component :is="extendedQuestion && {template:extendedQuestion}"></component>
                                                </div>

                                                <div class="answer">
                                                    <strong>Antwort:</strong><br/>
                                                     <component :is="answer && {template:answer}"></component>

                                                </div>
                                                <div class="extendedAnswer" v-if="extendedAnswer != null && extendedAnswer.length > 0">
                                                    <strong>Ergänzungen zur Antwort:</strong><br/>
                                                    <component :is="extendedAnswer && {template:extendedAnswer}"></component>
                                                </div>
                                                <div class="notes">
                                                    <div class="relatedCategories">{{topicTitle}}: <a v-for="(c, i) in categories" :href="c.linkToCategory">{{c.name}}{{i != categories.length - 1 ? ', ' : ''}}</a></div>
                                                    <div class="author">Erstellt von: <a :href="authorUrl">{{author}}</a></div>
                                                    <div class="sources" v-if="references.length > 0 && references[0].referenceText.length > 0">Quelle: <a v-for="r in references" :href="r.referenceText">{{r.referenceText}}</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="questionBodyBottom" v-show="showFullQuestion">
                                        <div class="row">
                                                <div class="questionFooterIcons col-xs-3 row pull-right">
                                            <div class="footerIcon col-xs-6 pull-right ellipsis dropup" @click="showQuestionMenu = true">
                                                <i class="fas fa-ellipsis-v" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"></i>
                                                <ul class="dropdown-menu dropdown-menu-right" v-show="showQuestionMenu">
                                                    <li>
                                                        <a :href="url">Frageseite anzeigen</a>
                                                    </li>
                                                    <li v-if="isCreator || isAdmin == 'True' ">
                                                        <a :href="editUrl" >Frage bearbeiten</a>
                                                    </li>
                                                    <li id="DeleteQuestion" v-if="isCreator || isAdmin == 'True' ">
                                                        <a class="TextLinkWithIcon" data-toggle="modal" :data-questionid="questionId" href="#modalDeleteQuestion">
                                                            Frage löschen
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a :href="historyUrl">Versionen anzeigen</a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <div class="footerIcon col-xs-6 pull-right fullWidth" >
                                                <a class="commentIcon" :href="linkToComments">
                                                    <span>{{commentCount}}</span>
                                                    <i class="far fa-comment"></i>
                                                </a>
                                            </div>
                                                </div>
                                                <div class="questionStats col-xs-8 pull-right" style="display: flex;">
                                                    <div class="probabilitySection"><span class="percentageLabel" :class="backgroundColor">{{correctnessProbability}}</span> <span class="chip" :class="backgroundColor">{{correctnessProbabilityLabel}}</span></div>
                                                    <div></div>
                                                    <div>{{answerCount}} mal beantwortet | {{correctAnswers}} richtig / {{wrongAnswers}} falsch</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </question-component>
                <div id="QuestionListPagination">
                    <ul class="pagination col-xs-12 row justify-content-xs-center" v-if="pageArray.length <= 8">
                        <li class="page-item page-btn" :class="{ disabled : selectedPage == 1 }">
                            <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                        </li>
                        <li class="page-item" v-for="(p, key) in pageArray" @click="loadQuestions(p)" :class="{ selected : selectedPage == p }">
                            <span class="page-link">{{p}}</span>
                        </li>
                        <li class="page-item page-btn" :class="{ disabled : selectedPage == pageArray.length }">
                            <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                        </li>
                    </ul>

                    <ul class="pagination col-xs-12 row justify-content-xs-center" v-else>
                        <li class="page-item col-auto page-btn" :class="{ disabled : selectedPage == 1 }">
                            <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                        </li>
                        <li class="page-item col-auto" @click="loadQuestions(1)" :class="{ selected : selectedPage == 1 }">
                            <span class="page-link">1</span>
                        </li>
                        <li class="page-item col-auto" v-show="selectedPage == 5">
                            <span class="page-link">2</span>
                        </li>
                        <li class="page-item col-auto" v-show="showLeftPageSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span class="page-link" v-on:click.this="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                                <div class="dropup" v-on:click.this="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                                    <div class="dropdown-toggle" type="button" id="DropUpMenuLeft" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" v-on:click="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                                        ...
                                    </div>
                                    <ul id="DropUpMenuLeftList" class="pagination dropdown-menu" aria-labelledby="DropUpMenuLeft" v-show="showLeftSelectionDropUp">
                                        <li class="page-item" v-for="p in leftSelectorArray" @click="loadQuestions(p)">
                                            <span class="page-link">{{p}}</span>
                                        </li>
                                    </ul>
                                </div>
                            </span>
                        </li>
                        <li class="page-item col-auto" v-for="(p, key) in centerArray" @click="loadQuestions(p)" :class="{ selected : selectedPage == p }">
                            <span class="page-link">{{p}}</span>
                        </li>

                        <li class="page-item col-auto" v-show="showRightPageSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span class="page-link" v-on:click.this="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                                <div class="dropup" v-on:click.this="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                                    <div class="dropdown-toggle" type="button" id="DropUpMenuRight" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" v-on:click="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                                        ...
                                    </div>
                                    <ul id="DropUpMenuRightList" class="pagination dropdown-menu" aria-labelledby="DropUpMenuLeft" v-show="showRightSelectionDropUp">
                                        <li class="page-item" v-for="p in rightSelectorArray" @click="loadQuestions(p)">
                                            <span class="page-link">{{p}}</span>
                                        </li>
                                    </ul>
                                </div>
                            </span>
                        </li>
                        <li class="page-item col-auto" v-show="selectedPage == pageArray.length - 4">
                            <span class="page-link">{{pageArray.length - 1}}</span>
                        </li>
                        <li class="page-item col-auto" @click="loadQuestions(pageArray.length)" :class="{ selected : selectedPage == pageArray.length }">
                            <span class="page-link">{{pageArray.length}}</span>
                        </li>
                        <li class="page-item col-auto page-btn" :class="{ disabled : selectedPage == pageArray.length }">
                            <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                        </li>
                    </ul>
                </div>
            </div>
        </question-list-component>
    </div>
<%= Scripts.Render("~/bundles/js/QuestionListApp") %>





