<%@ Control Language="C#" Inherits="ViewUserControl<AddQuestionComponentModel>" %>
<add-question-component inline-template current-category-id="<%: Model.CategoryId %>">
    <div id="AddInlineQuestionContainer">

        <div id="AddQuestionHeader" class="">
            <div class="add-inline-question-label main-label">
                Frage hinzufügen 
                <span>(Karteikarte)</span>
                <div style="cursor: pointer"@click="createQuestion()">erweiterte Optionen</div>
            </div>
            <div class="heart-container wuwi-red" @click="addToWishknowledge = !addToWishknowledge">
                <div>
                    <i class="fa fa-heart" :class="" v-if="addToWishknowledge"></i>
                    <i class="fa fa-heart-o" :class="" v-else></i>
                </div>
                <div class="Text">
                    <span v-if="addToWishknowledge">Hinzugefügt</span>
                    <span v-else class="wuwi-grey">Hinzufügen</span>
                </div>
            </div>
        </div>

        <div id="AddQuestionBody">
            <div id="AddQuestionFormContainer"  class="inline-question-editor">
                <div>
                    <div class="overline-s no-line">Frage</div>
                    <template v-if="questionEditor">
                        <editor-menu-bar-component :editor="questionEditor"/>
                    </template>
                    <template>
                        <editor-content :editor="questionEditor" :class="{ 'is-empty': highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0 }"/>
                    </template>
                    <div v-if="highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0" class="field-error">Bitte formuliere eine Frage.</div>
                </div>
                <div>
                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/FlashCard/FlashCardComponent.vue.ascx") %>

                </div>
                <div class="input-container">
                    <div class="overline-s no-line">                
                        Sichtbarkeit
                    </div>
                    <div class="privacy-selector" :class="{ 'not-selected' : !licenseIsValid && highlightEmptyFields }">
                        <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/PrivacySelector.vue.ascx") %>
                    </div>
                </div>
                <div>
                    <div class="btn btn-lg btn-primary memo-button" @click="addFlashcard()">Hinzufügen</div>
                </div>
            </div>
        </div>
    </div>
</add-question-component>