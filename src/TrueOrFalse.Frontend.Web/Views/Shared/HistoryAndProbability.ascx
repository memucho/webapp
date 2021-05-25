<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HistoryAndProbabilityModel>" %>

<% if(Model.QuestionValuation.IsInWishKnowledge) { 
        var status = Model.QuestionValuation.KnowledgeStatus; %>
        <div class="StatsRow" style="margin-bottom: 4px;">
            <div style="background-color: <%= status.GetColor() %>; display: inline-block; font-size: 13px;  padding: 2px 4px; -ms-border-radius: 5px; border-radius: 5px;">
                <%= status.GetText() %>
            </div>
        </div>
<% } %>

<div class="StatsRow">
    <% Html.RenderPartial("AnswerHistory", Model.AnswerHistory); %> 
</div>
<div class="StatsRow">
    <% Html.RenderPartial("CorrectnessProbability", Model.CorrectnessProbability); %>
</div>

<% if(Model.LoadJs){ %>
    <script type="text/javascript">
        FillSparklineTotals();
        $('.show-tooltip').tooltip();
    </script>
<% } %>