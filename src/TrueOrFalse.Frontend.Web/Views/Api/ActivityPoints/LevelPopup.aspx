<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LevelPopupModel>" ContentType="text/xml" %>

<div id="levelPopupModal" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <img class="happy-memo-svg" width="120" src="/Images/memucho_MEMO_happy_blau.svg">
                <h4 class="modal-title">
                    <span class="title-text"><b>Fortschritt:</b> Du bist jetzt Level </span>
                    <span class="level-display">
                        <svg>
                            <circle cx="50%" cy="50%" r="15" />
                            <text class="level-count" x="50%" y="50%" dy = ".34em" ><%= Model.UserLevel %></text>
                        </svg>
                    </span>
                    <% if (Model.IsLoggedIn) { %>
                        <div class="title-text">
                            Super! Du wirst immer schlauer.
                        </div>
                    <% } %>
                </h4>
            </div>
            <div class="modal-body">
                <% if (Model.IsLoggedIn)
                   { %>
                        Das nächste Level erreichst du bei <b><%= Model.PointsToNextLevel %></b> Punkten.
                <% }
                   else
                   { %>
                    Wenn du dich jetzt registrierst, behältst du deine Punkte und
                    dein erreichtes Level. <br/> <br/>
                    <b>Registriere dich jetzt und werde immer schlauer mit memucho!</b>
                 <% } %>
            </div>
            <div class="modal-footer">
                <% if (Model.IsLoggedIn)
                   { %>
                        <button type="button" class="btn" data-dismiss="modal">WEITERLERNEN</button>
                <% }
                   else
                   { %>
                        <button type="button" class="btn" data-dismiss="modal">WEITERMACHEN</button>
                        <button type="button" class="btn btn-primary redirect-to-register" data-dismiss="modal">REGISTRIEREN</button>
                <% } %>
            </div>
        </div>
    </div>
</div>