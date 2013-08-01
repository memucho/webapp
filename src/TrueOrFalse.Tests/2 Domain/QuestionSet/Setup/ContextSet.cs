﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Tests
{
    public class ContextSet : IRegisterAsInstancePerLifetime
    {
        private readonly ContextQuestion _contextQuestion;
        private readonly ContextUser _contextUser;
        private SetRepository _setRepository;

        public List<Set> Sets = new List<Set>();
        
        public ContextSet(ContextUser contextUser,
                          SetRepository setRepository)
        {
            _contextUser = contextUser;
            _contextUser.Add("Some User").Persist();
            _setRepository = setRepository;
        }

        public static ContextSet New()
        {
            return BaseTest.Resolve<ContextSet>();
        }

        public ContextSet AddSet(string name, string text = "")
        {
            var set = new Set();
            set.Name = name;
            set.Text = text;
            set.Creator = _contextUser.AllUsers.First();
            Sets.Add(set);

            return this;
        }


        public ContextSet Persist()
        {
            foreach (var set in Sets)
                _setRepository.Create(set);

            _setRepository.Flush();

            return this;
        }
    }
}
