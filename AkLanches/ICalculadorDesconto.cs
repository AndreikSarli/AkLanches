namespace AkLanches
{
    // Contrato para cálculo de desconto
    interface ICalculadorDesconto
    {
        decimal CalcularDesconto(decimal valorTotal);
    }

    // Pedidos sem desconto
    class SemDesconto : ICalculadorDesconto
    {
        public decimal CalcularDesconto(decimal valorTotal)
        {
            return 0.0m;
        }
    }

    // Desconto de 10% para pedidos a partir de R$ 30,00
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
