using JCortica_RPRO.Leonardo.Domain.Entities.André;
using Leonardo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteImpresao.Entities;

namespace JCortica_RPRO.Leonardo.Domain
{
    public static class FactoryLabel
    {
        public static LabelZebra Create(List<string> produtoNomes, Pesagem peso, Lotes lotes)
        {
            var labelItems = new List<LabelItem>();
            
            for (int i = 0; i < 24; i++)
            {
                labelItems.Add(new LabelItem(produtoNomes[i], lotes.NumeroLotes[i], peso.Pesos[i]));
            }

            var label = new LabelZebra();
            label.SetLabelItem(labelItems)
                 .SetDia(peso.Dia)
                 .SetObservacao(peso.Observacao)
                 .SetHora(peso.Hora)
                 .SetResponsavel(peso.Responsavel)
                 .SetNumeroFormula(peso.NumeroFormula)
                 .SetCodigoFormula(peso.CodigoFormula)
                 .SetNomeFormula(peso.NomeFormula)
                 .SetCiclo(peso.Ciclo);

            return label;          
        }
    }
}
