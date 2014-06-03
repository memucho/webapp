var Utils = (function () {
    function Utils() {
    }
    Utils.Random = function (minVal, maxVal, floatVal) {
        if (typeof floatVal === "undefined") { floatVal = 'undefined'; }
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return (typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    };

    Utils.SetElementValue = function (selector, newValue) {
        $(selector).text(newValue).animate({ opacity: 0.25 }, 100).animate({ opacity: 1.00 }, 800);
    };

    Utils.SetMenuPins = function (newAmount) {
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount);
    };

    Utils.MenuPinsPluseOne = function () {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html());
        newAmount += 1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    };

    Utils.MenuPinsMinusOne = function () {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html());
        newAmount += -1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    };
    return Utils;
})();
//# sourceMappingURL=Utils.js.map
