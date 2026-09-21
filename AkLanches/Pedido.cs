namespace AkLanches
{
    class Pedido
    {
        public List<ItemCardapio> Itens { get; private set; }

        public Pedido()
        {
            Itens = new List<ItemCardapio>();
        }

        // Cálculo do valor bruto sem descontos
        public decimal CalcularSubtotal()
        {
            decimal total = 0.0m;
            foreach (var item in Itens)
            {
                total += item.CalcularPrecoFinal();
            }
            return total;
        }
        // Cálculo do valor total do pedido que é o mesmo que o subtotal
        public decimal CalcularTotal()
        {
            return CalcularSubtotal();
        }

        // Recebe qualquer classe que implemente ICalculadorDesconto
        public decimal CalcularTotalFinal(ICalculadorDesconto estrategiaDesconto)
        {
            decimal subtotal = CalcularSubtotal();
            decimal desconto = estrategiaDesconto.CalcularDesconto(subtotal);
            return subtotal - desconto;
        }
    }
}