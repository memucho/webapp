<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SetKnowledgeBarModel>" %>

<div class="set-knowledge-bar" data-set-id="<%= Model.Set.Id %>">

    <% if(Model.SetKnowledgeSummary.NeedsLearningPercentage > 0) { %>
        <div class="needs-learning show-tooltip"
            data-html="true"
            title="Solltest du festige: <br/> <%= Model.SetKnowledgeSummary.NeedsLearning %> Fragen (<%= Model.SetKnowledgeSummary.NeedsLearningPercentage %>%)"
            style="width: <%= Model.SetKnowledgeSummary.NeedsLearningPercentage %>%;"></div>
    <% } %>

    <% if(Model.SetKnowledgeSummary.NeedsConsolidationPercentage > 0) { %>
        <div class="needs-consolidation show-tooltip"
            data-html="true"
            title="Solltest du lernen: <br/> <%= Model.SetKnowledgeSummary.NeedsConsolidation %> Fragen (<%= Model.SetKnowledgeSummary.NeedsConsolidationPercentage %>%)"
            style="width: <%= Model.SetKnowledgeSummary.NeedsConsolidationPercentage %>%;"></div>
    <% } %>
    
    <% if(Model.SetKnowledgeSummary.SolidPercentage > 0) { %>
        <div class="solid-knowledge show-tooltip" 
            data-html="true"
            title="Sicheres Wissen: <br/> <%= Model.SetKnowledgeSummary.Solid %> Fragen (<%= Model.SetKnowledgeSummary.SolidPercentage %>%)" 
            style="width: <%= Model.SetKnowledgeSummary.SolidPercentage %>%;"></div>
    <% } %>

    <% if(Model.SetKnowledgeSummary.NotLearnedPercentage > 0) { %>
        <div class="not-learned show-tooltip"
            data-html="true"
            title="Noch nicht gelernt: <br/> <%= Model.SetKnowledgeSummary.NotLearned %> Fragen (<%= Model.SetKnowledgeSummary.NotLearnedPercentage %>%)"
            style="width: <%= Model.SetKnowledgeSummary.NotLearnedPercentage %>%;"></div>
    <% } %>

    <% if(Model.SetKnowledgeSummary.NotInWishknowledgePercentage > 0) { %>
        <div class="not-in-wish-knowledge show-tooltip"
            data-html="true"
            title="Noch nicht im Wunschwissen: <br/> <%= Model.SetKnowledgeSummary.NotInWishknowledge %> Fragen (<%= Model.SetKnowledgeSummary.NotInWishknowledgePercentage %>%)"
            style="width: <%= Model.SetKnowledgeSummary.NotInWishknowledgePercentage %>%;"></div>
    <% } %>
</div>