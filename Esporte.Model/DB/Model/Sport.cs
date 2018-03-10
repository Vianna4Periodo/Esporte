using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esporte.Model.DB.Model
{
    public class Sport
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsIndividual { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
