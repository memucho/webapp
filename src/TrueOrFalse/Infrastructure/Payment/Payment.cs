using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class Payment
    {
        public readonly int MaxFreeQuestions = 10; 
        public readonly int MaxFreeTopics = 10; 
        public readonly int MaxFreeKnowledge = 30;


    }

public enum PaymentStatus
{
    Free = 0,
    PremiumPerMonth = 1
}