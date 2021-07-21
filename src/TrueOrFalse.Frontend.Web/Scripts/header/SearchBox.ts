interface SearchResultItem {
    ResultCount: number;
    Type: string;
    Item: any;

    NoResults: boolean;
    SearchUrl: string;
}

interface ResultItem {
    Id: number;
    Name: string;
    IconHtml: string;
    NumberOfQuestions: number;
    ImageUrl: string;
    ItemUrl: string;

    NoResults: boolean;
}

class SearchBox
{
    _elemInput: JQuery;
     
    constructor(element) {

        var self = this;

        this._elemInput = $(element);; 

        if (this._elemInput.length == 0)
            return;

        var autocomplete = this._elemInput.autocomplete({
            minLength: 0,
            source(request, response) {
                $.get("/Api/Search/ByName?term=" + request.term, data => {
                    if (data.Items.length == 0)
                        response([{ NoResults: true }]);
                    else
                        response(data.Items);
                });
            },
            select(event, ui) {

                self._elemInput.data("set-id", ui.item.Id);
                self._elemInput.val(ui.item.Name);

                return false;
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = <any>function (ul, resultItem: SearchResultItem): any {

            ul.addClass("AutocompleteHeader");

            var html = "";

            if (resultItem.NoResults) {
                html =
                    "<div class='SearchResultItem'>" +
                        "Keine Treffer. Bitte weitertippen <br> oder anderen Suchbegriff verwenden." +
                    "</div>";
            } else {

                switch (resultItem.Type) {

                    case "CategoriesHeader":
                        html = "<div class='SearchResultHeader'>Themen <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Categories":
                        var item = <ResultItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.IconHtml +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;

                    case "QuestionsHeader":
                        html = "<div class='SearchResultHeader'>Fragen <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Questions":
                        item = <ResultItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;

                    case "UsersHeader":
                        html = "<div class='SearchResultHeader'>Nutzer <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Users":
                        item = <ResultItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;
                }
            }

            return $("<li></li>")
                .data("ui-autocomplete-item", resultItem)
                .append(html)
                .appendTo(ul);
        }

    }
}


$(() => {
    new SearchBox("#headerSearchBox");
    new SearchBox("#StickyHeaderSearchBox");
    new SearchBox("#SmallHeaderSearchBox");

    var url;
    $(document).keyup((e: any) => {
        if (e.keyCode !== 13) {
            url = $(".ui-state-focus").children().children().attr("href");
        }

        if (e.keyCode === 13) {
            if (url != undefined) {

                $(location).attr('href', url);
            }
        }
    });

});