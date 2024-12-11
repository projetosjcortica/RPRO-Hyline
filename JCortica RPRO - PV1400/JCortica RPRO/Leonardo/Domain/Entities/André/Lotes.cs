using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCortica_RPRO.Leonardo.Domain.Entities.André
{
    public class Lotes
    {
        public string Dia { get; set; }
        public TimeSpan Hora { get; set; }
        public List<int> NumeroLotes { get; set; }
        public bool Valida { get; set; }
    }
}
