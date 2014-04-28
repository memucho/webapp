﻿using TrueOrFalse;

namespace TrueOrFalse.Updates
{
    public class Update : IRegisterAsInstancePerLifetime
    {
        private readonly UpdateStepExecuter _updateStepExecuter;

        public Update(UpdateStepExecuter updateStepExecuter){
            _updateStepExecuter = updateStepExecuter;
        }

        public void Run()
        {
            _updateStepExecuter
                .Add(36, SolrCoreReload.ReloadUser)
                .Add(37, SolrCoreReload.ReloadSet)
                .Add(38, UpdateToVs038.Run)
                .Add(39, UpdateToVs039.Run)
                .Add(40, SolrCoreReload.ReloadQuestion)
                .Add(41, UpdateToVs041.Run)
                .Add(42, UpdateToVs042.Run)
                .Add(43, UpdateToVs043.Run)
                .Add(44, UpdateToVs044.Run)
                .Add(45, UpdateToVs045.Run)
                .Run();
        }
    }
}