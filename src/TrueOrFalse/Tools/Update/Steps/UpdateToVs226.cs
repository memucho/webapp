using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs226
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
	                ADD COLUMN `PaymentStatus` TINYINT  NOT NULL DEFAULT 0 AFTER `LearningSessionOptions`;"
                ).ExecuteUpdate();
        }
    }
}