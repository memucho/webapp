/// <binding AfterBuild='copyScripts' />
var deps = {
    "vue": {
        "dist/vue.js": ""
    },
    "vue-textarea-autosize": {
        "dist/vue-textarea-autosize.umd.js": ""
    },
    "vue-sticky-directive": {
        "vue-sticky-directive.js": ""
    },
    "postscribe": {
        "dist/postscribe.js": ""
    },
    "d3": {
        "dist/d3.js": ""
    },
    "vue-slider-component" : {
        "dist/vue-slider-component.umd.js": ""
    },
};

var merge = require('merge-stream'); 
var gulp = require('gulp');

function copyScripts(cb) {
    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);

        //console.log(gulp.dest("scripts/npm/"));

        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("scripts/npm/" + prop + "/" + deps[prop][itemProp])));
        }
    }
    merge(streams);

    cb();
}

exports.copyScripts = copyScripts;

function buildTiptap(cb) {
    var run = require('gulp-run');
    run('npm run build:webpack');
    cb();
}

exports.buildTiptap = buildTiptap;