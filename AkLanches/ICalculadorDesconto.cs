namespace AkLanches
{
    // Regra que o desconto vai seguir.
    interface ICalculadorDesconto
    {
        decimal CalcularDesconto(decimal valorTotal);
    }

    // Quando não tem desconto.
    class SemDesconto : ICalculadorDesconto
    {
        public decimal CalcularDesconto(decimal valorTotal)
        {
            return 0.0m;
        }
    }

    // Desconto usado quando o pedido chega em R$ 30,00.
    class DescontoPedidoGrande : ICalculadorDesconto
    {
        public decimal CalcularDesconto(decimal valorTotal)
        {
            if (valorTotal >= 30.0m)
            {
                return valorTotal * 0.10m; // 10% de desconto
            }

            return 0.0m;
        }
    }
}
