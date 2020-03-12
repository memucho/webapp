<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>

<div id="app">
    <h2 id="h2TpopicAndLearnset"></h2>

    <div class="col-xs-4 switch" style="text-align: left; font-size: 18px;  width: 27%">Zeige nur von mir erstellte Inhalte</div>
    <div class="col-xs-1 switch">

        <div class="onoffswitch">
            <input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="switchShowOnlySelfCreated" @click="switchOnlySelfCreatedChanged()">
            <label class="onoffswitch-label" for="switchShowOnlySelfCreated">
                <span class="onoffswitch-inner"></span>
                <span class="onoffswitch-switch"></span>
            </label>
        </div>
    </div>
    <div id="table-wrapper" class="ui">
      <vuetable ref="vuetable"
        api-url="/Knowledge/GetCatsWish"
        :fields="fields"
        :sort-order="sortOrder"
        :css="css.table"
        pagination-path=""
        :per-page="15"
        :append-params="moreParams"
        @vuetable:pagination-data="onPaginationData"
        @vuetable:loading="onLoading()"
        @vuetable:loaded="onLoaded()">

          <!-- Topic ImageAndTitle-->
        <template slot="imageAndTitle" scope="props">
            <input type="hidden" class="hddCountDates" v-bind:value="props.rowData.ListCount"/>
            <div class="imageParent">
                <image class="image" v-bind:src="props.rowData.ImageFrontendData.Url"></image>
            </div>
            <div class="set-category-title">
                <a v-bind:href="props.rowData.LinkToSetOrCategory">{{props.rowData.Title}}</a>
            </div>
        </template>

        <!-- Topic Count-->      
        <template slot="topicCount" scope="props">
            <div class="topic-count">
                <div><span>{{props.rowData.QuestionsCount}} Fragen</span></div>
            </div>
        </template>

        <!-- Dropdownmenu -->
        <template slot="dropDown" scope="props">
        <div class="Button">
            <a v-bind:href="props.rowData.LinkToSetOrCategory + '/Lernen'" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                <i class="fa fa-lg fa-line-chart"></i>&nbsp;lernen
            </a>
        </div>
        <div class="Button dropdown" id ="DropdownTopic">
            <% var buttonId = Guid.NewGuid(); %>
            <a href="#" id="<%=buttonId %>" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>"  >
                <li><a v-bind:href="props.rowData.EditCategoryOrSetLink" target="_blank" rel="nofollow" data-allowed="logged-in"><i class="fa fa-pencil"></i>&nbsp;Bearbeiten</a></li>
                <li><a v-bind:href="props.rowData.CreateQuestionLink" target="_blank" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage erstellen und hinzufügen</a></li>
                <li @click="deleteRow(props.rowData.Id, props.rowData.IsCategory, props.rowIndex)"><a href="#"><i class="fa fa-trash-o"></i>&nbsp; Aus Wunschwissen entfernen </a></li> 
            </ul>
        </div>
        </template>
      </vuetable>
       <vuetable-pagination ref="pagination"
         :css="css.pagination"
         @vuetable-pagination:change-page="onChangePage">
       </vuetable-pagination>
    </div>
</div>

<%= Styles.Render("~/bundles/KnowledgeTopics") %>
<%= Scripts.Render("~/bundles/js/Vue") %>
<%= Scripts.Render("/Views/Knowledge/Js/KnowledgeTopics.js") %>
