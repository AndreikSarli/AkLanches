namespace AkLanches
{
    abstract class ItemCardapio
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public double PrecoBase { get; set; }

        public virtual double CalcularPrecoFinal()
        {
            return PrecoBase;
        }
    }



    class Lanche : ItemCardapio
    {
        public List<string> Ingredientes { get; set; } = new List<string>();

        public override double CalcularPrecoFinal()
        {
            double precoFinal = PrecoBase;

            precoFinal += Ingredientes.Count * 2.00;
            return precoFinal;
        }
    }


    class Bebida : ItemCardapio
    {

        private string _tamanho = "pequeno"; // Valor inicial para evitar null
        public string Tamanho
        {
            get => _tamanho;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Favor informar o tamanho");

                }
                _tamanho = value;

            }

        }

        public override double CalcularPrecoFinal()
        {
            double precoFinal = PrecoBase;
            switch (Tamanho.ToLower())
            {
                case "pequeno":
                    precoFinal += 0.00;
                    break;
                case "medio":
                    precoFinal += 1.50;
                    break;
                case "grande":
                    precoFinal += 3.00;
                    break;
                default:
                    throw new ArgumentException("Tamanho inválido");
            }
            return precoFinal;
        }
    }


}
