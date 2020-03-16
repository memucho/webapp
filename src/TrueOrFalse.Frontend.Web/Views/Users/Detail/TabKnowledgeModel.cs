using System.Collections.Generic;
using System.Linq;

public class TabKnowledgeModel : BaseModel
{
    public UserTinyModel User;
    public bool IsCurrentUser;

    public IList<Question> WishQuestions;
    public IList<QuestionsInCategory> WishQuestionsCategories;

    public TabKnowledgeModel(UserModel userModel)
    {
        User = userModel.User;
        IsCurrentUser = userModel.IsCurrentUser;

        var valuations = Sl.QuestionValuationRepo
            .GetByUserFromCache(User.Id)
            .QuestionIds().ToList();

        WishQuestions = Resolve<QuestionRepo>().GetByIds(valuations);

        if (!IsCurrentUser)
            WishQuestions = WishQuestions.Where(q => q.Visibility == QuestionVisibility.All).ToList();

        WishQuestionsCategories = WishQuestions.QuestionsInCategories().ToList();
    }
}
