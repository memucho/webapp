﻿
    <div class="modal fade" id="topicnavigationSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">TopicNavigation bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Titel</label>
                            <input class="form-control" v-model="title" placeholder="">
                            <small class="form-text text-muted">Der Titel ist optional.</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="text">Text</label>
                            <textarea-autosize class="form-control" rows="2" v-model="text" :min-height="40" />
                            <small class="form-text text-muted">Beschreibe die Navigation</small>
                        </div>

                        
                            <div class="form-group">
                                <label for="load">Lade</label>
                                <form class="form-inline" style="margin:0">
                                <div class="radio">
                                    <label class="clickable">
                                        <input type="radio" value="All" v-model="load">
                                        Alles
                                    </label>
                                </div>
                                <small class="form-text" style="margin: 0 10px;">oder</small>
                                <input class="form-control" v-model="load" placeholder="12,58,100">
                                </form>
                                <small class="form-text text-muted">Gebe eine SetId ein und trenne dies mit einem Komma.</small>
                            </div>
                        
                        <label for="order">Sortierung</label>
                        <div v-if="load != 'All'">
                            <div class="form-group">
                                <div class="radio disabled">
                                    <label>
                                        <input type="radio" disabled>
                                        Alphabetisch
                                    </label>
                                </div>
                                <div class="radio disabled">
                                    <label>
                                        <input type="radio" disabled>
                                        Anzahl Fragen
                                    </label>
                                </div>
                                <div class="radio disabled">
                                    <label>
                                        <input type="radio" disabled>
                                        Freie Sortierung
                                    </label>
                                </div>
                            </div>     
                        </div>
                        <div v-else>
                            <div class="form-group">
                                <div class="radio">
                                    <label class="clickable">
                                        <input type="radio" value="Name" v-model="order">
                                        Alphabetisch
                                    </label>
                                </div>
                                <div class="radio">
                                    <label class="clickable">
                                        <input type="radio" value="QuestionAmount" v-model="order">
                                        Anzahl Fragen
                                    </label>
                                </div>
                                <div class="radio">
                                    <label class="clickable">
                                        <input type="radio" value="Free" v-model="order">
                                        Freie Sortierung
                                    </label>
                                </div>
                            </div>     
                        </div>
                    </form>
                    
                    <div v-if="showSetList">
                        <div class="setCards" v-sortable="setOptions">
                            <div class="setCards topic grid" v-for="(id, index) in sets" :setId="id" :key="index">
                                <div class="setCards card">
                                    <div>
                                        <span>Set: {{id}}</span>
                                    </div>
                                    <div>
                                        <a class="clickable" @click.prevent="removeSet(index)"><i class="fa fa-trash"></i> Set entfernen</a>
                                    </div>
                                </div>
                            </div>
                            <div id="addCardPlaceholder" class="setCards topic grid placeholder">
                                <div class="addCard" v-if="showSetInput">
                                    <div class="form-group">
                                        <input class="form-control" v-model="newSetId" placeholder="" type="number">
                                        <div class="applyAndCancel">
                                            <a class="clickable" @click="hideSetInput">abbrechen</a>
                                            <div class="btn btn-primary" @click="addSet(newSetId)">hinzufügen</div>
                                        </div>
                                    </div>
                                </div>
                                <div v-else class="addCard btn btn-primary" @click="showSetInput = true">Set hinzufügen</div>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div class="applyAndCancel modalFooter">
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>