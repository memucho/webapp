declare var Vue: any;
declare var VueSelect: any;
declare var Sticky: any;

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

new Vue({
    el: '#ContentModuleApp',
    data() {
        return {
            saveSuccess: false,
            saveMessage: '',
            editMode: true,
            showTopAlert: false,
            changedContent: false,
            footerIsVisible: '',
            fabIsOpen: false,
            categoryId: null,
            content: null,
            json: null,
            nameIsValid: true,
            errorMsg: '',
            debounceSaveContent: _.debounce(this.saveContent, 400),
            debounceSaveSegments: _.debounce(this.saveSegments, 400)
        };
    },

    created() {
        var self = this;
        self.loadBootstrapTooltips();
        self.categoryId = parseInt($("#hhdCategoryId").val());
        eventBus.$on("set-edit-mode",
            (state) => {
                this.editMode = state;
                if (this.changedContent && !this.editMode) {
                    eventBus.$emit('cancel-edit-mode');
                }
            });
        window.addEventListener('scroll', this.footerCheck);
        window.addEventListener('resize', this.footerCheck);
        eventBus.$on('content-change',
            () => {
                if (this.editMode) {
                    this.changedContent = true;
                }
            });
        eventBus.$on('request-save', () => this.debounceSaveContent());
        eventBus.$on('save-segments', () => this.debounceSaveSegments());
    },
    destroyed() {
        window.removeEventListener('scroll', this.handleScroll);
        window.removeEventListener('resize', this.footerCheck);
    },

    mounted() {
        this.changedContent = false;
        if ((this.$el.clientHeight + 450) < window.innerHeight)
            this.footerIsVisible = true;
        eventBus.$emit('content-is-ready');
        eventBus.$on('name-is-valid', (data) => {
            this.nameIsValid = data.isValid;
            if (!data.isValid)
                this.errorMsg = data.msg;
        });
    },

    updated() {
        this.footerCheck();
    },

    methods: {

        updateAuthors() {
            var self = this;
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/GetAuthorsForHeader',
                data: JSON.stringify({
                    categoryId: this.categoryId
                }),
                success: function (data) {
                    $('#Authors').replaceWith(data.html);
                    self.loadBootstrapTooltips();
                },
            });
        },

        loadBootstrapTooltips() {
            $(function() {
                $('[data-toggle="tooltip"]').tooltip();
            });
        },

        footerCheck() {
            const elFooter = document.getElementById('CategoryFooter');

            if (elFooter) {
                var rect = elFooter.getBoundingClientRect();
                var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                if (rect.top - viewHeight >= 0 || rect.bottom < 0)
                    this.footerIsVisible = false;
                else
                    this.footerIsVisible = true;
            };
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        saveContent() {
            if (NotLoggedIn.Yes()) {
                return;
            }
            var self = this;
            if (!this.nameIsValid) {
                eventBus.$emit('save-msg', self.errorMsg);
                return;
            }

            var data = {
                categoryId: self.categoryId,
                content: self.content,
            }
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Category/SaveCategoryContent',
                data: JSON.stringify(data),
                success: function (success) {
                    if (success == true) {
                        self.saveSuccess = true;
                        self.saveMessage = "Das Thema wurde gespeichert.";
                        eventBus.$emit('save-success');
                        self.updateAuthors();
                    } else {
                        self.saveSuccess = false;
                        self.saveMessage = "Das Thema konnte nicht gespeichert werden.";
                    };
                    eventBus.$emit('save-msg', self.saveMessage);
                },
            });
        },

        saveSegments() {
            if (NotLoggedIn.Yes()) {
                return;
            }
            var self = this;
            var segmentation = [];

            $("#CustomSegmentSection > .segment").each((index, el) => {

                var segment;

                if ($(el).attr('data-child-category-ids').length > 0) {
                    segment = {
                        CategoryId: $(el).data('category-id'),
                        ChildCategoryIds: $(el).attr('data-child-category-ids')
                    }
                }

                else
                    segment = {
                        CategoryId: $(el).data('category-id'),
                    }

                segmentation.push(segment);
            });

            var data = {
                categoryId: self.categoryId,
                segmentation: segmentation
            }
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Category/SaveSegments',
                data: JSON.stringify(data),
                success: function(success) {
                    if (success == true) {
                        this.saveSuccess = true;
                        this.saveMessage = "Das Thema wurde gespeichert.";
                    } else {
                        this.saveSuccess = false;
                        this.saveMessage = "Das Speichern schlug fehl.";
                    };
                },
            })
        }
    },
});