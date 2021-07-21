interface QuestionListItem
{
    CorrectnessProbability: Number;
    HasPersonalAnswer: Boolean;
    Id: Number;
    ImageData: String;
    IsInWishknowledge: Boolean;
    LearningSessionStepCount: Number;
    LinkToComment: String;
    LinkToDeleteQuestion: String;
    LinkToEditQuestion: String;
    LinkToQuestion: String;
    LinkToQuestionDetailSite: String;
    LinkToQuestionVersions: String;
    Title: String;
    Visibility: Number;
}

let qlc = Vue.component('question-list-component', {
    components: {
        VueSlider: window['vue-slider-component']
    },
    props: [
        'categoryId',
        'isAdmin',
        'isQuestionListToShow',
        'activeQuestionId',
        'selectedPageFromActiveQuestion'],
    data() {
        return {
            pages: 0,
            selectedPage: 1,
            itemCountPerPage: 25,
            questions: [] as QuestionListItem[],
            hasQuestions: false,
            showFirstPage: true,
            pageArray: [] as Number[],
            questionText: "Fragen",
            showLeftPageSelector: false,
            showRightPageSelector: false,
            leftSelectorArray: [] as Number[],
            rightSelectorArray: [] as Number[],
            centerArray: [] as Number[],
            showLeftSelectionDropUp: false,
            showRightSelectionDropUp: false,
            pageIsLoading: false,
            lastQuestionInListIndex: null,
            answerBodyHasLoaded: false,
        };
    },
    created() {
        eventBus.$on('reload-knowledge-state', () => this.loadQuestions(this.selectedPage));
        eventBus.$on('reload-wishknowledge-state-per-question', (data) => this.changeQuestionWishknowledgeState(data.questionId, data.isInWishknowledge));
        eventBus.$on('reload-correctnessprobability-for-question', (id) => this.getUpdatedCorrectnessProbability(id));
        eventBus.$on('load-questions-list', () => { this.initQuestionList()});
        eventBus.$on('add-question-to-list', (q: QuestionListItem) => { this.addQuestionToList(q)});
        eventBus.$on('answerbody-loaded', () => {
            if (this.answerBodyHasLoaded)
                return;
            else
                this.initQuestionList();
            this.answerBodyHasLoaded = true;
        });
    },
    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
    },
    watch: {
        itemCountPerPage: function (val) {
            this.pages = Math.ceil(this.allQuestionCount / val);
        },
        selectedPageFromActiveQuestion: function(val) {
            this.selectedPage = val;
        },
        questions: function() {
            if (this.questions.length > 0)
                this.hasQuestions = true;
            if (this.questions.length == 1)
                this.questionText = "Frage";
        },
        selectedPage: function(val) {
            this.loadQuestions(val);
        },
        pages: function (val) {
            let newArray = [];

            let startNumber = 1;
            for (let i = 0; startNumber < val + 1; i++) {
                newArray.push(startNumber);
                startNumber = startNumber + 1;
            }
            this.pageArray = newArray;
        },
        leftSelectorArray: function() {
            if (this.leftSelectorArray.length >= 2) {
                this.showLeftPageSelector = true;
            }
            else
                this.showLeftPageSelector = false;
        },
        rightSelectorArray: function () {
            if (this.rightSelectorArray.length >= 2) {
                this.showRightPageSelector = true;
            }
            else
                this.showRightPageSelector = false;
        }
    },
    methods: {
        initQuestionList() {
            this.loadQuestions(this.selectedPage);
        },
        loadQuestions(selectedPage) {
            this.pageIsLoading = true;
            $.ajax({
                url: "/QuestionList/LoadQuestions/",
                data: {
                    itemCountPerPage: this.itemCountPerPage,
                    pageNumber: selectedPage,
                },
                type: "POST",
                success: questions => {
                    this.questions = questions;
                    this.updatePageCount(selectedPage);
                },
            });
        },
        updatePageCount(selectedPage) {
            this.selectedPage = selectedPage;
            this.showLeftSelectionDropUp = false;
            this.showRightSelectionDropUp = false;
            if (typeof this.questions[0] != "undefined")
                this.pages = Math.ceil(this.questions[0].LearningSessionStepCount / this.itemCountPerPage);
            else
                this.pages = 1;
            this.$nextTick(function () {
                this.setPaginationRanges(selectedPage);
                new Pin(PinType.Question);
            });
            this.pageIsLoading = false;
        },
        setPaginationRanges(selectedPage) {
            if ((selectedPage - 2) <= 2) {
                this.hideLeftPageSelector = true;
            };
            if ((selectedPage + 2) >= this.pageArray.length) {
                this.hideRightPageSelector = true;
            };

            let leftArray = [];
            let centerArray = [];
            let rightArray = [];

            if (this.pageArray.length >= 8) {

                centerArray = _.range(selectedPage - 2, selectedPage + 3);
                centerArray = centerArray.filter(e => e >= 2 && e <= this.pageArray.length - 1);

                leftArray = _.range(2, centerArray[0]);
                rightArray = _.range(centerArray[centerArray.length - 1] + 1, this.pageArray.length);

                this.centerArray = centerArray;
                this.leftSelectorArray = leftArray;
                this.rightSelectorArray = rightArray;    

            } else {
                this.centerArray = this.pageArray;
            };
        },
        loadPreviousQuestions() {
            if (this.selectedPage != 1)
                this.loadQuestions(this.selectedPage - 1);
        },
        loadNextQuestions() {
            if (this.selectedPage != this.pageArray.length)
                this.loadQuestions(this.selectedPage + 1);
        },
        changeQuestionWishknowledgeState(questionId, isInWishknowledge) {
            for (var q in this.questions) {
                if (this.questions[q].Id == questionId) {
                    this.questions[q].IsInWishknowledge = isInWishknowledge;
                    break;
                }
            }
        },
        getUpdatedCorrectnessProbability(id) {
            $.ajax({
                url: "/QuestionList/GetUpdatedCorrectnessProbability/",
                data: { questionId: id },
                type: "Post",
                success: correctnessProbability => {
                    for (var q in this.questions) {
                        if (this.questions[q].Id == id) {
                            this.questions[q].CorrectnessProbability = correctnessProbability;
                            break;
                        }
                    }
                }
            });
        },
        addQuestionToList(q: QuestionListItem) {
            if (this.questions.length <= 0)
                this.renderNewSessionBar(q.Id);
            this.questions.push(q);
        },
        renderNewSessionBar(id) {
            $.ajax({
                url: "/QuestionList/RenderSessionHeaderWithQuestionId/",
                data: { questionId: id, categoryId: this.categoryId },
                type: "Post",
                success: data => {
                    $('#NoQuestionsSessionBar').replaceWith(data.html);
                }
            });
        }
    },
});
