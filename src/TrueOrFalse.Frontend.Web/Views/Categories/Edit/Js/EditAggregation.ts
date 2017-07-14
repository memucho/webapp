$(function () {

    $('#EditAggregationModal').on('show.bs.modal', () => {
        loadModalBody();
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
                loadModalBody();
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

    $('#btnCloseAggregation').click(function (e) {

        e.preventDefault();
        $('#EditAggregationModal').modal('hide');
    });

});

function loadModalBody() {
    $('#EditAggregationModal .modal-body').html('<div style="text-align: center"><i class="fa fa-spinner fa-spin"></i></div>');
    $.ajax({
        url: '/EditCategory/AggregationModalContent?catId=' + $('#hhdCategoryId').val(),
        type: 'GET',
        success: function (data) {
            $('#EditAggregationModal .modal-body')
                .html(data);
            $('.show-tooltip').tooltip();
        }
    });
}

