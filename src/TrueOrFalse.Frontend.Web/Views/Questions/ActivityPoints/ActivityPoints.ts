class ActivityPoints {
    public static addPointsFromRightAnswer() {
        ActivityPoints.addPoints(15, "RightAnswer");
    }

    public static  addPointsFromWrongAnswer() {
        ActivityPoints.addPoints(1, "WrongAnswer");
    }

    public static addPointsFromShowSolutionAnswer() {
        ActivityPoints.addPoints(3, "ShowedSolution");
    }

    public static addPointsFromCountAsCorrect() {
        ActivityPoints.addPoints(12, "CountAsCorrect");
    }

    private static addPoints(amount: number, actionTypeString: string) {
        var url = "/Api/ActivityPoints/Add";
        $.post(url, { activityTypeString: actionTypeString, points: amount} , result => {
            this.updatePointsDisplay(result);
            this.showLevelPopup(result.levelPopup);
        });
    }

    private static updatePointsDisplay(levelData) {
        $("#activityPointsDispaly #activityPoints").html(levelData.totalPoints.toString());
        $("#header-level-display text").html(levelData.userLevel);
    }

    private static showLevelPopup(levelPopup: string) {
        if (levelPopup != "") {
            $("#levelPopupModal").remove();
            var levelPopupObject = $(levelPopup);
            levelPopupObject.find(".redirect-to-register").click(e => {
                $(".TextLinkWithIcon").click();
            });

            levelPopupObject.modal();
        }
    }
}