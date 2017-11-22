var iframeId = "memuchoFrame";
var senderDomains = ['http://memucho', 'http://memucho.local', 'https://memucho.de'];

//https://github.com/closingtag/super-awesome-responsive-iframe-solution/blob/master/index.html
function receiveMessage(event) {

    var message = event.data.split(':');
    var eventName = message[0];
    var iframes, len, i = 0;
    // Domains to accept post messages from:
    // Fix .indexOf in IE8
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (obj, start) {
            for (var i = (start || 0), j = this.length; i < j; i++) {
                if (this[i] === obj) { return i; }
            }
            return -1;
        }
    }

    if (senderDomains.indexOf(event.origin) !== -1 && eventName === 'resize') {
        iframes = document.getElementsByTagName('iframe');
        len = iframes.length;

        for (; i < len; i++) {
            if ((iframes[i].contentWindow || iframes[i].documentWindow) == event.source) {
                iframes[i].style.height = message[1] + "px";

                var maxWidth = iframes[i].getAttribute("data-maxWidth");
                if (maxWidth && maxWidth.length > 0) {
                    iframes[i].style.maxWidth = maxWidth;
                }

                return;
            }
        }
    }
}

function writeIframe(iframeId, iframeSource, logoOn) {
    var width = scriptTag.getAttribute("data-width");

    var attrMaxWidth = "";
    var styleMaxWidth = "";
    var maxWidth = scriptTag.getAttribute("data-maxWidth");
    if (maxWidth && maxWidth.length > 0) {
        attrMaxWidth = "maxWidth=\"" + maxWidth + "\"";
        styleMaxWidth = "max-width: " + maxWidth + ";";
    }

    if ((logoOn == null | (logoOn !== "true" && logoOn !== "false")) & iframeSource.indexOf("&host=memucho.de") >= 0) { // if no explicit value for logoOn is set and widget runs on memucho, than hide logo
        logoOn = "false";
    }

    var memuchoLogo = (logoOn === "false")
        ? ""
        : '<div style="font-family: \'Open Sans\', Arial, sans-serif; font-size: 12px; position: relative; width: 100%; visibility: hidden;" id="memuchoLogo' + iframeId + '"> ' +
            '<a href="https://memucho.de" target="_blank"' + 'onmouseover = "this.style.filter = \'grayscale(0)\'" onmouseout  = "this.style.filter = \'grayscale(1)\'">' +
                '<span style="font-weight: bold;"> memucho </span>' +
                '<img src="https://memucho.de/Images/Logo/LogoSmall.png"/ style="width: 23px; height: auto; vertical-align: middle; padding-bottom: 4px; border: none; box-shadow: none;">' +
            '</a>' +
        '</div>';

    var iframeHtml =
        '<div style="width: ' + width + '; ' + styleMaxWidth + '">' +
            '<style>' +
                'div#memuchoLogo' + iframeId + ' a{' +
                    'border-bottom: none !important; ' +
                    'font-family: "Open Sans", Arial, sans-serif !important; ' +
                    'font-size: 12px !important; ' +
                    'text-decoration: none !important; ' +
                    'color: rgb(175, 213, 52) !important; ' +
                    'position: absolute; top: -40px; ' +
                    'right: 15px; width: 150px !important; ' +
                    'text-align: right; filter: grayscale(1);' +
                '}' +
            '</style > ' +
            '<iframe ' +
                'id="' + iframeId + '" name="widget" ' +
                'src="#" height="1" ' + attrMaxWidth + ' ' +
                'marginheight="0" marginwidth="0" ' +
                'frameborder="no" scrolling="no"> ' +
            '</iframe>' + 
            memuchoLogo +
        '</div>';

    if (scriptTag.getAttribute("data-isPreview")) {
        var newElement = document.createElement('div');
        newElement.innerHTML = iframeHtml;
        document.getElementById('divPreviewSetWidget').appendChild(newElement);
    } else {
        document.write(iframeHtml);
    }

    setTimeout(function () {
        document.getElementById('memuchoLogo' + iframeId).style.visibility = "visible";
    }, 3000);

    var loadedIframe = parent.document.getElementById(iframeId);
    loadedIframe.width = width;
    loadedIframe.src = filePath;
}

function getScripts() {
    var scripts_ = document.getElementsByTagName('script');

    var scripts = [];
    for (var i = 0; i < scripts_.length; i++) {
        if (scripts_[i].getAttribute("data-t"))
            scripts.push(scripts_[i]);
    }

    return scripts;
}

function checkForPreview(script) {
    return script.getAttribute("data-isPreview") == "true";
}

if (window.addEventListener) {
    window.addEventListener('message', receiveMessage, false);
}
else if (window.attachEvent) {
    window.attachEvent('onmessage', receiveMessage);
}

var scripts = getScripts();
var scriptIndex;

var scriptTag;

var previewScripts = scripts.filter(checkForPreview);

if (!!previewScripts[0]) {
    scriptTag = previewScripts[0];
} else {
    if (scriptIndex == undefined)
        scriptIndex = -1;

    scriptIndex++;

    scriptTag = scripts[scriptIndex];
}

var type_ = scriptTag.getAttribute("data-t");

var domain = "https://memucho.de";
var domainForDebug = scriptTag.getAttribute("data-domainForDebug");

if (domainForDebug && domainForDebug.length > 0)
    domain = domainForDebug;


var queryDummy = "?1=1";

var queryKnowledgeBtn = "";
var hideKnowledgeBtn = scriptTag.getAttribute("data-hideKnowledgeBtn");
if (hideKnowledgeBtn && hideKnowledgeBtn.length > 0 && hideKnowledgeBtn == "true") {
    queryKnowledgeBtn = "&hideAddToKnowledge=true";
}

var querySetTitle = "";
var setTitleAttr = scriptTag.getAttribute("data-setTitle");
if (setTitleAttr && setTitleAttr.length > 0) {
    querySetTitle = "&title=" + setTitleAttr;
}

var querySetText = "";
var setTextAttr = scriptTag.getAttribute("data-setText");
if (setTextAttr && setTextAttr.length > 0) {
    querySetText = "&text=" + setTextAttr;
}

var queryQuestionCount = "";
var questionCountAttr = scriptTag.getAttribute("data-questionCount");
if (questionCountAttr && questionCountAttr.length > 0) {
    queryQuestionCount = "&questionCount=" + questionCountAttr;
}

var queryLogoOn = "";
var logoOnAttr = scriptTag.getAttribute("data-logoOn");
//if (logoOnAttr && logoOnAttr.length > 0) {
//    queryLogoOn = "&logoOn=" + logoOnAttr;
//}

var queryWidgetKey = "";
var widgetKeyAttr = scriptTag.getAttribute("data-widgetKey");
if (widgetKeyAttr && widgetKeyAttr.length > 0) {
    queryWidgetKey = "&widgetKey=" + widgetKeyAttr;
}

var queryHost = "&host=" + window.location.hostname;
var queryPartShared = queryDummy + queryKnowledgeBtn + queryHost + queryWidgetKey + queryQuestionCount;

if (type_ === "question")
{
    var questionId = scriptTag.getAttribute("data-id");

    var filePath = domain + '/widget/frage/' + questionId + queryPartShared + queryQuestionCount;
    var iframeId = "iframe-q" + questionId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath, logoOnAttr);
}
else if (type_ === "set") {
    var setId = scriptTag.getAttribute("data-id");

    var filePath = domain + '/widget/fragesatz/start/' + setId + queryPartShared;
    var iframeId = "iframe-s" + setId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath, logoOnAttr);
}
else if (type_ === "templateset") {
    var setId = scriptTag.getAttribute("data-id");

    var filePath = domain + '/widget/fragesatz/templateset/' + setId + queryPartShared + querySetTitle + querySetText;
    var iframeId = "iframe-s" + setId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath, logoOnAttr);
}
else if (type_ === "setVideo") {
    var setId = scriptTag.getAttribute("data-id");

    var filePath = domain + '/widget/fragesatz-v/' + setId + queryPartShared;
    var iframeId = "iframe-sv" + setId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath, logoOnAttr);
}