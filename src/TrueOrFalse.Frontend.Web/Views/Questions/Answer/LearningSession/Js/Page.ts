$(() => {

    let lsr = new LearningSessionResult();
    $(document).ready(lsr.PositionIndicatorAverageText);
    $(window).resize(lsr.PositionIndicatorAverageText);

    $("#divCallForRegistration")
        .delay(3500)
        .fadeIn()
        .animate({ opacity: 1 }, 1500);

    setInterval(() => {
            $(".shakeInInterval").removeClass("tada animated");
            window.setTimeout(() => {
                    $(".shakeInInterval").addClass("tada animated");
                },
                1000);
        },
        7000);

});