using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client.Impl;
using TrueOrFalse;
using ISession = NHibernate.ISession;

public class BadgeAwardCheckParams
{
    public User CurrentUser;
    public BadgeType BadgeType;

    public BadgeAwardCheckParams(BadgeType badgeType, User user)
    {
        BadgeType = badgeType;
        CurrentUser = user;
    }

    public T R<T>(){return Sl.R<T>();} 

    public int WuWi_Count()
    {
        return Sl.QuestionValuationRepo.GetByUserFromCache(CurrentUser.Id).Count;
    }

    public int WuWi_UserIsCreator()
    {
        return R<ISession>()
            .QueryOver<QuestionValuation>()
            .JoinQueryOver(x => x.Question)
            .JoinQueryOver(x => x.Creator)
            .Where(u => u.Id == CurrentUser.Id)
            .RowCount();
    }

    public int WuWi_OtherIsCreator()
    {
        return R<ISession>()
            .QueryOver<QuestionValuation>()
            .JoinQueryOver(x => x.Question)
            .JoinQueryOver(x => x.Creator)
            .Where(u => u.Id != CurrentUser.Id)
            .RowCount();
    }

    public int WuWi_AddedInLessThan24Hours()
    {
        string query = @"
            SELECT 
	            COUNT(TIMESTAMPDIFF(HOUR, q.DateCreated, qv.DateCreated))
            FROM questionvaluation qv
            LEFT JOIN question q
            ON q.Id = qv.QuestionId
            WHERE TIMESTAMPDIFF(HOUR, q.DateCreated, qv.DateCreated)  <= 24 
            AND qv.UserId = {0}
            AND q.Creator_id != {1}";

        return (int)R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id, CurrentUser.Id))
            .UniqueResult<long>();
    }

    public int AnswerCount()
    {
        return R<AnswerRepo>().Query
            .Where(i => i.UserId == CurrentUser.Id)
            .RowCount();
    }

    public int Questions_MultipleChoice_WithCategories()
    {
        var query = @"SELECT COUNT(*) FROM 
            (
                SELECT COUNT(q.Id)
                FROM question q
                LEFT JOIN categories_to_questions cq
                ON cq.Question_Id = q.Id
                WHERE q.Creator_id = {0}
                AND q.SolutionType = 3 /*MultipleChoice*/
                AND cq.Question_Id is not null
                GROUP BY q.Id
            ) as t";

        return (int)R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<long>();
    }

    public int Questions_WithImages()
    {
        return R<ImageMetaDataRepo>().Query
            .Where(x =>
                x.UserId == CurrentUser.Id &&
                x.Type == ImageType.Question)
            .RowCount();
    }

    public int Questions_MultipleChoice()
    {
        return R<QuestionRepo>().Query
            .Where(q =>
                q.Creator.Id == CurrentUser.Id &&
                q.SolutionType == SolutionType.MultipleChoice_SingleSolution)
            .RowCount();
    }

    public int Questions_InOtherPeopleWuwi()
    {
        return R<QuestionValuationRepo>()
            .Query
            .Where(v => v.User.Id != CurrentUser.Id)
            .JoinQueryOver(x => x.Question)
            .JoinQueryOver(x => x.Creator)
            .Where(u => u.Id == CurrentUser.Id)
            .RowCount();
    }

    public IList<Set> SetsWithAtLeast10Questions()
    {
        string query =
          @"SELECT s.Id
            FROM questionset s
            LEFT JOIN questioninset qs
            ON s.Id = qs.Set_id
            WHERE s.Creator_id = {0}
            GROUP BY qs.Set_id
            HAVING Count(qs.Set_id) > 10";

        var setIds = R<ISession>().CreateSQLQuery(String.Format(query, CurrentUser.Id)).List<int>();

        return R<SetRepo>().GetByIds(setIds.ToArray());
    }


    public bool IsBadgeAwarded(string badgeKey, BadgeAwardCheckParams awardCheckParams)
    {
        var badgeType = BadgeTypes.All().ByKey(badgeKey);
        return badgeType.AwardCheck(awardCheckParams).Success;
    }

    public int GamesPlayed()
    {
        return R<ISession>().QueryOver<Game>()
            .Where(q => q.Players.Any(p => p.User.Id == CurrentUser.Id))
            .RowCount();
    }


    private string GamesPlayerCountQuery()
    {
        string query =
            @"  SELECT * FROM (
	                SELECT (
		                select count(*) 
		                from game_player p_inner 
		                where p_inner.Game_Id = p_outer.Game_Id) as playerCount
	                FROM game_player p_outer
	                WHERE IsCreator = true
	                AND User_id = {0}
                ) t ";

        return query;
    }

    public int GamesWithMoreThan10Players()
    {
        var query = GamesPlayerCountQuery()
            + " WHERE playerCount >= 10";

        return R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<int>();
    }

    public int GamesWithExact2Players()
    {
        var query = GamesPlayerCountQuery()
            + " WHERE playerCount = 2";

        return R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<int>();
    }

    public int GamesWon()
    {
        var games = R<GameRepo>().AllCompletedByUser(CurrentUser.Id);
        return games.Count(g => g.GetWinner().User.Id == CurrentUser.Id);
    }

    public int DatesCreated()
    {
        return R<ISession>().QueryOver<Date>()
            .Where(d => d.User.Id == CurrentUser.Id)
            .RowCount();
    }

    public int UsersFollowing()
    {
        string query = @"
            SELECT COUNT(*)
            FROM  user_to_follower uf
            WHERE uf.Follower_id = {0}
        ";

        return R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<int>();
    }

    public BadgeLevel GetBadgeLevel(int points)
    {
        return BadgeType.Levels
            .OrderByDescending(b => b.PointsNeeded)
            .FirstOrDefault(badgeLevel => points >= badgeLevel.PointsNeeded);
    }

    public int Questions_MaxAddedToCategory()
    {
        string query = @"
            SELECT MAX(countQuestionId) FROM (
	            SELECT cq.Category_id, COUNT(cq.Question_id) AS countQuestionId
	            FROM categories_to_questions cq
	            LEFT JOIN question q
	            ON cq.Question_id = q.Id
	            WHERE q.Creator_id = {0}
	            GROUP BY cq.Category_id
            ) t";

        return R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<int>();
    }

    public int CountDifferentCategoriesAddedToQuestion()
    {
        string query = @"
            SELECT count(distinct category_id)
            FROM categories_to_questions cq
            LEFT JOIN question q
            ON cq.Question_id = q.Id
            WHERE q.Creator_id = {0}";

        return (int)R<ISession>()
            .CreateSQLQuery(String.Format(query, CurrentUser.Id))
            .UniqueResult<long>();
    }
}