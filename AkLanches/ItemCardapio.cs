namespace AkLanches
{

    // Dados que todo item do cardápio tem.
    abstract class ItemCardapio
    {
        public string Codigo { get; private set; }
        public string Descricao { get; protected set; }
        public decimal PrecoBase { get; private set; }

        public ItemCardapio(string codigo, string descricao, decimal precoBase)
        {
            Codigo = codigo;
            Descricao = descricao;
            PrecoBase = precoBase;
        }


        // Cada tipo de item pode calcular o próprio preço.
        public virtual decimal CalcularPrecoFinal()
        {
            return PrecoBase;
        }
    }


    // Parte dos lanches e dos adicionais.
    class Lanche : ItemCardapio
    {
        public List<IngredienteExtra> IngredientesExtras { get; private set; }

        public Lanche(string codigo, string descricao, decimal precoBase)
            : base(codigo, descricao, precoBase)
        {
            IngredientesExtras = new List<IngredienteExtra>();
        }

        // Agora recebe também o preço unitário do adicional
        // Adiciona um extra ao lanche.
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
        // Remove uma unidade do extra escolhido.
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
        // Soma o preço do lanche com os extras.
        // O tamanho escolhido pode aumentar o preço.
        public override decimal CalcularPrecoFinal()
        {
            decimal valorExtras = 0;
            foreach (var item in IngredientesExtras)
            {
                valorExtras += item.PrecoUnitario * item.Quantidade;
            }

            return PrecoBase + valorExtras;
        }

        // Monta a descrição do lanche com os extras escolhidos.
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


    // Parte das bebidas e dos tamanhos.
    class Bebida : ItemCardapio
    {
        // Tamanho usado quando a bebida é criada.
        private string _tamanho = "300ml";

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

                if (val != "300ml" && val != "500ml" && val != "700ml")
                {
                    throw new ArgumentException(
                        "Tamanho inválido. Opções aceitas: 300ml, 500ml e 700ml.");
                }

                _tamanho = val;
            }
        }

        public Bebida(string codigo, string descricao, decimal precoBase, string tamanho = "300ml")
            : base(codigo, descricao, precoBase)
        {
            Tamanho = tamanho;
        }

        public override decimal CalcularPrecoFinal()
        {
            decimal precoFinal = PrecoBase;

            switch (Tamanho.ToLower())
            {
                case "300ml":
                    break;
                case "500ml":
                    precoFinal += 1.50m;
                    break;
                case "700ml":
                    precoFinal += 2.50m;
                    break;
                default:
                    throw new ArgumentException("Tamanho inválido.");
            }

            return precoFinal;
        }

        // Usado quando o tamanho da bebida é alterado no pedido.
        public void AlterarTamanho(string novoTamanho)
        {
            Tamanho = novoTamanho;

            string nomeBase = Descricao.Split('(')[0].Trim();
            Descricao = $"{nomeBase} ({Tamanho})";
        }
    }
}