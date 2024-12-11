using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCortica_RPRO.Leonardo.Domain.Entities.André
{
    public class Pesagem
    {
        public string Dia { get; set; }
        public TimeSpan Hora { get; set; }
        public string Responsavel { get; set; }
        public string Observacao { get; set; }
        public string Ciclo { get; set; }
        public string NomeFormula { get; set; }
        public int NumeroFormula { get; set; }
        public int CodigoFormula { get; set; }
        public List<int> Pesos { get; set; }   
        public bool Valida { get; set; }
    }
}
