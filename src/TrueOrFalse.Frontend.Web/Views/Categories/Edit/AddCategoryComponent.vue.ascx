<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<add-category-component inline-template>
        <div class="cardModal">
            <div class="modal fade" id="AddCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
                <div class="modal-dialog modal-m" role="document">
                    <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                    <div class="modal-content">
                        <div class="cardModalContent">
                            <div class="modalHeader">
                                <h4 v-if="createCategory" class="modal-title">Neues Thema erstellen</h4>
                                <h4 v-else class="modal-title">Bestehendes Thema verknüpfen</h4>
                            </div>
                            <div class="modalBody" v-if="createCategory">
                                <form v-on:submit.prevent="addCategory">
                                    <div class="form-group">
                                        <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein" />
                                        <small class="form-text text-muted"></small>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{existingCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                                <div class="checkBox" disabled>
                                    <i class="fas fa-check-square"></i>
                                    Privates Thema
                                </div>
                            </div>
                            <div class="modalBody" v-else>
                                <form v-on:submit.prevent="selectCategory">
                                    <div class="form-group dropdown categorySearchAutocomplete"  :class="{ 'open' : showDropdown}">
                                        <div v-if="showSelectedCategory" class="searchResultItem" @click="toggleShowSelectedCategory()" data-toggle="tooltip" data-placement="top" :title="selectedCategory.Name">
                                            <img :src="selectedCategory.ImageUrl"/>
                                            <div>
                                                <div class="searchResultLabel body-m">{{selectedCategory.Name}}</div>
                                                <div class="searchResultQuestionCount body-s">{{selectedCategory.QuestionCount}} Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                                            </div>
                                        </div>
                                        <input v-show="!showSelectedCategory" ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="searchList" autocomplete="off" @click="lockDropdown = false"  aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
                                        <ul class="dropdown-menu" aria-labelledby="searchList">
                                            <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)" data-toggle="tooltip" data-placement="top" :title="c.Name" :data-original-title="c.Name">
                                                <img :src="c.ImageUrl"/>
                                                <div>
                                                    <div class="searchResultLabel body-m">{{c.Name}}</div>
                                                    <div class="searchResultQuestionCount body-s">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                                                </div>
                                            </li>
                                            <li class="dropdownFooter body-m">
                                                <b>{{totalCount}}</b> Treffer. <br/>
                                                Deins ist nicht dabei? <span class="dropdownLink" @click="createCategory = true">Erstelle hier dein Thema</span>
                                            </li>
                                        </ul>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{existingCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                            </div>
                            <div class="modalFooter">
                                <template v-if="createCategory">
                                    <div class="btn btn-primary memo-button" @click="addCategory" :disabled="disableAddCategory">Thema Erstellen</div>       
                                    <div class="btn btn-link memo-button" @click="createCategory = false" >Bestehendes Thema verknüpfen</div>
                                </template>
                                <template v-else>
                                    <div class="btn btn-primary memo-button" @click="addExistingCategory" :disabled="disableAddCategory">Thema verknüpfen</div>       
                                    <div class="btn btn-link memo-button" @click="createCategory = true">Neues Thema Erstellen</div>
                                </template>
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        </div>
</add-category-component>
