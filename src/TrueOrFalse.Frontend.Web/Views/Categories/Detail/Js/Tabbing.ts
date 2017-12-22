class Tabbing {

    private _page: CategoryPage ;

    constructor(page: CategoryPage) {

        this._page = page;

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabname = tab.attr('id');
            tab.click((e) =>  {
                e.preventDefault();
                if (tab.hasClass('active')) return;

                if (tab.hasClass('LoggedInOnly') && NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg(tabname);
                    return;
                }

                if (!this.ContentIsPresent(tabname)) {
                    this.RenderTabContent(tabname);
                }
                if (tabname == "LearningTab" && $('#hddLearningSessionStarted').val() == "False") {
                    new PageInit();
                    $('#hddLearningSessionStarted').val("True");
                }
                this.ShowTab(tabname);
            });
        });
    }

    private RenderTabContent(tabName: string): void {
        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._page.CategoryId;

        $.get(url, (html) => {

            $('#' + tabName + 'Content')
                .empty()
                .append(html);

            $("#TabContent .show-tooltip").tooltip();
        });
    }

    private ContentIsPresent(tabName: string): boolean {

        return !($.trim($('#' + tabName + 'Content').html())=='');
    }

    private ShowTab(tabName: string): void {

        $('.Tab').removeClass('active');
        $('#' + tabName).addClass('active');

        $('.TabContent').fadeOut(200);

        $('#' + tabName + "Content").delay(200).fadeIn(200);
    }
}
