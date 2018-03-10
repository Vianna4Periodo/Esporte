using Esporte.Model.DB.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esporte.Model.DB.Repository
{
    public class EsporteRepository : RepositoryBase<Sport>
    {
        public EsporteRepository(ISession session) : base(session)
        {
        }
    }
}
