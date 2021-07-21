////interface ResultItem {
////    ResultCount: Number,
////    Type: String, 
////    Item: Object,
////}

var addCategoryComponent = Vue.component('add-category-component', {
    data() {
        return {
            name: "",
            errorMsg: "",
            parentId: null,
            existingCategoryName: "",
            existingCategoryUrl: "",
            showErrorMsg: false,
            disabled: false,
            addCategoryBtnId: null,
            disableAddCategory: true,
            selectedCategories: [],
            moveCategories: false,
            createCategory: true,
            categories: [],
            searchTerm: "",
            totalCount: 0,
            selectedCategoryId: 0,
            debounceSearchCategory: _.debounce(this.searchCategory, 300),
            showDropdown: false,
            lockDropdown: true,
            selectedCategory: null,
            showSelectedCategory: false,
        };
    },
    watch: {
        name(val) {
            if (val.length <= 0)
                this.disableAddCategory = true;
            else
                this.disableAddCategory = false;
        },
        searchTerm(term) {
            if (term.length > 0 && this.lockDropdown == false) {
                this.showDropdown = true;
                this.debounceSearchCategory();
            }
            else
                this.showDropdown = false;
        },
        selectedCategoryId(id) {
            if (id > 0 && !this.createCategory)
                this.disableAddCategory = false;
        },
        createCategory() {
            if (this.selectedCategory != null)
                this.showSelectedCategory = true;
        }
    },
    mounted() {
        eventBus.$on('open-add-category-modal',
            id => {
                var parent = {
                    id: id,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    moveCategories: false,
                    redirect: true,
                }
                if ($('#LearningTabWithOptions').hasClass("active"))
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
            });
        $('#AddCategoryModal').on('show.bs.modal',
            event => {
                this.parentId = $('#AddCategoryModal').data('parent').id;
                this.addCategoryBtnId = $('#AddCategoryModal').data('parent').addCategoryBtnId;
                this.moveCategories = $('#AddCategoryModal').data('parent').moveCategories;
                if (this.moveCategories)
                    this.selectedCategories = $('#AddCategoryModal').data('parent').selectedCategories;
                if ($('#AddCategoryModal').data('parent').redirect != null &&
                    $('#AddCategoryModal').data('parent').redirect)
                    this.redirect = true;
            });

        $('#AddCategoryModal').on('hidden.bs.modal',
            event => {
                Object.assign(this.$data, this.$options.data.apply(this));
            });
    },
    methods: {
        clearData() {
            this.name = "";
            this.errorMsg = "";
            this.showErrorMsg = false;
            this.parentId = null;
            this.existingCategoryName = "";
            this.existingCategoryUrl = "";
            this.selectedCategories = [];
            this.moveCategories = false;
            this.redirect = false;
            this.showDropdown = false;
            this.selectedCategoryId = 0;
        },
        closeModal() {
            $('#AddCategoryModal').modal('hide');
        },

        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            Utils.ShowSpinner();
            var self = this;
            var url;
            var categoryData;
            if (this.moveCategories) {
                categoryData = {
                    name: self.name,
                    parentCategoryId: self.parentId,
                    childCategoryIds: self.selectedCategories
                }
                url = '/EditCategory/QuickCreateWithCategories';

            } else {
                categoryData = {
                    name: self.name,
                    parentCategoryId: self.parentId,
                }
                url = '/EditCategory/QuickCreate';
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/ValidateName',
                data: JSON.stringify({ name: self.name }),
                success: function (data) {
                    if (data.categoryNameAllowed) {
                        $.ajax({
                            type: 'Post',
                            contentType: "application/json",
                            url: url,
                            data: JSON.stringify(categoryData),
                            success: function (data) {
                                if (data.success) {
                                    if (self.redirect)
                                        window.open(data.url, '_self');
                                    if (self.addCategoryBtnId != null)
                                        self.loadCategoryCard(data.id);
                                    if (self.moveCategories) {
                                        eventBus.$emit('remove-category-cards', data.movedCategories);
                                    }
                                    else
                                        $('#AddCategoryModal').modal('hide');
                                        Utils.HideSpinner();
                                }
                            },
                        });
                    } else {
                        self.errorMsg = data.errorMsg;
                        self.existingCategoryName = data.name;
                        self.existingCategoryUrl = data.url;
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
                    };
                },
            });
        },
        loadCategoryCard(id) {
            var data = {
                parentId: this.parentId,
                newCategoryId: id
            };
            eventBus.$emit('add-category-card', data);
        },
        selectCategory(category) {
            this.showDropdown = false;
            this.lockDropdown = true;
            this.searchTerm = category.Name;
            this.selectedCategory = category;
            this.selectedCategoryId = category.Id;
            this.showSelectedCategory = true;
        },
        toggleShowSelectedCategory() {
            this.showSelectedCategory = false;
            this.$nextTick(() => {
                this.$refs.searchInput.focus();
            });
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
                    self.categories = result.categories.filter(c => c.Id != self.parentId);
                    self.totalCount = result.totalCount;
                    self.$nextTick(() => {
                        $('[data-toggle="tooltip"]').tooltip();
                    });
                });
        },
        addExistingCategory() {
            Utils.ShowSpinner();
            var self = this;
            var categoryData = {
                childCategoryId: self.selectedCategoryId,
                parentCategoryId: self.parentId,
            }

            if (this.selectedCategoryId == this.parentId) {
                this.errorMsg = "Das untergeordnete Thema, darf nicht das selbe Thema sein";
                this.showErrorMsg = true;
                Utils.HideSpinner();
                return;
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/AddChild',
                data: JSON.stringify(categoryData),
                success: function (data) {
                    if (data.success) {
                        if (self.redirect)
                            window.open(data.url, '_self');
                        if (self.addCategoryBtnId != null)
                            self.loadCategoryCard(data.id);
                        if (self.moveCategories) {
                            eventBus.$emit('remove-category-cards', data.movedCategories);
                        }
                        else
                            $('#AddCategoryModal').modal('hide');
                        Utils.HideSpinner();
                    } else {
                        self.errorMsg = data.errorMsg;
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
                    };
                },
            });
        }
    }
});


var AddCategoryApp = new Vue({
    el: '#AddCategoryApp',
});