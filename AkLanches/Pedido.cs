namespace AkLanches
{
    class Pedido
    {
        public List<ItemCardapio> Itens { get; set; } = new List<ItemCardapio>();

        public double CalcularTotal()
        {
            double total = 0.0;
            foreach (var item in Itens)
            {
                total += item.CalcularPrecoFinal();
            }
            return total;
        }


    }
}
