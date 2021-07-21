using System.Collections.Concurrent;
using System.Web;

public class LearningSessionCache
{
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new ConcurrentDictionary<string, LearningSession>();

    public static void AddOrUpdate(LearningSession learningSession)
    {
  
            _learningSessions.AddOrUpdate(
                HttpContext.Current.Session.SessionID,
                learningSession,
                (a, b) => learningSession
            );
        
    }

    public static LearningSession GetLearningSession()
    {
        _learningSessions.TryGetValue(HttpContext.Current.Session.SessionID, out var learningSession);
        return learningSession;
    }
}