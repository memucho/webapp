using System.Collections.Generic;
using NHibernate;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Tests
{
    public class CategorySearch : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public CategorySearch(ISession session)
        {
            _session = session;
        }

        public IList<Category> Run(string likeStatement)
        {            
            return _session.CreateQuery("from Category as c where c.Name like :nameLike")
                    .SetString("nameLike", "%" + likeStatement + "%")
                    .List<Category>();
        }
    }
}