<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
    <%if (Model.ShowContent) {%>
        <div v-if="textCanBeEdited">
            <inline-text-component/>
        </div>
        <div v-else @click="editInlineText()">
            <%: Html.Raw(HttpUtility.HtmlDecode(Model.Content))  %>
            <div v-if="missingText" class="missingTextInModule"> Hier klicken um Text zu bearbeiten</div>
        </div>
    <%} %>
<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>