using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs196
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
	                ADD INDEX `DateCreated` (`DateCreated`);"
            ).ExecuteUpdate();
        }
    }
}