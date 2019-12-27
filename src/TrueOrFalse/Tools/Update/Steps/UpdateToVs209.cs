
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs209
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            session
                .CreateSQLQuery(
                    @"ALTER TABLE `questionvaluation` ADD INDEX `userId` (`UserId` ASC) VISIBLE;"
                ).ExecuteUpdate();

        }
    }
}