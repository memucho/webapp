declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('editor-content', tiptapEditorContent);

var editQuestionComponent = Vue.component('edit-question-component',
    {
        data() {
            return {
                highlightEmptyFields: false,
                id: null,
                addToWuwi: false,
                solutionType: null,
                textSolution: null,
                singleSolutionJson: null,
                numericSolution: null,
                sequenceJson: null,
                dateSolution: null,
                multipleChoiceJson: null,
                matchListJson: null,
                flashCardJson: null,
                flashCardAnswer: null,
                edit: false,
                empty: 'empty',
                licenseIsValid: false,
                questionExtensionEditor: null,
                questionExtension: null,
                questionExtensionJson: null,
                questionExtensionHtml: null,
                questionEditor: null,
                question: null,
                questionJson: null,
                questionHtml: null,
                descriptionEditor: null,
                descriptionHtml: null,
                visibility: 1,
                categoryIds: [],
                categories: [],
                searchTerm: '',
                lockDropdown: true,
                debounceSearchCategory: _.debounce(this.searchCategory, 300),
                selectedCategories: [],
                totalCount: 0,
                showDropdown: false,
                licenseId: 0,
                sessionIndex: 0,
                solutionMetadataJson: null,
                licenseConfirmation: false,
                isLearningSession: false,
                currentLearningSessionIndex: null,
                solutionIsValid: false,
                showMore: false,
                disabled: true,
                modalIsVisible: false,
                showQuestionExtension: false,
                showDescription: false,
            }
        },
        mounted() {
            eventBus.$on('open-edit-question-modal',
                e => {
                    var question = {
                        questionId: e.questionId,
                        edit: e.edit,
                        sessionIndex: e.sessionIndex,
                        categoryId: e.categoryId
                    };
                    $('#EditQuestionModal').data('question', question).modal('show');
                });
            $('#EditQuestionModal').on('show.bs.modal',
                event => {
                    this.showQuestionExtension = false;
                    this.showDescription = false;
                    this.modalIsVisible = true;
                    this.loadEditors();
                    this.isLearningSession = $('#hddIsLearningSession').length > 0;
                    this.highlightEmptyFields = false;
                    if (this.visibility == 1)
                        this.licenseIsValid = true;
                    this.id = $('#EditQuestionModal').data('question').questionId;
                    if ($('#EditQuestionModal').data('question').edit) {
                        this.edit = true;
                        this.getQuestionData(this.id);
                        this.currentLearningSessionIndex = $('#hddIsLearningSession').attr('data-current-step-idx');
                        if ($('#EditQuestionModal').data('question').sessionIndex)
                            this.sessionIndex = $('#EditQuestionModal').data('question').sessionIndex;
                        else if (this.currentLearningSessionIndex)
                            this.sessionIndex = this.currentLearningSessionIndex;
                    } else {
                        this.solutionType = 1;
                        let categoryId = $('#EditQuestionModal').data('question').categoryId;
                        this.categoryIds.push(categoryId);
                        var json = { categoryId };
                        var self = this;
                        $.ajax({
                            type: 'post',
                            contentType: "application/json",
                            url: '/Category/GetMiniCategoryItem',
                            data: JSON.stringify(json),
                            success: function (data) {
                                self.selectedCategories.push(data.Category);
                            },
                        });
                        this.edit = false;
                        this.sessionIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
                    }
                });

            $('#EditQuestionModal').on('hidden.bs.modal', () => {
                this.modalIsVisible = false;
                this.destroyEditors();
                Object.assign(this.$data, this.$options.data.apply(this));
            });
        },
        watch: {
            searchTerm(term) {
                if (term.length > 0 && this.lockDropdown == false) {
                    this.showDropdown = true;
                    this.debounceSearchCategory();
                }
                else
                    this.showDropdown = false;
            },
            questionHtml() {
                this.formValidator();
            },
            solutionType() {
                this.solutionIsValid = false;
                this.solutionMetadataJson = null;
                this.highlightEmptyFields = false;
            },
            solutionIsValid() {
                this.formValidator();
            },
            visibility(val) {
                this.formValidator();
            },
            licenseConfirmation() {
                this.formValidator();
            },
        },
        methods: {
            loadEditors() {
                this.questionEditor = new tiptapEditor({
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
                                placeholder: 'Ergänzungen zur Frage zB. Bilder, Code usw.',
                                showOnlyCurrent: true,
                            }),
                            tiptapImage
                        ],
                        editorProps: {
                            handleClick: (view, pos, event) => {
                            },
                            attributes: {
                                id: 'QuestionInputField',
                            }
                        },
                        onUpdate: ({ editor }) => {
                            this.questionExtensionJson = editor.getJSON();
                            this.questionExtensionHtml = editor.getHTML();
                        },
                }),

                this.questionExtensionEditor = new tiptapEditor({
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
                    editorProps: {
                        handleClick: (view, pos, event) => {
                        },
                        attributes: {
                            id: 'QuestionExtensionInputField',
                        }
                    },
                    onUpdate: ({ editor }) => {
                        this.questionJson = editor.getJSON();
                        this.questionHtml = editor.getHTML();
                    },
                });

                this.descriptionEditor = new tiptapEditor({
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
                            placeholder:
                                'Erklärungen, Zusatzinfos, Merkhilfen, Abbildungen, weiterführende Literatur und Links etc.',
                            showOnlyCurrent: true,
                        }),
                        tiptapImage
                    ],
                    onUpdate: ({ editor }) => {
                        this.descriptionHtml = editor.getHTML();
                    },
                });
            },
            destroyEditors() {
                this.questionEditor.destroy();
                this.questionEditor = null;
                this.questionExtensionEditor.destroy();
                this.questionExtensionEditor = null;
                this.descriptionEditor.destroy();
                this.descriptionEditor = null;
            },
            getQuestionData(id) {
                var self = this;
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: '/Question/GetData/' + id,
                    success: function (data) {
                        self.solutionType = data.SolutionType;
                        self.initiateSolution(data.Solution);
                        self.questionHtml = data.Text;
                        self.questionEditor.commands.setContent(data.Text);
                        self.questionExtensionHtml = data.TextExtended;
                        self.questionExtensionEditor.commands.setContent(data.TextExtended);
                        if (data.TextExtended != null && data.TextExtended.length > 0)
                            self.showQuestionExtension = true;
                        self.categoryIds = data.CategoryIds;
                        self.descriptionHtml = data.DescriptionHtml;
                        self.descriptionEditor.commands.setContent(data.DescriptionHtml);
                        if (data.DescriptionHtml != null && data.DescriptionHtml.length > 0)
                            self.showDescription = true;
                        self.selectedCategories = data.Categories;
                        self.licenseId = data.LicenseId;
                        self.solutionMetadataJson = data.SolutionMetadataJson;
                        self.visibility = data.Visibility;
                    },
                });
            },
            getSolution() {
                let solution = "";
                let solutionType = parseInt(this.solutionType);
                switch (solutionType) {
                    case SolutionType.Text: return this.textSolution;
                    case SolutionType.MultipleChoice: solution = this.multipleChoiceJson;
                        break;
                    case SolutionType.MatchList: solution = this.matchListJson;
                        break;
                    case SolutionType.FlashCard: return this.flashCardAnswer;
                }
                
                return JSON.stringify(solution);
            },
            initiateSolution(solution) {
                let solutionType = parseInt(this.solutionType);
                switch (solutionType) {
                    case SolutionType.Text:
                        this.textSolution = solution;
                    break;
                    case SolutionType.MultipleChoice:
                        this.multipleChoiceJson = solution;
                    break;
                    case SolutionType.MatchList:
                        this.matchListJson = solution;
                    break;
                    case SolutionType.FlashCard:
                        this.flashCardJson = solution;
                }

                return solution;
            },
            save() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("EditQuestion");
                    return;
                }
                if (this.disabled) {
                    this.highlightEmptyFields = true;
                    return;
                }
                var url = this.edit ? '/Question/Edit' : '/Question/Create';
                var json = this.getSaveJson();
                var self = this;

                if (this.disabled) {
                    eventBus.$emit('show-missing-fields');
                    return;
                }

                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: url,
                    data: JSON.stringify(json),
                    success: function (result) {
                        if (self.isLearningSession) {
                            var answerBody = new AnswerBody();
                            var skipIndex = this.questions != null ? -5 : 0;

                            answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                                "?skipStepIdx=" +
                                skipIndex +
                                "&index=" +
                                self.sessionIndex);

                            if (!self.edit)
                                eventBus.$emit('add-question-to-list', result);
                            else
                                eventBus.$emit('reload-question-id', result.Id);
                            eventBus.$emit("change-active-question", self.sessionIndex);

                        }
                        else
                            self.reloadAnswerBody(result.Data.Id);

                        self.highlightEmptyFields = false;
                        $('#EditQuestionModal').modal('hide');
                    },
                });
            },

            reloadAnswerBody(id) {
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: '/AnswerQuestion/RenderAnswerBody',
                    data: JSON.stringify({ questionId: id }),
                    success: function (html) {
                        $('#AnswerBody').replaceWith(html);
                        var answerBody = new AnswerBody();
                    },
                });
            },

            getSaveJson() {
                let solution = this.getSolution();
                let solutionType = parseInt(this.solutionType);
                if (this.solutionType == 4 || this.solutionType == 6)
                    solutionType = 1;

                let licenseId = this.getLicenseId();

                var editJson = {
                    QuestionId: this.id,
                }
                var createJson = {
                    AddToWishknowledge: this.addToWishknowledge,
                }

                var jsonExtension = {
                    CategoryIds: this.categoryIds,
                    TextHtml: this.questionHtml,
                    Solution: solution,
                    SolutionType: solutionType,
                    Visibility: this.visibility,
                    SolutionMetadataJson: this.solutionMetadataJson,
                    LicenseId: licenseId,
                    SessionIndex: this.sessionIndex,
                }
                var json = this.edit ? editJson : createJson;

                $.extend(json, jsonExtension);

                return json;
            },

            searchCategory() {
                this.showDropdown = true;
                var self = this;
                var data = {
                    term: self.searchTerm,
                    type: 'Categories'
                };

                $.get("/Api/Search/ByNameForVue", data,
                    function (result) {
                        self.categories = result.categories;
                        self.totalCount = result.totalCount;
                        self.$nextTick(() => {
                            $('[data-toggle="tooltip"]').tooltip();
                        });
                    });
            },

            getLicenseId() {
                if (this.licenseId == 0) {
                    return 1;
                }
                return this.licenseId;
            },

            selectCategory(category) {
                this.showDropdown = false;
                this.lockDropdown = true;
                this.searchTerm = '';

                var index = this.categoryIds.indexOf(category.Id);
                if (index < 0) {
                    this.categoryIds.push(category.Id);
                    this.selectedCategories.push(category);
                }
            },

            removeCategory(data) {
                if (this.selectedCategories.length > 1) {
                    this.selectedCategories.splice(data.index, 1);
                    var categoryIdIndex = this.categoryIds.indexOf(data.categoryId);
                    this.categoryIds.splice(categoryIdIndex, 1);
                }
            },
            formValidator() {
                if (!this.modalIsVisible)
                    return;
                var questionIsValid = this.questionEditor.state.doc.textContent.length > 0;
                var solutionIsValid = this.solutionIsValid;
                this.licenseIsValid = this.licenseConfirmation || this.visibility == 1;

                this.disabled = !questionIsValid || !solutionIsValid || !this.licenseIsValid;
            }
        }
    });
