using System.Web.Optimization;
using FluentNHibernate.Conventions.Inspections;

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
                  "~/Style/jquery-ui/jquery-ui.theme.css",
                  "~/Fonts/font-awesome-5.7.2/css/all.css",
                  "~/Fonts/font-awesome-5.7.2/css/v4-shims.css",
                  "~/Views/Shared/CategoryLabel.css"));

            bundles.Add(new StyleBundle("~/bundles/markdownCss")
                .Include("~/Style/markdown-editor.css"));

            bundles.Add(new StyleBundle("~/bundles/message")
                .Include("~/Views/Messages/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/shared")
                .IncludeDirectory("~/Scripts/", "*.js")
                .IncludeDirectory("~/Scripts/vendor", "*.js")
                .IncludeDirectory("~/Scripts/header", "*.js")
                .IncludeDirectory("~/Scripts/socialLogins", "*.js")
                .IncludeDirectory("~/Views/Images", "*.js")
                .IncludeDirectory("~/Views/Welcome/Login", "*.js")
                .Include("~/Scripts/vendor/vuetable-2.js")
                .Include("~/Scripts/npm/vue/vue.js"));

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
                .Include("~/Views/Shared/Spinner/Spinner.css")
                .Include("~/Views/Categories/Detail/Category.css")
                .Include("~/Views/Shared/Delete.css"));


            bundles.Add(new StyleBundle("~/bundles/Segmentation")
                .Include("~/Views/Categories/Detail/Partials/Segmentation/Segmentation.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Category")
                .Include("~/Views/Knowledge/Wheel/KnowledgeWheel.js")
                .Include("~/Scripts/npm/d3/d3.js")
                .IncludeDirectory("~/Views/Categories/Detail/JsAnalyticsTab/", "*.js")
                .IncludeDirectory("~/Views/Categories/Detail/Js/", "*.js")
                .Include("~/Views/Categories/ResultTestSession/Js/GetResultTestSession.js")
                .Include("~/Scripts/npm/vue-textarea-autosize/vue-textarea-autosize.umd.js")
                .Include("~/Views/Categories/Detail/JsEditMode/CategoryHeader/CategoryImageComponent.js")
                .Include("~/Views/Categories/Detail/JsEditMode/CategoryHeader/CategoryNameComponent.js")
                .Include("~/Views/Categories/Detail/JsEditMode/CategoryHeader/CategoryHeaderApp.js")
                .Include("~/Views/Categories/Edit/Js/DeleteCategory/DeleteCategoryComponent.js")
                .Include("~/Views/Categories/Edit/Js/AddCategory/AddCategoryComponent.js")
                .Include("~/Views/Shared/Editor/EditorMenuBarComponent.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/d3")
                .Include("~/Scripts/npm/d3/d3.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/errorModal")
                .IncludeDirectory("~/Views/Shared/Modals/ErrorModal/","*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/CategorySort")
                .Include("~/Scripts/npm/sortablejs/Sortable.js")
                .Include("~/Scripts/npm/vue-sortable/vue-sortable.js"));


            bundles.Add(new Bundle("~/bundles/js/tiptap")
                .IncludeDirectory("~/Scripts/npm/tiptap-build/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Editor")
                .Include("~/Views/Shared/Editor/EditorMenuBarComponent.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/EditQuestion")
                .Include("~/Views/Questions/Edit/EditComponents/EditQuestionComponent.js")
                .IncludeDirectory("~/Views/Questions/Edit/EditComponents/FlashCard/", "*.js")
                .IncludeDirectory("~/Views/Questions/Edit/EditComponents/MatchList/", "*.js")
                .IncludeDirectory("~/Views/Questions/Edit/EditComponents/MultipleChoice/", "*.js")
                .Include("~/Views/Shared/CategoryChip/CategoryChipComponent.js")
                .IncludeDirectory("~/Views/Questions/Edit/EditComponents/Text/", "*.js")
                .Include("~/Scripts/npm/vue-textarea-autosize/vue-textarea-autosize.umd.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/EditQuestionLoader")
                .Include("~//Views/Questions/Edit/EditComponents/EditQuestionLoader.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/CategoryEditMode")
                .Include("~/Scripts/npm/vue-select/vue-select.js")
                .Include("~/Scripts/npm/vue-sticky-directive/vue-sticky-directive.js")
                .Include("~/Scripts/npm/postscribe/postscribe.js")
                .Include("~/Scripts/npm/vue-float-action-button/vue-fab.js")
                .IncludeDirectory("~/Views/Categories/Detail/JsEditMode/InlineEdit/", "*.js")
                .IncludeDirectory("~/Views/Categories/Detail/JsEditMode/Segmentation/", "*.js")
                .IncludeDirectory("~/Views/Categories/Detail/JsEditMode/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/PublishCategory")
                .IncludeDirectory("~/Views/Categories/Detail/JsEditMode/PublishCategory/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/MyWorldToggle")
                .IncludeDirectory("~/Views/Shared/MyWorldToggle/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/FloatingActionButton")
                .Include("~/Scripts/npm/vue-material-design-ripple-build/vue-material-design-ripple-build.js")
                .Include("~/Views/Categories/Detail/FloatingActionButton/FloatingActionButton.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/js/TopicTabFABLoader")
                .Include("~/Views/Categories/Detail/FloatingActionButton/TopicTabFabLoader.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/LearningTabFABLoader")
                .Include("~/Views/Categories/Detail/FloatingActionButton/LearningTabFabLoader.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/EditQuestionModalLoader")
                .Include("~/Views/Questions/Js/EditQuestionModalLoader/EditQuestionModalLoader.js"));

            bundles.Add(new StyleBundle("~/bundles/CategoryHistory")
                .Include("~/Views/Categories/History/*.css"));

            bundles.Add(new StyleBundle("~/bundles/CategoryHistoryDetail")
                .Include("~/Views/Categories/History/Detail/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/CategoryHistoryDetail")
                .Include("~/Views/Categories/History/Detail/Js/*.js"));

            bundles.Add(new StyleBundle("~/bundles/Login")
                .Include("~/Views/Welcome/Registration/SocialButtons.css"));

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
                //.Include("~/Scripts/vendor.somewhere/simplemde.js")
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

            bundles.Add(new ScriptBundle("~/bundles/js/QuestionDetailsApp")
                .Include("~/Views/Questions/Answer/Js/QuestionDetails/AnswerQuestionDetailsApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/QuestionDetailsComponent")
                .Include("~/Views/Questions/Answer/Js/QuestionDetails/AnswerQuestionDetailsComponent.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/SessionConfig")
                .Include("~/Scripts/npm/vue-slider-component/vue-slider-component.umd.js")
                .IncludeDirectory("~/Views/Questions/Answer/Js/SessionConfig/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/QuestionListApp")
                .Include("~/Views/Questions/Js/QuestionList/QuestionListApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/QuestionListComponents")
                .Include("~/Views/Questions/Js/AddQuestion/AddQuestion.js")
                .Include("~/Scripts/npm/vue-slider-component/vue-slider-component.umd.js")
                .Include("~/Views/Questions/Js/QuestionList/SessionConfig.js")
                .Include("~/Views/Questions/Js/QuestionList/QuestionListComponent.js")
                .Include("~/Views/Questions/Js/QuestionList/QuestionComponent.js")
                .Include("~/Views/Shared/PinComponentVue/PinComponent.vue.js"));


            bundles.Add(new StyleBundle("~/bundles/QuestionList")
                .Include("~/Views/Questions/QuestionList/QuestionList.css"));


            bundles.Add(new StyleBundle("~/bundles/QuestionHistory")
                .Include("~/Views/Questions/History/*.css"));

            bundles.Add(new StyleBundle("~/bundles/QuestionHistoryDetail")
                .Include("~/Views/Questions/History/Detail/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/QuestionHistoryDetail")
                .Include("~/Views/Questions/History/Detail/Js/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/DeleteQuestion")
                .Include("~/Views/Questions/Js/QuestionRowDelete.js"));

            bundles.Add(new StyleBundle("~/bundles/Set")
                .Include("~/Views/Questions/Answer/AnswerQuestion.css")
                .Include("~/Views/Questions/Answer/AnswerQuestionSolution.css")
                .Include("~/Views/Sets/Detail/Set.css"));
                
            bundles.Add(new ScriptBundle("~/bundles/js/Messages")
                .IncludeDirectory("~/Views/Messages/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/LearningSessionResult")
                .IncludeDirectory("~/Views/Questions/Answer/LearningSession/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/WidgetQuestion")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .Include("~/Views/Widgets/WidgetQuestion.js")
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

            bundles.Add(new ScriptBundle("~/bundles/js/MaintenanceCMS")
                .Include("~/Views/Maintenance/Js/CMS.js")
                .Include("~/Views/Maintenance/Js/CmsCategoryNetworkNavigation.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/diff2html")
                .Include("~/Scripts/vendor/diff2html/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/mailto")
                .Include("~/Scripts/various/mailto.js"));

            //-------------------------------------- KnowledgeCentral----------------------------------------

            bundles.Add(new StyleBundle("~/bundles/Knowledge")
                .IncludeDirectory("~/Views/Knowledge/", "*.css")
                .Include("~/Views/Shared/Spinner/Spinner.css "));

            bundles.Add(new ScriptBundle("~/bundles/js/Knowledge")
                .Include("~/Views/Knowledge/Js/Page.js")
                .Include("~/Views/Knowledge/Js/WishKnowledgeContent.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/_dashboard")
                .Include("~/Views/Knowledge/Js/_dashboard.js")); 

            bundles.Add(new StyleBundle("~/bundles/_dashboard")
                .Include("~/Views/Knowledge/Css/_dashBoard.css"));

            bundles.Add(new StyleBundle("~/bundles/KnowledgeTopics")
                .Include("~/Views/Knowledge/Css/KnowledgeTopics.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/KnowledgeQuestions")
                .Include("~/Views/Knowledge/Js/KnowledgeQuestions.js"));

            bundles.Add(new StyleBundle("~/bundles/KnowledgeQuestions")
                .Include("~/Views/Knowledge/Css/KnowledgeQuestions.css"));

            //------------------------ END KNOWLEDGECENTRAL------------------------------------------------------------

            bundles.Add(new ScriptBundle("~/bundles/js/Vue")
                .Include("~/Scripts/vendor/vuetable-2.js")
                .Include("~/Scripts/npm/vue/vue.js"));

            bundles.Add(new StyleBundle("~/bundles/Promoter")
                .Include("~/Views/Welcome/Promoter.css"));

            bundles.Add(new ScriptBundle("~/bundles/AboutMemucho")
                .Include("~/Views/About/Js/Page.js"));

            bundles.Add(new StyleBundle("~/bundles/Team")
                .Include("~/Views/Welcome/Team.css"));

            bundles.Add(new StyleBundle("~/bundles/switch")
                .Include("~/Views/Shared/Switch/Switch.css"));
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
