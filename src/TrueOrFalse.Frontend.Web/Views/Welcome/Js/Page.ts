$(function () {
    //var tourInit = new TourInit();
    //new SendRequestNewsletter();
    new Pin(PinType.Category);

    $("[data-type=learn-wishknowledge]").click(function (e) {
        if (NotLoggedIn.Yes()) {
            e.preventDefault();
            NotLoggedIn.ShowErrorMsg("LearnWishKnowledge");
            return;
        }
    });
    new NumbersCountUp();
});