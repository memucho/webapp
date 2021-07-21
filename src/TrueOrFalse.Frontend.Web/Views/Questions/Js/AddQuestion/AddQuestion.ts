Vue.component('editor-content', tiptapEditorContent);

Vue.component('add-question-component', {
    props: ['current-category-id'],
    data() {
        return {
            highlightEmptyFields: false,
            isLoggedIn: IsLoggedIn.Yes,
            visibility: 1,
            addToWishknowledge: true,
            questionEditor: new tiptapEditor({
                editable: true,
                extensions: [
                    tiptapStarterKit,
                    tiptapLink.configure({
                        HTMLAttributes: {
                            target: '_self',
                            rel: 'noopener noreferrer nofollow'
                        }
                    }),
                    tiptapCodeBlockLowlight.configure({
                        lowlight,
                    }),
                    tiptapUnderline,
                    tiptapPlaceholder.configure({
                        emptyEditorClass: 'is-editor-empty',
                        emptyNodeClass: 'is-empty',
                        placeholder: 'Gib den Fragetext ein',
                        showOnlyCurrent: true,
                    }),
                    tiptapImage
                ],
                onUpdate: ({ editor }) => {
                    this.questionJson = editor.getJSON();
                    this.questionHtml = editor.getHTML();
                    this.formValidator();
                },
            }),
            question: null,
            questionJson: null,
            questionHtml: null,
            solutionType: 9,
            flashCardAnswer: null,
            flashCardJson: null,
            licenseConfirmation: false,
            showMore: false,
            licenseIsValid: false,
            solutionIsValid: false,
            disabled: true,
        }
    },

    watch: {
        visibility() {
            this.formValidator();
        },
        licenseConfirmation() {
            this.formValidator();
        },
        flashCardAnswer() {
            this.formValidator();
        }
    },

    mounted() {
        if (this.visibility == 1)
            this.licenseIsValid = true;
    },

    methods: {
        addFlashcard() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateFlashCard");
                return;
            }
            if (this.disabled) {
                this.highlightEmptyFields = true;
                return;
            }
            var self = this;
            var lastIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
            var json = {
                CategoryId: this.currentCategoryId,
                TextHtml: this.questionHtml,
                Answer: this.flashCardAnswer,
                Visibility: this.visibility,
                AddToWishknowledge: this.addToWishknowledge,
                LastIndex: lastIndex,
            }
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Question/CreateFlashcard',
                data: JSON.stringify(json),
                success: function (data) {
                    var answerBody = new AnswerBody();
                    var skipIndex = this.questions != null ? -5 : 0;

                    answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                        "?skipStepIdx=" +
                        skipIndex +
                        "&index=" +
                        lastIndex);

                    eventBus.$emit('add-question-to-list', data.Data);
                    eventBus.$emit("change-active-question", lastIndex);
                    self.highlightEmptyFields = false;
                    self.questionEditor.commands.setContent('');
                    eventBus.$emit('clear-flashcard');
                },
            });
        },
        createQuestion() {
            var question = {
                categoryId: parseInt(this.currentCategoryId),
                edit: false,
            };
            eventBus.$emit('open-edit-question-modal', question);
        },
        formValidator() {
            var questionIsValid = this.questionEditor.state.doc.textContent.length > 0;
            var solutionIsValid = this.solutionIsValid;
            this.licenseIsValid = this.licenseConfirmation || this.visibility == 1;

            this.disabled = !questionIsValid || !solutionIsValid || !this.licenseIsValid;
        }
    }
})