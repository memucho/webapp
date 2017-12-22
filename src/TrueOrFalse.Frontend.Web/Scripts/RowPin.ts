enum PinType {
    Question,
    Set,
    SetDetail,
    Category,
    CategoryDetail
}

class Pin {

    _changeInProgress: boolean;
    _pinRowType: PinType;

    OnPinChanged: () => void;

    constructor(pinRowType: PinType, onPinChanged: () => void = null) {

        var self = this; 
        this._pinRowType = pinRowType;
        this.OnPinChanged = onPinChanged;

        var allPins;
        if (self.IsQuestionRow())
            allPins = $(".Pin[data-question-id]").find(".iAdded, .iAddedNot"); 
        else if (self.IsSetDetail() || self.IsSetRow())
            allPins = $(".Pin[data-set-id]").find(".iAdded, .iAddedNot"); 
        else if (self.IsCategoryRow() || self.IsSetDetail())
            allPins = $(".Pin[data-category-id]").find(".iAdded, .iAddedNot"); 

        allPins.off('click.rowPin').on('click.rowPin', function (e) {

            e.preventDefault();
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("Pin_" + PinType[self._pinRowType]);
                return;
            }

            var elemPin = $($(this).parents(".Pin"));

            var id = -1;
            if (self.IsQuestionRow())
                id = parseInt(elemPin.attr("data-question-id"));
            else if (self.IsSetRow() || self.IsSetDetail())
                id = parseInt(elemPin.attr("data-set-id"));
            else if (self.IsCategoryRow() || self.IsCategoryDetail())
                id = parseInt(elemPin.attr("data-category-id"));

            if (self.IsCategoryRow()) {
                elemPin = $($.unique(elemPin.add($('[data-category-id=' + id + ']')).get()));//CategoryDetail page: toggle both desktop and mobile pin
            }

            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) /* pin */ {

                self.Pin(id, onPinChanged);
                elemPin.find(".iAddedNot, .iAddSpinner").toggleClass("hide2");

                window.setTimeout(() => {
                    elemPin.find(".iAdded, .iAddSpinner").toggleClass("hide2");
                    self._changeInProgress = false;

                    if (self.IsQuestionRow())
                        Utils.MenuPinsPluseOne();
                    else 
                        Utils.SetMenuPins();

                    if (self.IsSetDetail())
                        Utils.PinQuestionsInSetDetail();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(elemPin) + 1, elemPin);
                }, 400);

            } else /* unpin */ {

                self.UnPin(id, onPinChanged);
                elemPin.find(".iAdded, .iAddSpinner").toggleClass("hide2");

                window.setTimeout(() => {
                    elemPin.find(".iAddedNot, .iAddSpinner").toggleClass("hide2");
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsMinusOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());
                        
                    self.SetSidebarValue(self.GetSidebarValue(elemPin) - 1, elemPin);
                }, 400);
            }
        });

        $("#UnpinSetModal").off("click").on("click", "#JS-RemoveQuestions", () => {
            $.when(
                SetsApi.UnpinQuestionsInSet($('#JS-RemoveQuestions').attr('data-set-id'), onPinChanged))
             .then(() => {
                Utils.UnpinQuestionsInSetDetail();
                Utils.SetMenuPins();
            });
        });

        $("#UnpinCategoryModal").off("click").on("click", "#JS-RemoveQuestionsCat", () => {
            $.when(
                CategoryApi.UnpinQuestionsInCategory($('#JS-RemoveQuestionsCat').attr('data-category-id'), onPinChanged))
            .then(() => {
                Utils.SetMenuPins();
            });
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        if (this.IsSetDetail()) {
            Utils.SetElementValue2(parent.find("#totalPins"), newValue.toString() + "x");
            parent.find("#totalPins").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        } else {
            Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
            parent.parents(".rowBase").find(".totalPinsTooltip").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        }
    }

    GetSidebarValue(parent: JQuery): number {
        if (this.IsSetDetail())
            return parseInt(/[0-9]*/.exec(parent.find("#totalPins").html())[0]);
        else
            return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find(".totalPins").html())[0]);
    }

    Pin(id: number, onPinChanged: () => void = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id, onPinChanged);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Pin(id, onPinChanged);
        } else if (this.IsCategoryRow() || this.IsCategoryDetail()) {
            CategoryApi.Pin(id, onPinChanged);
        }
    }

    UnPin(id: number, onPinChanged: () => void = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id, onPinChanged);
        } else if (this.IsSetRow() || this.IsSetDetail()) {

            SetsApi.Unpin(id);

            $("#JS-RemoveQuestions").attr("data-set-id", id);
            $("#UnpinSetModal").modal('show');

        } else if (this.IsCategoryRow() || this.IsCategoryDetail()) {

            CategoryApi.Unpin(id);

            $("#JS-RemoveQuestionsCat").attr("data-category-id", id);
            $("#UnpinCategoryModal").modal('show');
        }
    }

    IsQuestionRow(): boolean {
        return this._pinRowType == PinType.Question;
    }

    IsSetRow(): boolean {
        return this._pinRowType == PinType.Set;
    }

    IsSetDetail(): boolean {
        return this._pinRowType == PinType.SetDetail;
    }

    IsCategoryRow(): boolean {
        return this._pinRowType == PinType.Category;
    }

    IsCategoryDetail(): boolean {
        return this._pinRowType == PinType.CategoryDetail;
    }
}