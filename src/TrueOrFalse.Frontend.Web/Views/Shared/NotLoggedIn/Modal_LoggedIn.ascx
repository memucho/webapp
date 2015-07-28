﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="modalNotLoggedIn" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h4><i class="fa fa-power-off"></i> Du bist nicht angemeldet.</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">Um diese Funktion zu nutzen, musst du angemeldet sein. <br />
                        Jetzt <a href="/Anmelden">anmelden</a> oder <a href="/Registrieren">registrieren</a>!
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn btn-default" id="btnCloseDateDelete">Schließen</a>
            </div>
        </div>
    </div>
</div>