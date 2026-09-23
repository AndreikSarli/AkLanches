namespace AkLanches
{
    // Guarda todos os itens escolhidos no pedido.
    class Pedido
    {
        public List<ItemCardapio> Itens { get; private set; }

        // Começa o pedido com a lista vazia.
        public Pedido()
        {
            Itens = new List<ItemCardapio>();
        }

        // Soma o preço de todos os itens.
        public decimal CalcularSubtotal()
        {
            decimal total = 0.0m;

            foreach (var item in Itens)
            {
                total += item.CalcularPrecoFinal();
            }

            return total;
        }

        // Retorna o valor do pedido antes do desconto.
        public decimal CalcularTotal()
        {
            return CalcularSubtotal();
        }

        // Calcula o valor final depois do desconto.
        public decimal CalcularTotalFinal(ICalculadorDesconto estrategiaDesconto)
        {
            decimal subtotal = CalcularSubtotal();
            decimal desconto = estrategiaDesconto.CalcularDesconto(subtotal);

            return subtotal - desconto;
        }
    }
}
