$(() => {

    $('#EditAggregationModal').on('show.bs.modal', () => {
        initilizeNavBar();
        $("#EditAggregationModal .nav .tab-unterthemen").click();
    });

    $('#btnEditAggregation').click(e => {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/EditCategory/EditAggregation",
            data: {

                categoryId: $('#hhdCategoryId').val(),
                categoriesToExcludeIdsString: $('#CategoriesToExcludeIdsString').val(),
                categoriesToIncludeIdsString: $('#CategoriesToIncludeIdsString').val()

            },
            cache: false,
            success(e) {
                loadEditAggreationTab();
                //window.alert("Erfolgreich aktualisiert.");
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    });

    $('#btnResetAggregation').click(e => {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: "/EditCategory/ResetAggregation",
            data: {
                categoryId: $('#hhdCategoryId').val()
            },
            cache: false,
            success(e) {
                loadModalBody();
                //window.alert("Erfolgreich aktualisiert.");
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    });

    $('#btnCloseAggregation').click(e => {
        e.preventDefault();
        $('#EditAggregationModal').modal('hide');
    });

});


function initilizeNavBar() {
    $("#EditAggregationModal .nav .tab-unterthemen").unbind();
    $("#EditAggregationModal .nav .tab-unterthemen").click(e => {
        e.preventDefault();
        $("#EditAggregationModal .nav .tab-unterthemen").addClass("active");
        $("#EditAggregationModal .nav .tab-categories-graph").removeClass("active");
        $("#EditAggregationModal .tab-body").empty();
        $('#btnEditAggregation').show();
        loadEditAggreationTab();
    });

    $("#EditAggregationModal .nav .tab-categories-graph").unbind();
    $("#EditAggregationModal .nav .tab-categories-graph").click(e => {
        e.preventDefault();
        $("#EditAggregationModal .nav .tab-categories-graph").addClass("active");
        $("#EditAggregationModal .nav .tab-unterthemen").removeClass("active");
        $("#EditAggregationModal .tab-body").empty();
        $('#btnEditAggregation').hide();
        loadCategoryGraphTab();
    });
}

function loadEditAggreationTab() {
    $('#EditAggregationModal .tab-body').html('<div style="text-align: center"><i class="fa fa-spinner fa-spin"></i></div>');
    $.ajax({
        url: '/EditCategory/GetEditCategoryAggregationModalContent?categoryId=' + $('#hhdCategoryId').val(),
        type: 'GET',
        success: data => {
            $('#EditAggregationModal .tab-body')
                .html(data);
            $('.show-tooltip').tooltip();
        }
    });
}

function loadCategoryGraphTab() {
    $('#EditAggregationModal .tab-body').html('<div style="text-align: center"><i class="fa fa-spinner fa-spin"></i></div>');
    $.ajax({
        url: '/EditCategory/GetCategoryGraphDisplay?categoryId=' + $('#hhdCategoryId').val(),
        type: 'GET',
        success: data => {
            $('#EditAggregationModal .tab-body')
                .html(data);
        }
    });
}