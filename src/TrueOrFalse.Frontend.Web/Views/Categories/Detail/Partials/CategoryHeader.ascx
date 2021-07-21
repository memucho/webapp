<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryHeader">
    <%: Html.HiddenFor(m => m.ImageIsNew) %>
    <%: Html.HiddenFor(m => m.ImageSource) %>
    <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
    <%: Html.HiddenFor(m => m.ImageGuid) %>
    <%: Html.HiddenFor(m => m.ImageLicenseOwner) %>
    <% var buttonId = Guid.NewGuid(); %>
    <div id="HeadingSection">
        <%if (Model.Category.Creator == Sl.SessionUser.User || Sl.SessionUser.IsInstallationAdmin ) {%>
            <category-image-component category-id="<%= Model.Category.Id %>" inline-template is-learning-tab="<%= Model.IsInLearningTab %>">
                <div class="ImageContainer" @click="openImageUploadModal()">
                    <div class="imageUploadBtn" v-if="!disabled">
                        <div>
                            <i class="fas fa-pen"></i>
                        </div>
                        <div class="imageUploadLabel">
                            Verwende ein <br />
                            anderes Bild
                        </div>

                    </div>
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, isHeader: true) %>
                </div>
            </category-image-component> 
        <%} else {%>
            <div class="ImageContainer">
                <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category), isHeader: true) %>
            </div>
        <%} %>
        <div id="HeadingContainer" data-category-name="<%= Model.Name %>">
            <h1 style="margin-bottom: 0">

                <%if (Model.Category.Creator == Sl.SessionUser.User || Sl.SessionUser.IsInstallationAdmin ) {%>
                    <category-name-component inline-template old-category-name="<%= Model.Name %>" category-id="<%= Model.Category.Id %>" is-learning-tab="<%= Model.IsInLearningTab %>">
                        <textarea-autosize
                            placeholder="Type something here..."
                            ref="categoryNameArea"
                            v-model="categoryName"
                            :min-height="54"
                            rows="1"
                            @keydown.enter.native.prevent
                            @keyup.enter.native.prevent
                            :disabled="disabled"
                        />
                    </category-name-component>

                    <%} else {%>
                        <%= Model.Name %>
                <%} %>

                <%if (Model.Category.Visibility == CategoryVisibility.Owner) {%><i class="fas fa-lock header-icon"></i>
                <%} %>
            </h1>
            <div>
                <div class="greyed category-sub-header">
                    
                    <% if (!Model.Category.IsHistoric) { %>
                        <div class="Button Pin mobileHeader" data-category-id="<%= Model.Id %>">
                            <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, displayAdd: false)) %>
                            </a>
                        </div>
                    <% } %>
                    <div>
                        <% if (Model.AggregatedTopicCount == 1) { %> 1 Unterthema <% }
                           else if (Model.AggregatedTopicCount > 1){ %> <%= Model.AggregatedTopicCount %> Unterthemen <% }
                           else { %> 0 Unterthemen
                        <% } %>
                        und
                        <% if (Model.CountAggregatedQuestions == 1) { %> 1 Frage <% }
                           else if (Model.CountAggregatedQuestions > 1){ %> <%= Model.Category.GetCountQuestionsAggregated() %> Fragen <% }
                           else { %> 0 Fragen
                        <% } %>
                    </div>
                    <div class="category-sub-header-divider">
                        <div class="vertical-line"></div>
                    </div>
                    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeaderAuthors.ascx", Model); %>

                    <% if (Model.IsInstallationAdmin) { %>
                        <a href="#" id="jsAdminStatistics">
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fas fa-user-cog" data-details="<%= Model.GetViewsPerDay() %>">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>
                        </a>
                        <div id="last60DaysViews" style="display: none"></div>
                    <% } %>
                </div>
            </div>
            <% if (!Model.Category.IsHistoric) { %>
                <div class="KnowledgeBarWrapper mobileHeader">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                </div>
            <% } %>
            <%if (Model.Category.Visibility == CategoryVisibility.Owner) {%>
                <% Html.RenderPartial("~/Views/Categories/Detail/Partials/PublishCategory/PublishCategory.vue.ascx"); %>
            <%} %>
        </div>
    </div>

    <% if (!Model.Category.IsHistoric) { %>
        <div id="TabsBar">
            <div id="CategoryTabsApp" class="Tabs">
                <div id="TopicTab" class="Tab" data-url="<%=Links.CategoryDetail(Model.Name, Model.Id) %>" >
                    <div class="center-tab">
                        <a href="">
                            <%= Model.Category.Type == CategoryType.Standard ? "Thema" : "Übersicht" %>
                        </a>
                    </div>

                </div>
                <div id="LearningTabWithOptions" class="Tab">
                    <div id="LearningTab" class="Tab" data-url="<%=Links.CategoryDetailLearningTab(Model.Name, Model.Id) %>">
                        <a href="" >
                            Fragen
                        </a>
                        <div id="LearnOptionsHeaderContainer">
                            <i id="LearnOptionsHeader" class="fa fa-cog disable" aria-hidden="true" data-toggle="tooltip" data-html="true" title="<p style='width: 200px'><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
                            </i>
                            <% if (!Model.ShowLearningSessionConfigurationMessageForTab)
                               { %>
                                <div id="SessionConfigReminderHeader" class="hide">
                                    <span>
                                        <img src="/Images/Various/SessionConfigReminder.svg" class="session-config-reminder-header">
                                    </span>
                                    <span class="far fa-times-circle"></span>
                                </div>
                            <% } %>
                        </div>


                    </div>
                </div>
            </div>
            <div id="Management">
                <div class="Border hide-sm"></div>
                <div class="KnowledgeBarWrapper col-md-3 hide-sm">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                    <%--<div class="KnowledgeBarLegend">Dein Wissensstand</div>--%>
                </div>
                <% if (Model.Category.Id != RootCategory.RootCategoryId)
                   { %> 
                    <div class="Border hide-sm"></div>
                <% } %>
                <div class="Buttons"><%if (Model.Category.Id != RootCategory.RootCategoryId){%>
                        <div class="PinContainer hide-sm">
                            <div class="Button Pin pinHeader" data-category-id="<%= Model.Id %>">
                                <a href="#" class="noTextdecoration" style="font-size: 22px;">
                                    <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, isHeader: true)) %>
                                </a>
                            </div>
                        </div>
                    <%} %>


                    <div id="MyWorldToggleApp" :class="{'active': showMyWorld}" <%if (Model.IsMyWorld){%> class="active"<%} %> v-cloak>
                        <div class="toggle-label">
                            <div>
                                Zeige nur mein
                                <br/>
                                <b>Wunschwissen</b>
                            </div>
                        </div>
                        <% Html.RenderPartial("~/Views/Shared/MyWorldToggle/MyWorldToggleComponent.vue.ascx", Model); %>
                    </div>
                    <div class="Button dropdown DropdownButton">
                        <% buttonId = Guid.NewGuid(); %>
                        <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                            <li>
                                <a href="<%= Links.CategoryHistory(Model.Id) %>" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-code-fork"></i>
                                    </div>
                                    Bearbeitungshistorie
                                </a>
                            </li>
                            <li>
                                <a onclick="eventBus.$emit('open-edit-question-modal', {
                                                                            categoryId: <%= Model.Category.Id %>,
                                                                            edit: false
                                                                        })" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-plus-circle"></i>
                                    </div>
                                    Frage hinzufügen
                                </a>
                            </li>
                            <li>
                                <a onclick="eventBus.$emit('open-add-category-modal', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-plus-circle"></i>
                                    </div>
                                    Thema Erstellen
                                </a>
                            </li>
                            <li>
                                <a href="" id="AnalyticsTab" data-url="<%=Links.CategoryDetailAnalyticsTab(Model.Name, Model.Id) %>" data-allowed="logged-in" class="Tab" >
                                    <div class="dropdown-icon">
                                        <i class="fas fa-project-diagram"></i>
                                    </div>
                                    Wissensnetz anzeigen
                                </a>
                            </li>
                            <%if (Model.IsOwnerOrAdmin) {%>
                                <li>
                                    <a onclick="eventBus.$emit('open-delete-category-modal', <%= Model.Category.Id %>)" data-allowed="logged-in">
                                        <div class="dropdown-icon">
                                            <i class="fas fa-trash"></i>
                                        </div>
                                        Thema löschen
                                    </a>
                                </li>
                            <%}%>
                        </ul>
                    </div>
                </div>
                

            </div>
        </div>
    <% } %>
    
</div>
<% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>
<%= Scripts.Render("~/bundles/fileUploader") %>
<%= Styles.Render("~/bundles/CategoryEdit") %>
<%= Scripts.Render("~/bundles/js/CategoryEdit") %>
<%= Scripts.Render("~/bundles/js/PublishCategory") %>
<%= Scripts.Render("~/bundles/js/MyWorldToggle") %>

