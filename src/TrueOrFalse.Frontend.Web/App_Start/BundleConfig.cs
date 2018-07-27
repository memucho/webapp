using System.Web.Optimization;

namespace TrueOrFalse.View
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* CSS */
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                  "~/Style/bootstrap/bootstrap.css",
                  "~/Style/includes/shared.css",
                  "~/Style/*.css",
                  "~/Style/jquery-ui/jquery-ui.structure.css", 
                  "~/Style/jquery-ui/jquery-ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/markdownCss")
                .Include("~/Style/markdown-editor.css"));

            bundles.Add(new StyleBundle("~/bundles/message")
                .Include("~/Views/Messages/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/shared")
                .IncludeDirectory("~/Scripts/", "*.js")
                .IncludeDirectory("~/Scripts/vendor", "*.js")
                .IncludeDirectory("~/Scripts/header", "*.js")
                .IncludeDirectory("~/Scripts/hubs", "*.js")
                .IncludeDirectory("~/Scripts/socialLogins", "*.js")
                .IncludeDirectory("~/Views/Images", "*.js")
                .IncludeDirectory("~/Views/Welcome/Login", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileUploader")
                .Include("~/Scripts/vendor.file-uploader/header.js")
                .Include("~/Scripts/vendor.file-uploader/util.js")
                .Include("~/Scripts/vendor.file-uploader/button.js")
                .Include("~/Scripts/vendor.file-uploader/handler.base.js")
                .Include("~/Scripts/vendor.file-uploader/handler.form.js")
                .Include("~/Scripts/vendor.file-uploader/handler.xhr.js")
                .Include("~/Scripts/vendor.file-uploader/uploader.basic.js")
                .Include("~/Scripts/vendor.file-uploader/dnd.js")
                .Include("~/Scripts/vendor.file-uploader/uploader.js")
                .Include("~/Scripts/vendor.file-uploader/jquery-plugin.js"));

            bundles.Add(new ScriptBundle("~/bundles/guidedTourScript")
                .IncludeDirectory("~/Scripts/guidedTour/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/guidedTourStyle")
                .Include("~/Style/guidedTour/bootstrap-tour.css"));

            bundles.Add(new ScriptBundle("~/bundles/Welcome")
                .IncludeDirectory("~/Views/Welcome/Js", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/RegistrationJs")
                .Include("~/Views/Welcome/Js/Validation.js")
                .IncludeDirectory("~/Views/Welcome/Registration/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Registration")
                .Include("~/Views/Welcome/Registration/SocialButtons.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Help")
                .IncludeDirectory("~/Views/Help/Js", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Category")
                .Include("~/Views/Categories/Detail/Category.css")
                .Include("~/Views/Categories/Detail/Js/SquareWishKnowledge.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Category")
                .Include("~/Views/Knowledge/Wheel/KnowledgeWheel.js")
                .IncludeDirectory("~/Views/Categories/Detail/Js/", "*.js")
                .Include("~/Views/Categories/ResultTestSession/Js/GetResultTestSession.js"));

            bundles.Add(new StyleBundle("~/bundles/Login")
                .Include("~/Views/Welcome/Registration/SocialButtons.css"));

            bundles.Add(new StyleBundle("~/bundles/Knowledge")
                .IncludeDirectory("~/Views/Knowledge/", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Knowledge")
                .Include("~/Views/Knowledge/Js/*.js"));

            bundles.Add(new StyleBundle("~/bundles/AlgoInsight")
                .IncludeDirectory("~/Views/AlgoInsight/", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/questions")
                .IncludeDirectory("~/Views/Questions/Js/", "*.js")
                .Include("~/Scripts/ValuationPerRow.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/questionEdit")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .IncludeDirectory("~/Views/Questions/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Beta")
                .IncludeDirectory("~/Views/Beta/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Sets")
                .IncludeDirectory("~/Views/Sets/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Categories")
                .IncludeDirectory("~/Views/Categories/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/CategoryEdit")
                .Include("~/Views/Categories/Edit/EditCategory.css")
                .Include("~/Scripts/vendor.somewhere/simplemde.css")
                .Include("~/Views/Categories/Detail/Category.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/CategoryEdit")
                .IncludeDirectory("~/Views/Categories/Edit/Js/", "*.js")
                .Include("~/Views/Categories/Js/CategoryDelete.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js")
                .Include("~/Scripts/vendor.somewhere/simplemde.js")
                .Include("~/Views/Categories/Edit/Js/EditCategoryNavBar.js")
                .Include("~/Scripts/vendor.somewhere/d3v3.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Users")
                .IncludeDirectory("~/Views/Users/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Users")
                .IncludeDirectory("~/Views/Users/", "*.css"));

            bundles.Add(new StyleBundle("~/bundles/User")
                .IncludeDirectory("~/Views/Users/Detail/", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/Js/User")
                .IncludeDirectory("~/Views/Users/Detail/Js", "*.js")
                .Include("~/Views/Users/Js/UserRowFollow.js").
                Include("~/Views/Users/Js/UserRow.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/AnswerQuestion")
                .Include("~/Scripts/widgets/ShareDialog.js")
                .Include("~/Views/Questions/Answer/ShareQuestion.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/Js/", "*.js")
                .IncludeDirectory("~/Views/Questions/ActivityPoints", "*.js")
                .Include("~/Views/Questions/Answer/TestSession/Js/TestSessionResult.js")
                .Include("~/Views/Categories/ResultTestSession/Js/GetResultTestSession.js")
                .Include("~/Views/Questions/Answer/LearningSession/Js/LearningSessionResult.js")
                .Include("~/Views/Questions/Answer/LearningSession/Js/LearningSessionResultCharts.js"));

            bundles.Add(new StyleBundle("~/bundles/AnswerQuestion")
                .Include("~/Views/Questions/Answer/*.css"));

            bundles.Add(new StyleBundle("~/bundles/js/DeleteQuestion")
                .Include("~/Views/Questions/Js/QuestionRowDelete.js"));

            bundles.Add(new ScriptBundle("~/bundles/SetEdit")
                .Include("~/Views/Sets/Detail/Js/YoutubeApi.js")
                .IncludeDirectory("~/Views/Sets/Edit/Js/" ,"*.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Set")
                .Include("~/Views/Knowledge/Wheel/KnowledgeWheel.js")
                .Include("~/Scripts/widgets/ShareDialog.js")
                .IncludeDirectory("~/Views/Sets/Detail/Js", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Questions/ActivityPoints", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Set")
                .Include("~/Views/Questions/Answer/AnswerQuestion.css")
                .Include("~/Views/Questions/Answer/AnswerQuestionSolution.css")
                .Include("~/Views/Sets/Detail/Set.css"));
                

            bundles.Add(new ScriptBundle("~/bundles/js/Messages")
                .IncludeDirectory("~/Views/Messages/Js/", "*.js"));

            //Dates
            bundles.Add(new StyleBundle("~/bundles/Dates")
                .Include("~/Views/Dates/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Dates")
                .Include("~/Views/Dates/Js/*.js"));

            bundles.Add(new StyleBundle("~/bundles/EditDate")
                .Include("~/Views/Dates/Edit/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/EditDate")
                .IncludeDirectory("~/Views/Dates/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteSets.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/LearningSessionResult")
                .IncludeDirectory("~/Views/Questions/Answer/LearningSession/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/TestSessionResult")
                .IncludeDirectory("~/Views/Questions/Answer/TestSession/Js/", "*.js"));

            //Games
            bundles.Add(new ScriptBundle("~/bundles/js/Games")
                .IncludeDirectory("~/Views/Games/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Games")
                .Include("~/Views/Games/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Game")
                .IncludeDirectory("~/Views/Games/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteSets.js"));

            bundles.Add(new StyleBundle("~/bundles/Game")
                .Include("~/Views/Games/Edit/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/GamePlay")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Games/Play/Js/", "*.js")
                .IncludeDirectory("~/Views/Questions/ActivityPoints", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/GamePlay")
                .Include("~/Views/Games/Play/*.css")
                .Include("~/Views/Questions/Answer/AnswerQuestion.css")
                .Include("~/Views/Questions/Answer/AnswerQuestionSolution.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/AwesomeIframe")
                .Include("~/Views/Widgets/AwesomeIframe.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/WidgetQuestion")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .Include("~/Views/Widgets/WidgetQuestion.js")
                .Include("~/Views/Widgets/AwesomeIframe.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/WidgetSet")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .Include("~/Views/Widgets/AwesomeIframe.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/WidgetSetVideo")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Questions/ActivityPoints", "*.js")
                .Include("~/Views/Sets/Detail/Js/SetVideo.js")
                .Include("~/Views/Widgets/WidgetSetVideo.js")
                .Include("~/Views/Widgets/AwesomeIframe.js"));

            //Markdown
            bundles.Add(new ScriptBundle("~/bundles/markdown")
                .Include("~/Scripts/Markdown.Converter.js",
                         "~/Scripts/Markdown.Sanitizer.js", 
                         "~/Scripts/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Maintenance")
               .Include("~/Scripts/BootstrapCustomUtils.js"));

            bundles.Add(new StyleBundle("~/bundles/Maintenance")
                .Include("~/Views/Maintenance/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/MaintenanceImages")
                .Include("~/Views/Maintenance/Images/ImageMaintenance.js"));

            bundles.Add(new StyleBundle("~/bundles/MaintenanceImages")
                .Include("~/Views/Maintenance/Images/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/MaintenanceTools")
                .Include("~/Views/Maintenance/ToolsBrainWaveHub.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/MaintenanceCMS")
                .Include("~/Views/Maintenance/Js/CMS.js")
                .Include("~/Views/Maintenance/Js/CmsCategoryNetworkNavigation.js"));

            bundles.Add(new StyleBundle("~/bundles/MaintenanceContentStats")
                .Include("~/Scripts/vendor.somewhere/dataTables.bootstrap.css")
                .Include("~/Views/Maintenance/ContentStats.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/MaintenanceContentStats")
                .Include("~/Scripts/vendor.somewhere/dataTables.bootstrap.js")
                .Include("~/Scripts/vendor.somewhere/jquery.dataTables.js")
                .Include("~/Views/Maintenance/Js/ContentStats.js"));

            bundles.Add(new ScriptBundle("~/bundles/mailto")
                .Include("~/Scripts/various/mailto.js"));

            bundles.Add(new StyleBundle("~/bundles/Promoter")
                .Include("~/Views/Welcome/Promoter.css"));
            bundles.Add(new ScriptBundle("~/bundles/AboutMemucho")
                .Include("~/Views/About/Js/Page.js"));

#if RELEASE
            BundleTable.EnableOptimizations = true;
#endif
            SetIgnorePatterns(bundles.IgnoreList);
        }

        public static void SetIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList.Ignore("*SetVideoPlayer.js");
        }
    }
}