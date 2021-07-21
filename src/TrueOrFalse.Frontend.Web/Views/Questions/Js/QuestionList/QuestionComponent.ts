let qc = Vue.component('question-component', {
    props: [
        'questionId',
        'questionTitle',
        'questionImage',
        'knowledgeState',
        'isInWishknowledge',
        'url',
        'hasPersonalAnswer',
        'isAdmin',
        'selectedPage',
        'isQuestionListToShow',
        'questionIndex',
        'activeQuestionId',
        'selectedPageFromActiveQuestion',
        'lengthOfQuestionsArray',
        'questionLinkToComment',
        'linkToEditQuestion',
        'linkToQuestionVersions',
        'linkToQuestion',
        'isLastItem',
        'sessionIndex',
        'visibility'
    ],
    data() {
        return {
            answer: "",
            extendedAnswer: "",
            categories: [],
            references: [],
            author: "",
            authorId: "",
            authorImage: "",
            allDataLoaded: false,
            backgroundColor: "",
            correctnessProbability: "",
            correctnessProbabilityLabel: "",
            showFullQuestion: false,
            commentCount: 0,
            extendedQuestion: "",
            isLoggedIn: IsLoggedIn.Yes,
            pinId: "QuestionListPin-" + this.questionId,
            questionTitleId: "#QuestionTitle-" + this.questionId,
            extendedQuestionId: "#eqId-" + this.questionId,
            answerId: "#aId-" + this.questionId,
            extendedAnswerId: "#eaId-" + this.questionId,
            questionDetailsId: "QuestionDetails-" + this.questionId,
            showQuestionMenu: false,
            isCreator: false,
            editUrl: "",
            historyUrl: "",
            topicTitle: "Thema",
            authorUrl: "",
            questionDetails: "",
            pageHasChanged: false,
            answerCount: "0",
            correctAnswers: "0",
            wrongAnswers: "0",
            questionTitleHtml: "<div class='body-m bold margin-bottom-0'>" + this.questionTitle + "</div>",
            highlightedHtml: "",
            canBeEdited: false,
        }
    },
    mounted() {
        this.correctnessProbability = this.knowledgeState + "%";
        this.setKnowledgebarData(this.knowledgeState);
        this.getWishknowledgePinButton();
        new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionRow);
        if (this.isLastItem)
            this.$parent.lastQuestionInListIndex = this.questionIndex;
    },
    watch: {
        isQuestionListToShow(val) {
            if (val != this.showFullQuestion)
                this.expandQuestion();
        },
        knowledgeState(val) {
            this.setKnowledgebarData(val);
            this.correctnessProbability = this.knowledgeState + "%";
            this.loadQuestionBody();
        },
        selectedPage() {
            this.showFullQuestion = false;
            if (this.isInWishknowledge) {
                $("#" + this.pinId + " .iAddedNot").addClass("hide2");
                $("#" + this.pinId + " .iAdded").removeClass("hide2");
            } else {
                $("#" + this.pinId + " .iAdded").addClass("hide2");
                $("#" + this.pinId + " .iAddedNot").removeClass("hide2");
            }
        },
        isInWishknowledge() {
            this.setKnowledgebarData(this.knowledgeState);
        },
        categories() {
            if (this.categories.length >= 2)
                this.topicTitle = "Themen";
            else
                this.topicTitle = "Thema";
        },
        questionId() {
            this.allDataLoaded = false;
            this.pinId = "QuestionListPin-" + this.questionId;
            this.questionTitleId = "#QuestionTitle-" + this.questionId;
            this.questionDetailsId = "QuestionDetails-" + this.questionId;
        },
    },
    methods: {
        abbreviateNumber(val) {
            var newVal;
            if (val < 1000000) {
                return val.toLocaleString("de-DE");
            }
            else if (val >= 1000000 && val < 1000000000) {
                newVal = val / 1000000;
                return newVal.toFixed(2).toLocaleString("de-DE") + " Mio.";
            }
        },
        setKnowledgebarData(val) {
            if (this.isInWishknowledge) {
                if (this.hasPersonalAnswer) {
                    if (val >= 80) {
                        this.backgroundColor = "solid";
                        this.correctnessProbabilityLabel = "Sicheres Wissen";
                    } else if (val < 80 && val >= 50) {
                        this.backgroundColor = "shouldConsolidate";
                        this.correctnessProbabilityLabel = "Zu festigen";
                    } else if (val < 50 && val >= 0) {
                        this.backgroundColor = "shouldLearn";
                        this.correctnessProbabilityLabel = "Zu lernen";
                    }
                } else {
                    this.backgroundColor = "inWishknowledge";
                    this.correctnessProbabilityLabel = "Nicht gelernt";
                }
            } else {
                this.backgroundColor = "";
                this.correctnessProbabilityLabel = "Nicht im Wunschwissen";
            }

        },
        expandQuestion() {
            this.showFullQuestion = !this.showFullQuestion;
            if (this.allDataLoaded == false) {
                this.loadQuestionBody();
            };
        },
        highlightCode(elementId) {
            document.getElementById(elementId).querySelectorAll('code').forEach(block => {
                block.innerHTML = Utils.GetHighlightedCode(block.textContent);
            });
        },
        loadQuestionBody() {
            $.ajax({
                url: "/QuestionList/LoadQuestionBody/",
                data: { questionId: this.questionId },
                type: "POST",
                success: data => {
                    if (data.answer == null || data.answer.length <= 0) {
                        if (data.extendedAnswer && data.extendedAnswer > 0)
                            this.answer = "<div>" + data.extendedAnswer + "</div>";
                        else
                            this.answer = "<div> Fehler: Keine Antwort! </div>";
                    } else {
                        this.answer = "<div>" + data.answer + "</div>";;
                        if (data.extendedAnswer != null)
                            this.extendedAnswer = "<div>" + data.extendedAnswer + "</div>";;
                    };
                    if (data.categories) {
                        this.categories = data.categories;
                        this.linkToFirstCategory = data.categories[0].linkToCategory;
                    };
                    this.references = data.references;
                    this.author = data.author;
                    this.authorImage = data.authorImage;
                    this.extendedQuestion = data.extendedQuestion;
                    this.commentCount = data.commentCount;
                    this.isCreator = data.isCreator && this.isLoggedIn;
                    this.editUrl = data.editUrl;
                    this.historyUrl = data.historyUrl;
                    this.authorUrl = data.authorUrl;
                    this.$nextTick(function () {
                        Images.Init();
                    });
                    this.allDataLoaded = true;
                    this.answerCount = this.abbreviateNumber(data.answerCount);
                    this.correctAnswers = this.abbreviateNumber(data.correctAnswerCount);
                    this.wrongAnswers = this.abbreviateNumber(data.wrongAnswerCount);
                    this.canBeEdited = data.canBeEdited;
                },
            });
        },
        getWishknowledgePinButton() {
            var pinId = "#" + this.pinId;
            $.ajax({
                url: "/QuestionList/RenderWishknowledgePinButton/",
                data: {
                    isInWishknowledge: this.isInWishknowledge
                },
                type: "POST",
                success: partialView => {
                    $(pinId).html(partialView);
                }
            });
        },
        getEditUrl() {
            $.ajax({
                url: "/QuestionList/GetEditUrl/",
                data: {
                    isInWishknowledge: this.isInWishknowledge,
                },
                type: "POST",
                success: url => {
                    this.editUrl = url;
                }
            });
        },
        loadSpecificQuestion: function () {
            var answerBody = new AnswerBody();
            answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                "?skipStepIdx=-5" +
                "&index=" + this.sessionIndex);

            eventBus.$emit('change-active-page', this.selectedPage);
            eventBus.$emit('change-active-question', this.questionIndex);
            eventBus.$emit('update-progress-bar', this.lengthOfQuestionsArray, this.sessionIndex);
        },
        editQuestion() {
            var question = {
                questionId: this.questionId,
                edit: true,
                sessionIndex: this.sessionIndex
            };

            eventBus.$emit('open-edit-question-modal', question);
        }
    },
});