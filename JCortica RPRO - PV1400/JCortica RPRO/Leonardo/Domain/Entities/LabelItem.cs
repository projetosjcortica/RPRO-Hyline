namespace Leonardo.Domain
{
    public class LabelItem
    {
        public string ProductName { get; set; }
        public int Lote { get; set; } = 0;
        public int Weight { get; set; } = 0;

        public LabelItem(string name, int lote,int weight)
        {
            ProductName = name;
            Lote = lote;
            Weight = weight;
        }

        public LabelItem(string name) 
        { 
            ProductName = name;
        }

        public override bool Equals(object obj)
        {
            // Verificar se o objeto é nulo ou de tipo diferente
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // Fazer cast para o tipo correto
            LabelItem other = (LabelItem)obj;

            // Comparar as propriedades relevantes
            return Lote == other.Lote && Weight == other.Weight;
        }

    }
}
