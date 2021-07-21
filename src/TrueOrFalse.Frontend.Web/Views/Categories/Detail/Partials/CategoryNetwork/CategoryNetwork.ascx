<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<div id="Footer-CategoryNetwork">
    <h1>Über- und untergeordnete Themen</h1>
    
    <% if (Model.Category.IsHistoric) { %>
        <div class="alert alert-info" role="alert">
            Aus technischen Gründen können <b>keine Archivdaten für <i>Über- und untergeordnete 
            Themen</i></b> angezeigt werden. Es werden die aktuellen Themenbeziehungen dargestellt.
        </div>
    <% } %>
    
    <div class="CategoryRelations Box">
        <% if (Model.CategoriesParent.Count > 0) { %>
            <div class="related-categories">
                <% foreach (var category in Model.CategoriesParent)
                    { %>
                    <% if (category.IsVisibleToCurrentUser()) 
                            Html.RenderPartial("CategoryLabel", category); %>
                <% } %>
            </div>
        <% }
            else { %>
            <div>keine übergeordneten Themen</div>
        <%  } %>
    
        <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
        <div class="MainCategory">
            <div class="category-main-chip">
                <% Html.RenderPartial("CategoryLabel", Model.Category); %>
            </div>
        </div>
        <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
    
        <% if(Model.CategoriesChildren.Count > 0){ %>
            <div class="related-categories">
                <% foreach(var category in Model.CategoriesChildren){ %>
                    <% if (category.IsVisibleToCurrentUser()) 
                        Html.RenderPartial("CategoryLabel", category); %>
                <% } %>
                <%if (!Model.Category.IsHistoric)  {%>
                    <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                       style="font-size: 13px; cursor: pointer; line-height: 32px; padding-top: 4px; color: #555555;"
                       onclick="eventBus.$emit('open-add-category-modal', <%= Model.Category.Id %>)" 
                       data-original-title="Neues untergeordnetes Thema erstellen"></i>
                <%}%>

            </div>
        <% } else { %>
            <div style="margin-top: 0;">keine untergeordneten Themen
                <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                    style="font-size: 14px; cursor: pointer"
                    onclick="eventBus.$emit('open-add-category-modal', <%= Model.Category.Id %>)" 
                    data-original-title="Neues untergeordnetes Thema erstellen"></i>
            </div>
        <%  } %>
    </div>





</div>
