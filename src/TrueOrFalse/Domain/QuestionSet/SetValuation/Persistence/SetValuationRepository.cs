﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SetValuationRepository : RepositoryDb<SetValuation> 
    {
        private readonly SearchIndexSet _searchIndexSet;
        private readonly SetRepository _setRepository;

        public SetValuationRepository(
            ISession session, 
            SearchIndexSet searchIndexSet,
            SetRepository setRepository) : base(session)
        {
            _searchIndexSet = searchIndexSet;
            _setRepository = setRepository;
        }

        public IList<SetValuation> GetBy(int setId)
        {
            return _session.QueryOver<SetValuation>()
                           .Where(s => s.SetId == setId).List<SetValuation>();
        }

        public SetValuation GetBy(int setId, int userId)
        {
            return _session.QueryOver<SetValuation>()
                           .Where(q => q.UserId == userId && q.SetId == setId)
                           .SingleOrDefault();
        }

        public IList<SetValuation> GetBy(IList<int> setIds, int userId)
        {
            if (!setIds.Any())
                return new List<SetValuation>();

            var sb = new StringBuilder();
            sb.Append("SELECT * FROM SetValuation WHERE UserId = " + userId + " ");
            sb.Append("AND (SetId = " + setIds[0]);

            for(int i = 1; i < setIds.Count(); i++){
                sb.Append(" OR SetId = " + setIds[i]);
            }
            sb.Append(")");

            Console.Write(sb.ToString());

            return _session.CreateSQLQuery(sb.ToString())
                           .SetResultTransformer(Transformers.AliasToBean(typeof(SetValuation)))
                           .List<SetValuation>();
        }

        public override void Create(IList<SetValuation> sets)
        {
            base.Create(sets);
            _searchIndexSet.Update(_setRepository.GetByIds(sets.SetIds().ToArray()));
        }

        public override void Create(SetValuation setValuation)
        {
            base.Create(setValuation);
            _searchIndexSet.Update(_setRepository.GetById(setValuation.SetId));
        }

        public override void CreateOrUpdate(SetValuation set)
        {
            base.CreateOrUpdate(set);
            _searchIndexSet.Update(_setRepository.GetById(set.Id));
        }

        public override void Update(SetValuation set)
        {
            base.Update(set);
        }
    }
}