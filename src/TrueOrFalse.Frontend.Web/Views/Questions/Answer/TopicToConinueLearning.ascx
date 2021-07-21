
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopicToContinueLearningModel>" %>

<div class="col-xs-12">
    <% if (!Model.IsLearningSession &&  Model.ContentRecommendationResult.Categories.Count != 0)
       { %>
        <h4 class="marginTop50Bottom30">Themen zum Weiterlernen:</h4>
        <div id="ParentsChildrenTopics">
            <% if(Model.Categories.Count <= 3) { 
                   for (var i = 0; i < Model.Categories.Count; i++)
                   {
                       var category = Model.Categories[i].Id;
            %>
                    <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-3" style="">
                        <% Html.RenderPartial("~/Views/Welcome/Partials/WelcomeCardMiniCategory.ascx", new WelcomeCardMiniCategoryModel(category)); %>
                    </div>
            <% }
               }
               else
               {                  
                   for (var i = 0; i < 4; i++)
                   {
                       var category = Model.Categories[i].Id; %>

                    <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-3" style="">
                        <% Html.RenderPartial("~/Views/Welcome/Partials/WelcomeCardMiniCategory.ascx", new WelcomeCardMiniCategoryModel(category)); %>
                    </div>
            <% }

                    }%>
            <% } %>
        </div>
    </div>
