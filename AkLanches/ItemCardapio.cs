namespace AkLanches
{
    abstract class ItemCardapio
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double PrecoBase { get; set; }

        public ItemCardapio(string codigo, string descricao, double precoBase)
        {
            Codigo = codigo;
            Descricao = descricao;
            PrecoBase = precoBase;
        }

        public virtual double CalcularPrecoFinal()
        {
            return PrecoBase;
        }
    }

    class Lanche : ItemCardapio
    {
        public List<string> Ingredientes { get; private set; }

        public Lanche(string codigo, string descricao, double precoBase)
            : base(codigo, descricao, precoBase)
        {
            Ingredientes = new List<string>();
        }

        public override double CalcularPrecoFinal()
        {
            return PrecoBase + (Ingredientes.Count * 2.00);
        }
    }

    class Bebida : ItemCardapio
    {
        private string _tamanho = "pequeno";

        public string Tamanho
        {
            get => _tamanho;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Favor informar o tamanho.");
                }

                string val = value.Trim().ToLower();
                if (val != "pequeno" && val != "medio" && val != "grande")
                {
                    throw new ArgumentException("Tamanho inválido. Opções aceitas: pequeno, medio, grande.");
                }

                _tamanho = val;
            }
        }

        public Bebida(string codigo, string descricao, double precoBase, string tamanho = "pequeno")
            : base(codigo, descricao, precoBase)
        {
            Tamanho = tamanho;
        }

        public override double CalcularPrecoFinal()
        {
            double precoFinal = PrecoBase;
            switch (Tamanho.ToLower())
            {
                case "pequeno":
                    break;
                case "medio":
                    precoFinal += 1.50;
                    break;
                case "grande":
                    precoFinal += 3.00;
                    break;
                default:
                    throw new ArgumentException("Tamanho inválido.");
            }
            return precoFinal;
        }
    }
}