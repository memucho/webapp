var MarkDownEditor = (function () {
    function MarkDownEditor() {
        var _this = this;
        $("#openExtendedQuestion").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").toggle();
            if(!_this._isOpen) {
                _this.InitEditor();
            }
        });
    }
    MarkDownEditor.prototype.InitEditor = function () {
        var converter1 = Markdown.getSanitizingConverter();
        converter1.hooks.chain("preBlockGamut", function (text, rbg) {
            return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                return "<blockquote>" + rbg(inner) + "</blockquote>\n";
            });
        });
        var editor1 = new Markdown.Editor(converter1);
        editor1.run();
        this._isOpen = true;
    };
    return MarkDownEditor;
})();
