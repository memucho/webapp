<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="cardModal">
    <div class="modal fade" id="modalDeleteQuestion" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog modal-m" role="document">
            <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>

            <div class="modal-content">
                <div class="cardModalContent">
                    <div class="modalHeader">
                        <h4 class="modal-title">Frage löschen</h4>
                    </div>
                    <div class="modalBody">
                        <div class="body-m">Möchtest Du "<span id="spanQuestionTitle"></span>" unwiederbringlich löschen? Alle damit verknüpften Daten werden entfernt!</div>
                        <div class="alert alert-danger" id="questionDeleteCanNotDelete"></div>
                        <div class="alert alert-info" id="questionDeleteResult"></div>
                    </div>
                    <div class="modalFooter">
                        <a href="#" class="btn btn-danger memo-button" id="confirmQuestionDelete">Frage Löschen</a>
                        <div class="btn btn-link memo-button" data-dismiss="modal">Abbrechen</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>