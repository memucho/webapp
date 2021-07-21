

var deleteCategoryComponent = Vue.component('delete-category-component', {
    data() {
        return {
            categoryName: '',
            categoryId: 0,
            showErrorMsg: false,
            errorMsg: '',
        };
    },
    watch: {
    },
    mounted() {
        eventBus.$on('open-delete-category-modal',
            id => {
                this.categoryId = id;
                this.loadCategoryData();
            });
        $('#DeleteCategoryModal').on('show.bs.modal',
            event => {
            });

        $('#DeleteCategoryModal').on('hidden.bs.modal',
            event => {
            });
    },
    methods: {
        loadCategoryData() {
            var self = this;
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Categories/GetDeleteData',
                data: JSON.stringify({id: this.categoryId}),
                success: function (data) {
                    self.categoryName = data.CategoryName;
                    if (data.HasChildren)
                        eventBus.$emit('show-error', 'Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind. Bitte entferne alle Unterthemen und versuche es erneut.');
                    else
                        $('#DeleteCategoryModal').modal('show');
                },
            });
        },
        deleteCategory() {
            var self = this;
            $.ajax({
                type: 'POST',
                url: "/Categories/Delete/" + this.categoryId,
                cache: false,
                success: function () {
                    self.closeModal();
                    window.location.href = window.location.origin;
                },
                error: function (result) {
                    window.console.log(result);
                }
            });
        },
        clearData() {
        },
        closeModal() {
            $('#DeleteCategoryModal').modal('hide');
        },
    }
});