//using System;
//using System.Collections.Generic;

//public class CardQuestionAuthorModel : BaseModel
//{
//    public Question Question;
//    public Author Author;
//    public List<Author> Authors;
//    public User User;
//    public QuestionRepo QuestionRepo;
//    public ReputationCalcResult Reputation;
//    public bool DoIFollow;
//    public int AmountWishCountQuestions;
//    public bool IsCurrentUser;

//    public CardQuestionAuthorModel(Question question)
//    {
//        Authors = new List<Author>();
//        QuestionRepo = Sl.QuestionRepo;

//        var _questionRepo = R<QuestionRepo>();
//        var authors = _questionRepo.GetAuthorsQuestion(question.Id);

//        foreach (var author in authors)
//        {
//           Authors.Add(new Author
//            {
//                ImageUrl = new UserImageSettings(author.Id).GetUrl_250px(author).Url,
//                User = author,
//                Reputation = author.Reputation,
//                ReputationPos = author.ReputationPos
//            });
//        }

//        if (Authors.Count == 1)
//        {
//            Reputation = Resolve<ReputationCalc>().Run(authors[0]);
//            var followerIAm = R<FollowerIAm>().Init(new List<int> { authors[0].Id }, UserId);
//            DoIFollow = followerIAm.Of(authors[0].Id); SidebarModel.Reputation = Reputation;
//            AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(authors[0].Id);
//            SidebarModel.IsCurrentUser = authors[0].Id == UserId && IsLoggedIn;
//            IsCurrentUser = authors[0].Id == UserId && IsLoggedIn;
//        }
//    }


//}

//public class Author
//{
//    public string Name;
//    public string ImageUrl;
//    public User User;
//    public Boolean ShowWishKnowledge;
//    public int Reputation;
//    public int ReputationPos;
//}