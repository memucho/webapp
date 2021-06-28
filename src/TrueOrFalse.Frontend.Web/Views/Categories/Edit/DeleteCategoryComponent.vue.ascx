﻿<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<delete-category-component inline-template>
    <div class="categoryCardModal">
            <div class="modal fade" id="DeleteCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog modal-m" role="document">
            <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
    
            <div class="modal-content">
                <div class="addCategoryCardModal">
                    <div class="modalHeader">
                        <h4 v-if="hasChildren" class="modal-title">Kann nicht gelöscht werden</h4>
                        <h4 v-else class="modal-title">Thema löschen</h4>
                    </div>
                    <template v-if="hasChildren">
                        <div class="modalBody">
                            <div class="alert alert-warning">
                                Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind.
                            </div>
                        </div>
                        <div class="modalFooter">
                            <div class="btn btn-primary memo-button" data-dismiss="modal">Verstanden</div>       
                            <div class="btn btn-link memo-button" data-dismiss="modal">Abbrechen</div>
                        </div>   
                    </template>
                    <template v-else>
                        <div class="modalBody">
                            <div class="body-m">Möchtest Du "<strong>{{categoryName}}</strong>" unwiderruflich löschen?</div>
                            <div class="body-s">Fragen werden nicht gelöscht.</div>
                            <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                {{errorMsg}}
                            </div>
                        </div>
                        <div class="modalFooter">
                            <div class="btn btn-danger memo-button" @click="deleteCategory()">Thema löschen</div>       
                            <div class="btn btn-link memo-button"  data-dismiss="modal">Abbrechen</div>
                        </div>   
                    </template>
                </div>
            </div>
        </div>
    </div>

    </div>
</delete-category-component>
