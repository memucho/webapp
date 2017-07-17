/// <reference path="../../../../scripts/typescript.defs/simplemde.d.ts" />

$(() => {

    var previewTimeOut;

    var simplemde = new SimpleMDE({
        element: $("#TopicMarkdown")[0],
        spellChecker: false,
        syncSideBySidePreviewScroll : false,
        previewRender: (plainText, preview) => {

            var scrollTop = preview.scrollTop;

            if (previewTimeOut)
                clearTimeout(previewTimeOut);

            previewTimeOut = setTimeout(() => {
                $.post("/EditCategory/GetMarkdownPreview/", { categoryId: $("#hhdCategoryId").val() , text: plainText}, (result) => {
                    preview.innerHTML = result;
                    preview.scrollTop = scrollTop;
                });
            }, 750);

            return "Loading...";
        }
    });

    new CategoryDelete();
});