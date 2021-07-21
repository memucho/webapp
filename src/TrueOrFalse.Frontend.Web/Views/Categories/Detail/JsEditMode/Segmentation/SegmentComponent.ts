var pinComponent = Vue.component('pin-category-component', {
    template: '#pin-category-template',

    props: {
        categoryId: [Number, String],
    },
    data() {
        return {
            isInWishknowledge: false,
            isLoading: false,
            stateLoad: 'notAdded',
        }
    },
    watch: {
    },
    mounted() {
        this.loadWishknowledge();
    },
    methods: {
        loadWishknowledge() {
            if (NotLoggedIn.Yes())
                return;
            var self = this;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Category/GetWishknowledge',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result == 'True')
                        self.isInWishknowledge = true;
                    else
                        self.isInWishknowledge = false;
                    if (self.isInWishknowledge)
                        self.stateLoad = 'added';
                    else
                        self.stateLoad = 'notAdded';
                },
            });
        },
        addToWishknowledge() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("PinCategory");
                return;
            }

            if (this.stateLoad == 'loading')
                return;
            var self = this;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Api/Category/Pin',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result == 'True')
                        self.isInWishknowledge = true;
                    else
                        self.isInWishknowledge = false;
                    if (self.isInWishknowledge)
                        self.stateLoad = 'added';
                    else
                        self.stateLoad = 'notAdded';

                    self.renderNewKnowledgeBar();
                },
            });
        },
        removeFromWishknowledge() {
            var self = this;
            if (this.stateLoad == 'loading')
                return;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Api/Category/UnPin',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result == 'False')
                        self.isInWishknowledge = true;
                    else
                        self.isInWishknowledge = false;
                    if (self.isInWishknowledge)
                        self.stateLoad = 'added';
                    else
                        self.stateLoad = 'notAdded';

                    $("#JS-RemoveQuestionsCat").attr("data-category-id", self.categoryId);
                    $("#UnpinCategoryModal").modal('show');

                    self.renderNewKnowledgeBar();
                },
            });
        },

        renderNewKnowledgeBar() {
            var self = this;
            $.ajax({
                url: '/Category/RenderNewKnowledgeSummaryBar?categoryId=' + self.categoryId,
                type: 'GET',
                success: data => {
                    $(".category-knowledge-bar[data-category-id='" + self.categoryId + "']").replaceWith(data);
                    $('.show-tooltip').tooltip();
                },
            });

            var parentId = $('#hhdCategoryId').val();
            $.ajax({
                url: '/Category/RenderNewKnowledgeSummaryBar?categoryId=' + parentId,
                type: 'GET',
                success: data => {
                    $(".category-knowledge-bar[data-category-id='" + parentId + "']").replaceWith(data);
                    $('.show-tooltip').tooltip();
                },
            });
        }
    }
});

var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: [String, Number],
        editMode: Boolean,
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
        isMyWorld: Boolean,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
            id: parseInt(this.categoryId),
            isSelected: false,
            checkboxId: '',
            showHover: false,
            imgHtml: null,
            linkToCategory: null,
            visibility: 0,
            categoryTypeHtml: null,
            childCategoryCount: 0,
            questionCount: 0,
            categoryName: null,
            knowledgeBarHtml: null,
        };
    },
    watch: {
        selectedCategories() {
            this.isSelected = this.selectedCategories.includes(this.id);
        },
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        },
        categoryId() {
            this.init();
        }
    },
    created() {
        this.init();
    },
    methods: {
        init() {
            this.getCategoryData();
            this.dropdownId = this.segmentId + '-Dropdown' + this.id;
            this.checkboxId = this.segmentId + '-Checkbox' + this.id;
            if (this.isCustomSegment)
                this.dropdownId += this.$parent.id;
        },
        getCategoryData() {
            var self = this;
            var data = {
                categoryId: parseInt(self.categoryId),
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryData',
                data: JSON.stringify(data),
                success: function (data) {
                    self.imgHtml = data.imgHtml;
                    self.linkToCategory = data.linkToCategory;
                    self.visibility = data.visibility;
                    self.categoryTypeHtml = data.categoryTypeHtml;
                    self.knowledgeBarHtml = data.knowledgeBarHtml;
                    self.questionCount = data.questionCount;
                    self.childCategoryCount = data.childCategoryCount;
                    self.categoryName = data.categoryName;
                    self.$nextTick(() => Images.ReplaceDummyImages());
                },
            });
        },

        thisToSegment() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("MoveToSegment");
                return;
            }
            if (!this.isCustomSegment) {
                this.$parent.loadSegment(this.id);
            }
        },
        selectCategory() {
            if (this.editMode) {
                this.isSelected = this.selectedCategories.includes(this.id);

                if (this.isSelected)
                    this.$emit('unselect-category', this.id);
                else
                    this.$emit('select-category', this.id);
            }
        },
        removeParent() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveParent");
                return;
            }
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveParent',
                data: JSON.stringify(data),
                success: function (data) {
                    if (data.success == true)
                        self.visible = false;
                    else {
                        eventBus.$emit('show-error', data.errorMsg);
                    }
                },
            });
        },
        hideCategory() {
            this.$parent.filterChildren([this.categoryId]);
        },

        handler: function (e) {
            console.log(e);
        }
    }
});

var segmentComponent = Vue.component('segment-component', {
    props: {
        title: String,
        description: String,
        childCategoryIds: String,
        categoryId: [String, Number],
        editMode: Boolean,
        isMyWorld: Boolean,
    },

    data() {
        return {
            categories: [],
            segmentId: null,
            cardsKey: null,
            isCustomSegment: true,
            selectedCategories: [],
            currentChildCategoryIds: [],
            currentChildCategoryIdsString: "",
            hover: false,
            showHover: false,
            addCategoryId: "",
            dropdownId: null,
            timer: null,
            linkToCategory: null,
            visibility: 0,
            segmentTitle: null,
            knowledgeBarHtml: null,
            disabled: true,
        };
    },

    created() {
    },

    mounted() {
        this.getSegmentData();
        this.segmentId = "Segment-" + this.categoryId;
        if (this.childCategoryIds != null) {
            var baseChildCategoryIds = JSON.parse(this.childCategoryIds);
            this.currentChildCategoryIds = baseChildCategoryIds;
        }
        this.addCategoryId = "AddCategoryTo-" + this.segmentId + "-Btn";
        this.dropdownId = this.segmentId + '-Dropdown';

        this.$on('select-category', (id) => this.selectCategory(id));
        this.$on('unselect-category', (id) => this.unselectCategory(id));
        eventBus.$on('add-category-card',
            (e) => {
                if (this.categoryId == e.parentId)
                    this.currentChildCategoryIds.push(e.newCategoryId);
            });
    },

    watch: {
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        },
        currentChildCategoryIds() {
            this.currentChildCategoryIdsString = this.currentChildCategoryIds.join(',');
        },
        selectedCategoryIds(val) {
            this.disabled = val.length <= 0;
        }
    },

    updated() {
    },

    methods: {
        getSegmentData() {
            var self = this;
            var data = {
                categoryId: parseInt(self.categoryId),
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegmentData',
                data: JSON.stringify(data),
                success: function(data) {
                    self.linkToCategory = data.linkToCategory;
                    self.visibility = data.visibility;
                    self.knowledgeBarHtml = data.knowledgeBarHtml;
                    if (self.title)
                        self.segmentTitle = self.title;
                    else self.segmentTitle = data.categoryName;
                },
            });
        },
        selectCategory(id) {
            if (this.selectedCategories.includes(id))
                return;
            else this.selectedCategories.push(id);
        },
        unselectCategory(id) {
            if (this.selectedCategories.includes(id)) {
                var index = this.selectedCategories.indexOf(id);
                this.selectedCategories.splice(index, 1);
            }
        },
        updateCategoryOrder() {
            this.categories = $("#" + this.segmentId + " > .topic").map((idx, elem) => $(elem).attr("category-id")).get();
        },
        removeSegment() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveSegment");
                return;
            }
            eventBus.$emit('remove-segment', parseInt(this.categoryId));
        },
        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#" + self.addCategoryId),
                moveCategories: false,
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        removeChildren() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveChildren");
                return;
            }
            var self = this;
            var data = {
                parentCategoryId: self.categoryId,
                childCategoryIds: self.selectedCategories,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveChildren',
                data: JSON.stringify(data),
                success: function (result) {
                    var removedChildCategoryIds = JSON.parse(result.removedChildCategoryIds);
                    self.filterChildren(removedChildCategoryIds);
                },
            });
        },
        filterChildren(selectedCategoryIds) {
            console.log(selectedCategoryIds);
            let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
                function (e) {
                    return this.indexOf(e) < 0;
                },
                selectedCategoryIds
            );
            this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
            this.selectedCategoryIds = [];
            eventBus.$emit('save-segments');
        },
        hideChildren() {
            this.filterChildren(this.selectedCategories);
        }
    },
});