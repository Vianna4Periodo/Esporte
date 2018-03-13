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
        public IList<Sport> GetAllByName(String nome)
        {
            try
            {
                return this.Session.Query<Sport>().Where(w => w.Nome.ToLower() == nome.Trim().ToLower()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("The pearson " + nome + " was not found! Error: ", ex);
            }
        }
    }
}
