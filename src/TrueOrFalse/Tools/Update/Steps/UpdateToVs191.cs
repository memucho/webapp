using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs191
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `SettingLearningSessionType` INT NULL DEFAULT '0' AFTER `IsWishSession`;"
            ).ExecuteUpdate();
        }
    }
}

