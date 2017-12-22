using System.Collections.Generic;
using System.Linq;

public class TabKnowledgeModel : BaseModel
{
    public User User;
    public bool IsCurrentUser;

    public IList<Set> WishSets;
    public IList<Question> WishQuestions;
    public IList<QuestionsInCategory> WishQuestionsCategories;

    public TabKnowledgeModel(UserModel userModel)
    {
        User = userModel.User;
        IsCurrentUser = userModel.IsCurrentUser;

        WishSets = WishSets = Resolve<SetRepo>().GetByIds(
            Resolve<SetValuationRepo>()
                .GetByUser(User.Id)
                .SetIds().ToArray()
            );

        var valuations = Sl.QuestionValuationRepo
            .GetByUserFromCache(User.Id)
            .QuestionIds().ToList();

        WishQuestions = Resolve<QuestionRepo>().GetByIds(valuations);

        if (!IsCurrentUser)
            WishQuestions = WishQuestions.Where(q => q.Visibility == QuestionVisibility.All).ToList();

        WishQuestionsCategories = WishQuestions.QuestionsInCategories().ToList();
    }
}
