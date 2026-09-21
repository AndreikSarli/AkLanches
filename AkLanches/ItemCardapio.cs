namespace AkLanches
{

    abstract class ItemCardapio
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoBase { get; set; }

        public ItemCardapio(string codigo, string descricao, decimal precoBase)
        {
            Codigo = codigo;
            Descricao = descricao;
            PrecoBase = precoBase;
        }


        public virtual decimal CalcularPrecoFinal()
        {
            return PrecoBase;
        }
    }


    class Lanche : ItemCardapio
    {
        public List<IngredienteExtra> IngredientesExtras { get; private set; }

        public Lanche(string codigo, string descricao, decimal precoBase)
            : base(codigo, descricao, precoBase)
        {
            IngredientesExtras = new List<IngredienteExtra>();
        }

        // Agora recebe também o preço unitário do adicional
        public void AdicionarIngrediente(string nome, decimal precoUnitario)
        {
            var itemExistente = IngredientesExtras.Find(i => i.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (itemExistente != null)
            {
                if (itemExistente.Quantidade >= 10)
                {
                    throw new ArgumentException($"Limite máximo atingido para '{nome}' (Máx: 10 unidades).");
                }
                itemExistente.Quantidade++;
            }
            else
            {
                IngredientesExtras.Add(new IngredienteExtra(nome, precoUnitario, 1));
            }
        }


        // Decrementa a quantidade de um ingrediente (1 por vez)
        public void RemoverIngrediente(string nome)
        {
            var itemExistente = IngredientesExtras.Find(i => i.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (itemExistente == null)
            {
                throw new ArgumentException($"O ingrediente '{nome}' não está adicionado ao lanche.");
            }

            if (itemExistente.Quantidade > 1)
            {
                itemExistente.Quantidade--;
            }
            else
            {
                IngredientesExtras.Remove(itemExistente);
            }
        }

        // Soma o valor individual de cada ingrediente multiplicado pela sua quantidade
        public override decimal CalcularPrecoFinal()
        {
            decimal valorExtras = 0;
            foreach (var item in IngredientesExtras)
            {
                valorExtras += item.PrecoUnitario * item.Quantidade;
            }

            return PrecoBase + valorExtras;
        }

        public string ObterDescricaoComExtras()
        {
            if (IngredientesExtras.Count == 0) return Descricao;

            List<string> listaFormatada = new List<string>();
            foreach (var item in IngredientesExtras)
            {
                if (item.Quantidade > 1)
                {
                    listaFormatada.Add($"{item.Nome} (x{item.Quantidade})");
                }
                else
                {
                    listaFormatada.Add(item.Nome);
                }
            }

            return $"{Descricao} + [{string.Join(", ", listaFormatada)}]";
        }
    }


    class Bebida : ItemCardapio
    {
        private string _tamanho = "pequeno";

        public string Tamanho
        {
            get { return _tamanho; }
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

        public Bebida(string codigo, string descricao, decimal precoBase, string tamanho = "pequeno")
            : base(codigo, descricao, precoBase)
        {
            Tamanho = tamanho;
        }

        public override decimal CalcularPrecoFinal()
        {
            decimal precoFinal = PrecoBase;

            switch (Tamanho.ToLower())
            {
                case "pequeno":
                    break;
                case "medio":
                    precoFinal += 1.50m;
                    break;
                case "grande":
                    precoFinal += 3.00m;
                    break;
                default:
                    throw new ArgumentException("Tamanho inválido.");
            }

            return precoFinal;
        }
    }
}