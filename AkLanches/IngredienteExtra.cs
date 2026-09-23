namespace AkLanches
{
    // Guarda os dados de cada ingrediente extra.
    class IngredienteExtra
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        // Cria o extra com nome, preço e quantidade.
        public IngredienteExtra(string nome, decimal precoUnitario, int quantidade = 1)
        {
            Nome = nome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }
}
