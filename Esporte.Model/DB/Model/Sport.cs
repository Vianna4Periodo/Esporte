using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esporte.Model.DB.Model
{
    public class Sport
    {
        public virtual Guid Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool IsIndividual { get; set; }
        public virtual DateTime DataCadastro { get; set; }
    }

    public class SportMap : ClassMapping<Sport>
    {
        public SportMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));
            Property(x => x.Nome);
            Property(x => x.Descricao);
            Property(x => x.IsIndividual);
            Property(x => x.DataCadastro);
        }
    }
}
