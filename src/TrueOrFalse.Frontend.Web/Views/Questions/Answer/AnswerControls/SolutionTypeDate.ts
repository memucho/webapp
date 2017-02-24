﻿class SolutionTypeDateEntry
    extends AnswerEntryBase
    implements IAnswerEntry  {

    public SolutionType = SolutionType.Date;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);

        $("#txtAnswer").keypress(() => { this.AnswerQuestion.OnAnswerChange(); });
        $("#txtAnswer").keyup(() => { this.SetDateUi(); });

        var metaData = this.GetJsonMetaData();
        $("#spanEntryPrecision").text(SolutionMetadataDate.GetPrecisionLabel(metaData.Precision) + "genau");
    }

    GetAnswerText(): string {
        return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
        return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    }

    SetDateUi() {
        var dateR = DateParser.Run($("#txtAnswer").val());

        if (!dateR.IsValid) {
            $("#spanEntryFeedback").html("kein g&#252;ltiges Datum");
            $("#iDateError").show();
            $("#iDateCorrect").hide();
        }
        else {
            $("#spanEntryFeedback").html(
                "<b>" + SolutionMetadataDate.GetPrecisionLabel(dateR.Precision) + "genau </b>" +
                "(" + dateR.ToLabel() + ")"
                );
            $("#iDateError").hide();
            $("#iDateCorrect").show();
        }

    }

    GetJsonMetaData(): SolutionMetadataDate {
        var jsonVal = $("#hddSolutionMetaDataJson").val();
        if (jsonVal.length == 0) {
            window.alert("Fehler: ungültige Frage");
            Logger.Error("no solution metaData");
        }

        return <SolutionMetadataDate>jQuery.parseJSON(jsonVal);
    }
}