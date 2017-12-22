class CategoryPage {

    CategoryId;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
    }

}

$(() => {
    
    var page = new CategoryPage();

    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new Tabbing(page);
    new CategoryHeader(page);
});