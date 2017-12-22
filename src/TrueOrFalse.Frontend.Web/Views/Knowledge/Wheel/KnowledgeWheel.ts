class KnowledgeWheel {

    static ReloadSet() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForSet/?setId=" + $("#hhdSetId").val());
    }

    static ReloadCategory() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForCategory/?categoryId=" + $("#hhdCategoryId").val());
    }

    private static Reload(url: string) {
        $.get(url, (html) => {

            $("#knowledgeWheelContainer")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            drawKnowledgeChart("chartKnowledge");
        });        
    }
}

class KnowledgeBar {
    public static ReloadCategory() {
        KnowledgeBar.Reload("/Category/KnowledgeBar/?categoryId=" + $("#hhdCategoryId").val());
        KnowledgeBar.ReloadForTopicNavs();
    }

    private static Reload(url: string) {
        $.get(url, (html) => {

            $("#CategoryHeader .KnowledgeBarWrapper")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            $("#CategoryHeader .KnowledgeBarWrapper .show-tooltip").tooltip();
        });
    }

    private static ReloadForTopicNavs() {
        $('#TopicTabContent .KnowledgeBarWrapper').each(function () {
            var categoryId = $(this).find('.category-knowledge-bar').attr('data-category-id');
            var setId = $(this).find('.set-knowledge-bar').attr('data-set-id');

            if (categoryId == undefined && setId == undefined)
                throw "no category id or set id found";

            if (setId == undefined)
                $.get("/Category/KnowledgeBar/?categoryId=" + categoryId,
                    (html) => {

                        $(this)
                            .empty()
                            .animate({ opacity: 0.00 }, 0)
                            .append(html)
                            .animate({ opacity: 1.00 }, 400);

                        $(this).find('.show-tooltip').tooltip();
                    });

            if (categoryId == undefined)
                $.get("/Set/KnowledgeBar/?setId=" + setId,
                    (html) => {
                        $(this)
                            .empty()
                            .animate({ opacity: 0.00 }, 0)
                            .append(html)
                            .animate({ opacity: 1.00 }, 400);

                        $(this).find('.show-tooltip').tooltip();
                    });
        });
    }
}