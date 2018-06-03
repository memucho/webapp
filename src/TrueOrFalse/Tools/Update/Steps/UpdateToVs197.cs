using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs197
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE IF NOT EXISTS `answer_aggregated` (
                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                    `QuestionId` int(11) DEFAULT NULL,
                    `TotalTrue` int(11) DEFAULT NULL,
                    `TotalFalse` int(11) DEFAULT NULL,
                    `UserId` int(11) DEFAULT NULL,
                    `LastUpdated` datetime DEFAULT NULL,
                    PRIMARY KEY (`Id`),
                    KEY `UserId` (`UserId`),
                    KEY `QuestionId` (`QuestionId`)
                ) ENGINE=InnoDb DEFAULT CHARSET=utf8;"
            ).ExecuteUpdate();
        }
    }
}