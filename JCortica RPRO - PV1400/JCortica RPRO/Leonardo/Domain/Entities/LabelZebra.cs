using Leonardo.Contracts;
using Leonardo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TesteImpresao.Entities
{
    public class LabelZebra : ILabel
    {
        public int Id { get; set; }
        private double Height { get; set; } = 1199;
        private double Width { get; set; } = 799;
        public List<LabelItem> LabelItem { get; set; }
        private string ProductTable { get; set; }
        public string Dia { get; set; }
        public TimeSpan Hora { get; set; }
        public string Ciclo { get; set; }
        public string Responsavel { get; set; }
        public string Observacao { get; set; }
        public string NomeFormula { get; set; }
        public int NumeroFormula { get; set; }
        public int CodigoFormula { get; set; }
        private string LabelCode { get; set; }
       
        public LabelZebra()
        { }

        public LabelZebra SetHeight(double height)
        {
            double heightPoints = height * 2.83465;
            Height = heightPoints;
            return this;
        }
        public LabelZebra SetWidth(double width)
        {
            double widthPoints = width * 2.83465;
            Width = widthPoints;
            return this;
        }
        public LabelZebra SetLabelItem(List<LabelItem> labelItem)
        {
            LabelItem = labelItem;
            return this;
        }

        public string GetLabel()
        {
            return LabelCode;
        }
        public LabelZebra SetResponsavel(string responsavel)
        {
            Responsavel = responsavel;
            return this;
        }
        public LabelZebra SetId(int id)
        {
            Id = id;
            return this;
        }
        public LabelZebra SetObservacao(string observacao)
        {
            Observacao = observacao;
            return this;
        }

        public LabelZebra SetDia(string dia)
        {
            Dia = dia;
            return this;
        }

        public LabelZebra SetHora(TimeSpan hora)
        {
            Hora = hora;
            return this;
        }

        public LabelZebra SetCiclo(string ciclo)
        {
            Ciclo = ciclo;
            return this;
        }
        public LabelZebra SetNomeFormula(string nomeFormula)
        {
            NomeFormula = nomeFormula;
            return this;
        }
        public LabelZebra SetNumeroFormula(int numeroFormula)
        {
            NumeroFormula = numeroFormula;
            return this;
        }
        public LabelZebra SetCodigoFormula(int codigoFormula)
        {
            CodigoFormula = codigoFormula;
            return this;
        }
        public LabelZebra CreateNewLineProductTable(LabelItem labelItem, int index)
        {
            var startTextY = 484;
            var startBorderY = 461;
            string line = $@"
                            ^FO18,{startBorderY + (index * 30)}^GB320,32,2^FS
                            ^FO336,{startBorderY + (index * 30)}^GB224,32,2^FS
                            ^FO558,{startBorderY + (index * 30)}^GB224,32,2^FS
                            ^FT29,{startTextY + (index * 30)}^A0N,17,18^FH\^CI28^FD{labelItem.ProductName}^FS^CI27
                            ^FT413,${startTextY + (index * 30)}^A0N,17,18^FH\^CI28^FD{labelItem.Lote}^FS^CI27
                            ^FT643,${startTextY + (index * 30)}^A0N,17,18^FH\^CI28^FD{labelItem.Weight}^FS^CI27";

            ProductTable += line;
            return this;
        }
        public void CreateLabel()
        {
            var labelIndex = 0;
            foreach (var labelItem in LabelItem)
            {
                if (labelItem.Weight != 0)
                {
                    CreateNewLineProductTable(labelItem, labelIndex);
                    labelIndex++;
                }
            }


            LabelCode = $@"
                        ^XA
                        ~TA000
                        ~JSN
                        ^LT0
                        ^MNN
                        ^MTT
                        ^PON
                        ^PMN
                        ^LH0,0
                        ^JMA
                        ^PR5,5
                        ~SD30
                        ^JUS
                        ^LRN
                        ^CI27
                        ^PA0,1,1,0
                        ^XZ
                        ^XA
                        ^MMT
                        ^PW{Width}
                        ^LL{Height}
                        ^LS0
                        ^FO18,423^GB320,40,2^FS
                        ^FT31,450^A0N,23,23^FH\^CI28^FDProduto^FS^CI27
                        ^FO336,423^GB224,40,2^FS
                        ^FO558,423^GB224,40,2^FS
                        ^FT415,450^A0N,23,23^FH\^CI28^FDLote^FS^CI27
                        ^FT647,450^A0N,23,23^FH\^CI28^FDKg^FS^CI27
                        ^FT18,218^AFN,26,13^FH\^FDResponsável: {Responsavel}^FS
                        ^FT18,359^AFN,26,13^FH\^FDEtiqueta ID: {Id}^FS
                        ^FT18,395^AFN,26,13^FH\^FDObservação: {Observacao}^FS
                        ^FT18,288^AFN,26,13^FH\^FDNúmero Fórmula: {NumeroFormula}^FS
                        ^FT18,324^AFN,26,13^FH\^FDCódigo Fórmula: {CodigoFormula}^FS
                        ^FT18,253^AFN,26,13^FH\^FDNome Fórmula: {NomeFormula}^FS
                        ^FO536,67^GFA,2409,5880,30,:Z64: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un15:057D
                        ^FT18,111^AFN,26,13^FH\^FDData: {Dia}^FS
                        ^FT18,147^AFN,26,13^FH\^FDHorário: {Hora.ToString(@"hh\:mm\:ss")}^FS
                        ^FT18,182^AFN,26,13^FH\^FDCiclo: {Ciclo}^FS
                        ^FT18,72^A0N,51,51^FH\^CI28^FDEtiqueta de Dosagem^FS^CI27
                        {ProductTable}
                        ^PQ1,0,1,Y
                        ^XZ";
        }

        public override bool Equals(object obj)
        {
            // Verificar se o objeto é nulo ou de tipo diferente
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // Fazer cast para o tipo correto
            LabelZebra other = (LabelZebra)obj;

            // Verificar se as listas são nulas ou se têm tamanhos diferentes
            if (LabelItem == null || other.LabelItem == null)
            {
                return LabelItem == other.LabelItem;
            }

            // Comparar as duas listas de forma item a item
            return LabelItem.SequenceEqual(other.LabelItem);
        }

    }
}
