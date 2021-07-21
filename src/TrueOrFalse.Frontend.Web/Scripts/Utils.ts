var ResponsiveBootstrapToolkit: any;

class Utils
{
    static UIMessageHtml(text: string, type: string): string {
        var cssClass = "info";

        if (type === "danger"
            || type === "warning"
            || type === "success") {
            cssClass = type;
        }

        return "<div class='alert alert-" + cssClass
            + " fade in'><a class='close' data-dismiss='alert' href='#'>×</a>" + text + "</div>";
    }

    static Random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number
    {
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    }

    static SetElementValue(selector: string, newValue: string) {
        Utils.SetElementValue2($(selector), newValue);
    }

    static SetElementValue2(elements: JQuery, newValue: string) {
        elements.text(newValue);
        Utils.Hightlight(elements);
    }

    static Hightlight(elements: JQuery) : JQuery {
        elements
            .animate({ opacity: 0.25 }, 100)
            .animate({ opacity: 1.00 }, 800);

        return elements;
    }

    static SetMenuPins(newAmount:number = -1) {
        if (newAmount != -1)
            Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
        else {
            $.get("/Knowledge/GetNumberOfWishknowledgeQuestions/",
                function (htmlResult) {
                    if (htmlResult != -1)
                        Utils.SetElementValue("#menuWishKnowledgeCount", htmlResult);
                });
             
        }
    }

    static MenuPinsPluseOne() {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html()); newAmount += 1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    }

    static MenuPinsMinusOne() {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html()); newAmount += -1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    }

    static PinQuestionsInSetDetail() {
        $(".question-row").find(".iAdded").show();
        $(".question-row").find(".iAddedNot").hide();
    }

    static UnpinQuestionsInSetDetail() {
        $(".question-row").find(".iAdded").hide();
        $(".question-row").find(".iAddedNot").show();
    }

    static DisplayBreakpointOnResize() {
        $(window).resize(() => {
            Utils.DisplayBreakpoint();
        });
    }

    static DisplayBreakpoint() {
        if ($(window).width() < 768) {
            window.console.log("xs " + $(window).width());
        }
        else if ($(window).width() >= 768 && $(window).width() <= 992) {
            window.console.log("sm " + $(window).width());
        }
        else if ($(window).width() > 992 && $(window).width() <= 1200) {
            window.console.log("md " + $(window).width());
        }
        else {
            window.console.log("lg  " + $(window).width());
        }
    }

    static IsScrolledIntoView(elem) {
        var $elem = $(elem);
        var $window = $(window);

        var docViewTop = $window.scrollTop();
        var docViewBottom = docViewTop + $window.height();

        var elemTop = $elem.offset().top;
        var elemBottom = elemTop + $elem.height();

        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    }

    static IsInWidget() {
        return $("#IsWidget").length > 0;
    }

    //http://stackoverflow.com/questions/979975/how-to-get-the-value-from-the-get-parameters
    static GetQueryString(): any {
        return (() => {
            // This function is anonymous, is executed immediately and 
            // the return value is assigned to QueryString!
            var query_string = {};
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                // If first entry with this name
                if (typeof query_string[pair[0]] === "undefined") {
                    query_string[pair[0]] = decodeURIComponent(pair[1]);
                    // If second entry with this name
                } else if (typeof query_string[pair[0]] === "string") {
                    var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
                    query_string[pair[0]] = arr;
                    // If third or later entry with this name
                } else {
                    query_string[pair[0]].push(decodeURIComponent(pair[1]));
                }
            }
            return query_string;
        })();        
    }

    static GetHost() {
        var protocol = location.protocol;
        var slashes = protocol.concat("//");
        return slashes.concat(window.location.hostname);        
    }

    static ShowSpinner() {
        $(".spinner").show();
    }

    static HideSpinner() {
        $(".spinner").hide();
    }

    static ConvertEncodedHtmlToJson(encodedHtml: string) {

        var decodeEntities = (function() {
            function decodeHtml(html) {
                var txt = document.createElement("textarea");
                txt.innerHTML = html;
                return txt.value.replace('[[', '').replace(']]', '');
            }

            return decodeHtml;
        })();

        var decodedHtml = decodeEntities(encodedHtml);

        return JSON.parse(decodedHtml);
    }

    static ConvertJsonToMarkdown(json: Object) {
        var jsonString = JSON.stringify(json);
        var encodedHtml = String(jsonString).replace(/"/g, '&quot;');

        return '[[' + encodedHtml + ']]';
    }

    static GetHighlightedCode(oldHtml: string) {
        var hastNode = {
            type: 'root',
            data: { language: null, relevance: null },
            children: [
            ]
        }
        var lowlightTree = lowlight.highlightAuto(oldHtml);

        hastNode.data.language = lowlightTree.language;
        hastNode.data.relevance = lowlightTree.relevance;
        hastNode.children = lowlightTree.value;
        var newHtml = toHtml(hastNode);

        return newHtml;
    }
}

