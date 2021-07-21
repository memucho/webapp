<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>
<keep-alive>
    <text-component content="<%: HttpUtility.HtmlDecode(Model.Content)%>" inline-template>
        <div class="inline-text-editor" @click="contentIsChanged = true">
            <template v-if="editor">
                <editor-menu-bar-component :editor="editor" :heading="true"/>
            </template>
            <template v-if="editor">
                <editor-content :editor="editor">
                    <%: Html.Raw(Model.Content)  %>
                </editor-content>
            </template>

        </div>

    </text-component>
</keep-alive>
