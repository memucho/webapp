﻿interface Segment {
    CategoryId: Number,
    Title: String,
    ChildCategoryIds: Array<Number>,
};


var segmentationComponent = Vue.component('segmentation-component', {
    props: {
        categoryId: Number,
        editMode: Boolean,
    },

    data() {
        return {
            baseCategoryList: [],
            customCategoryList: [] as Segment[],
            componentKey: 0,
            selectedCategoryId: null,
            isCustomSegment: false,
            hasCustomSegment: false,
            selectedCategories: [],

        };
    },

    created() {
    },

    mounted() {
        this.hasCustomSegment = $('#CustomSegmentSection').html().length > 0;
        this.$on('remove-segment', id => this.addCategoryToBaseList(id));
    },

    watch: {
    },

    updated() {
    },

    methods: {
        loadSegment(id) {
            var self = this;
            var currentElement = $("#CustomSegmentSection");
            var data = { CategoryId: id }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegmentHtml',
                data: JSON.stringify(data),
                success: function(data) {
                    if (data) {
                        self.hasCustomSegment = true;
                        var inserted = currentElement.append(data.html);
                        var instance = new segmentComponent({
                            el: inserted.get(0)
                        });
                        self.$refs['card' + id].visible = false;
                    } else {
                    };
                },
            });
        },
        addCategoryToBaseList(categoryId) {
            var self = this;
            var currentElement = $("#GeneratedSegmentSection > .topicNavigation");

            $.ajax({
                type: 'Get',
                contentType: "application/int",
                url: '/Segmentation/GetCategoryCard',
                data: categoryId,
                success: function (data) {
                    if (data) {
                        var inserted = currentElement.append(data.html);
                        var instance = new categoryCardComponent({
                            el: inserted.get(0)
                        });
                    } else {

                    };
                },
            });
        },
    },
});