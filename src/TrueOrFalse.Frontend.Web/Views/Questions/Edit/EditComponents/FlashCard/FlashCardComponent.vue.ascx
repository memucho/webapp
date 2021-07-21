<flashcard-component inline-template :solution="flashCardJson" :highlight-empty-fields="highlightEmptyFields">
    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>
        <template v-if="answerEditor">
            <editor-menu-bar-component :editor="answerEditor"/>
        </template>
        <template>
            <editor-content :editor="answerEditor" :class="{ 'is-empty': highlightEmptyFields && answerEditor.state.doc.textContent.length <= 0 }"/>
        </template>
        <div v-if="highlightEmptyFields && answerEditor.state.doc.textContent.length <= 0" class="field-error">Bitte gib eine Antwort an.</div>

    </div>
</flashcard-component>
