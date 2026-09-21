namespace AkLanches
{
    class IngredienteExtra
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public IngredienteExtra(string nome, decimal precoUnitario, int quantidade = 1)
        {
            Nome = nome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }
}