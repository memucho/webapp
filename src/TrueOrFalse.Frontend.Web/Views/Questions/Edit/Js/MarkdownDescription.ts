/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/markdown.d.ts" />

class MarkdownDescription 
{ 
    constructor() { 
        this.InitEditor();
    }

    InitEditor() 
    { 
        var converter1 = Markdown.getSanitizingConverter();

        converter1.hooks.chain("preBlockGamut", function (text, rbg) {
            return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                return "<blockquote>" + rbg(inner) + "</blockquote>\n";
            });
        }); 


        var editor1 = new Markdown.Editor(converter1, "-2");
        editor1.run();
    }
}