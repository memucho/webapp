using System;
using System.Net.Mail;
using System.Windows.Forms;
using NHibernate;
using Quartz;
using Serilog;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcTotalWishInOthersPeople : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var start = DateTime.Now;
                var report = GetReport();

                Sl.Resolve<ISession>().CreateSQLQuery(
                        @"UPDATE user SET TotalInOthersWishknowledge = (
                        SELECT count(*) FROM questionvaluation qv
                        JOIN question q
                        ON qv.Questionid = q.Id
                        WHERE qv.RelevancePersonal > 0
                        AND q.Creator_id = user.Id
                        AND qv.UserId <> user.Id);"
                    ).ExecuteUpdate();

                var end = DateTime.Now;
                report += (end - start) + " Sekunden.";

                SendMail("daniel.majunke@googlemail.com", "Daniel", report);
                SendMail("robert@robert - m.de", "Robert", report);

            }, "RecalcTotalWishInOthersPeople");

        }

        private string GetReport()
        {
            var userIds = Sl.UserRepo.GetAllIds();
            var counter = 0;

            foreach (var userId in userIds)
            {
                var userTotalWishKnowledgeInOtherPoeple = Sl.Resolve<ISession>().CreateSQLQuery(@"Select TotalInOthersWishknowledge From User where Id = :userId ").SetParameter("userId", userId).UniqueResult<int>();
                var joinTotalWishKnowledgeInOtherPoeple = Sl.Resolve<ISession>().CreateSQLQuery(
                    @"SELECT count(qv.Id) FROM questionvaluation qv JOIN 
                    question q ON qv.Questionid = q.Id
                    WHERE qv.RelevancePersonal > 0
                    AND q.Creator_id = :userId
                    AND qv.UserId <> :userId; ")
                    .SetParameter("userId", userId)
                    .UniqueResult<long>();
               
                if (userTotalWishKnowledgeInOtherPoeple != joinTotalWishKnowledgeInOtherPoeple)
                        counter++;
            }

            return counter +
                   " Zahlen unterscheiden sich bei der User Tabelle mit der Spalte TotalInOthersWishknowledge, von dem Join über die entsprechenden Tabellen und dauerte ";
        }

        private void SendMail(string to, string name, string report)
        {
            SendEmail.Run(new MailMessage("daniel.majunke@googlemail.com", to,
                "Report TotalWishKnowledge in other people",
                $"Hallo {name}, hier die gewünschten Zahlen: {report}"));
        }
    }
}