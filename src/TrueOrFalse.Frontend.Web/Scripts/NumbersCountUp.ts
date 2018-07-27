

class NumbersCountUp {

    JQuery: JQuery[] = new Array();

    constructor() {
        
        if ($('.CountUpProgress').length != 0) {
            this.JQuery.push($(".CountUpProgress"));
        }

        if ($('.CountUp').length != 0) {
            this.JQuery.push($('.CountUp'));
        }

        for (var i = 0; i < this.JQuery.length; i++) {
            this.JQuery[i].attr("hasBeenViewed", "false");
        }

        this.animateWhenVisible();
    }

    countUp(start: number, end: number, character: string, htmlClass: string, element: HTMLElement) {
            
        var loops = 50;
        var intervalSlower;
        var loopCount = 0;
        var value = start;
        var timePointSlower = (end * 0.9);
        var increment = (end - start) / loops;
        var interval = setInterval(updateTimer, 10);
        
        function slower() {
            intervalSlower = setTimeout(updateTimer, 100);
        }

        function numberWith1000Separator(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }

        function updateTimer() {

            value += increment;
            loopCount++;

            if (htmlClass === "CountUpProgress") {
                $(element).width(value.toFixed(0) + character);
            } else {
                $(element).html(numberWith1000Separator(value.toFixed(0)) + character);
            }

            if (value > timePointSlower && value <= end) {
                clearInterval(interval);
                slower();
            }

            if (loopCount >= loops) {
                clearTimeout(intervalSlower);
                clearInterval(interval);
            }
        }
    }

    checkIfVisible(numbersCountUp: NumbersCountUp) {
        numbersCountUp.JQuery.forEach((element) => { // a Objekt with this Struktur Count{ 0,1,2} ,CountUp{0,1,2}
        
           for (let i = 0; i < element.length; i++) {
          
                var hT = $(element[i]).offset().top,
                    hH = $(element[i]).outerHeight(),
                    wH = $(window).innerHeight(),
                    wS = $(window).scrollTop();
               
                if ((wS) > (hT + hH - wH) && element[i].getAttribute("hasbeenviewed") === "false") {

                    let character = element[i].getAttribute("data-character") != null ? element[i].getAttribute("data-character") : "";
                    let htmlClass = element[i].getAttribute("class") === "CountUp" ?  "CountUp" : "CountUpProgress";
                    let finalNumber = element[i].getAttribute("data-number") != null ? parseInt(element[i].getAttribute("data-number")) : -1;
                    let startNumber = element[i].getAttribute("data-startNumber") !== null ? parseInt(element[i].getAttribute("data-startNumber")) : 0;

                    if (finalNumber === -1)
                        console.log("Please add attribuet data-number in your Numbers Count Up ");

                    numbersCountUp.countUp(startNumber, finalNumber, character, htmlClass, element[i]); 
                    element[i].setAttribute("hasbeenviewed", "true");
                }
            }
        });
        return;
    }

    animateWhenVisible() {
        var self = this;
        
        self.checkIfVisible(self);
        $(document).scroll(() => {
            self.checkIfVisible(self);
        });
    }

}

