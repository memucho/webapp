using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs198
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `LastLogin` TIMESTAMP NULL DEFAULT NULL AFTER `WidgetHostsSpaceSeparated`;"
            ).ExecuteUpdate();
        }
    }
}