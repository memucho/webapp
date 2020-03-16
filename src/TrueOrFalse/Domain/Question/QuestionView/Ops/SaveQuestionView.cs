using System;
using System.Threading.Tasks;
using System.Web;
using NHibernate;
using TrueOrFalse.Search;

public class SaveQuestionView : IRegisterAsInstancePerLifetime
{
    private readonly QuestionViewRepository _questionViewRepo;
    private readonly SearchIndexQuestion _searchIndexQuestion;
    private readonly ISession _session;

    public SaveQuestionView(
        QuestionViewRepository questionViewRepo, 
        SearchIndexQuestion searchIndexQuestion,
        ISession session)
    {
        _questionViewRepo = questionViewRepo;
        _searchIndexQuestion = searchIndexQuestion;
        _session = session;
    }

    public void Run(Guid questionViewGuid, Question question, User user, WidgetView widgetView = null)
    {
        Run(questionViewGuid, question, user == null ? -1 : user.Id);
    }

    public void Run(
        Guid questionViewGuid,
        Question question,
        int userId,
        LearningSession learningSession = null,
        Guid learningSessionStepGuid = default(Guid),
        WidgetView widgetView = null)
    {
        if (HttpContext.Current == null)
            return;

        var userAgent = UserAgent.Get();

        if (IsCrawlerRequest.Yes(userAgent))
            return;

        _questionViewRepo.Create(new QuestionView
        {
            Guid = questionViewGuid,
            QuestionId = question.Id,
            UserId = userId,
            Milliseconds = -1,
            UserAgent = userAgent,
            LearningSession = learningSession,
            LearningSessionStepGuid = learningSessionStepGuid,
            WidgetView = widgetView
        });

        var viewCount = _questionViewRepo.GetViewCount(question.Id);

        AsyncExe.Run(() => { _searchIndexQuestion.UpdateQuestionView(question.Id, viewCount, question.Creator?.Id); });
    }

    public void LogOverallTime(Guid guid, int millisencondsSinceQuestionView)
    {
        if (guid == Guid.Empty)
        {
            Logg.r().Warning("Trying to log time for question view with empty guid");
            return;
        }

        var questionView = Sl.R<QuestionViewRepository>().GetByGuid(guid);

        if (questionView == null)
        {
            Logg.r().Warning("Trying to log time for unknown question view (guid unknown)");
            return;
        }

        questionView.Milliseconds = millisencondsSinceQuestionView;
        _questionViewRepo.Update(questionView);
    }
}
