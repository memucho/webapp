﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs2
    {
        public static void Run(){
            ServiceLocator.Resolve<ExecuteSqlFile>().Run("Utilities/Update/Scripts/2-new-total-fields-tbl-question.sql");
        }
    }
}
