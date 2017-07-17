using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs178
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"CREATE TABLE `ActivityPoints` (
	                    `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `DateEarned` DATETIME NULL DEFAULT NULL,	
	                    `Amount` int(11) null default null,
	                    `User_id` INT(11) NULL DEFAULT NULL,
	                    `ActionType` INT(11) NULL DEFAULT NULL,
	                    `DateCreated` DATETIME NULL DEFAULT NULL,
	                    `DateModified` DATETIME NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`),
	                    INDEX `User_id` (`User_id`)
                        );"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
	                ADD COLUMN `ActivityPoints` INT(11) NOT NULL DEFAULT '0' AFTER `GoogleId`,
	                ADD COLUMN `ActivityLevel` INT(3) NOT NULL DEFAULT '0' AFTER `ActivityPoints`;"
                ).ExecuteUpdate();
        }
    }
}