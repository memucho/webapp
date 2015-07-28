﻿using System;
using System.Collections.Generic;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class UpdateStepExecuter : IRegisterAsInstancePerLifetime
    {
        private readonly DbSettingsRepository _dbSettingsRepository;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExecuter(DbSettingsRepository dbSettingsRepository){
            _dbSettingsRepository = dbSettingsRepository;
        }

        public UpdateStepExecuter Add(int stepNo, Action action)
        {
            _actions.Add(stepNo, action);
            return this;
        }

        public void Run()
        {                
            var dbSettings = _dbSettingsRepository.Get();

            foreach (var dictionaryItem in _actions)
                if (dbSettings.AppVersion < dictionaryItem.Key)
                {
                    Logg.r().Information("update to {0} - START", dictionaryItem.Key);
                    dictionaryItem.Value();
                    Logg.r().Information("update to {0} - END", dictionaryItem.Key);
                    _dbSettingsRepository.UpdateAppVersion(dictionaryItem.Key);
                }   
        }
    }
}