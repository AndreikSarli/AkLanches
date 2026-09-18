namespace AkLanches
{
    class Program
    {
        static void ExibirCabecalho()
        {
            Console.Clear();
            Console.WriteLine("AKLANCHES - SISTEMA DE PEDIDOS");
            Console.WriteLine("--------------------------------------------------\n");
        }

        static void Main(string[] args)
        {
            Pedido pedido = new Pedido();
            bool executando = true;

            while (executando)
            {
                ExibirCabecalho();

                Console.WriteLine($"Itens no carrinho: {pedido.Itens.Count} | Total Parcial: R$ {pedido.CalcularTotal():F2}\n");

                if (pedido.Itens.Count > 0)
                {
                    Console.WriteLine("Itens do Pedido:");
                    for (int i = 0; i < pedido.Itens.Count; i++)
                    {
                        var item = pedido.Itens[i];
                        Console.WriteLine($"  [{i + 1}] {item.Descricao} (SKU: {item.Codigo}) - R$ {item.CalcularPrecoFinal():F2}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("MENU PRINCIPAL");
                Console.WriteLine("1. Adicionar Lanche");
                Console.WriteLine("2. Adicionar Bebida");
                Console.WriteLine("3. Remover Item");
                Console.WriteLine("4. Finalizar Pedido");
                Console.WriteLine("0. Sair");
                Console.Write("\nOpção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1":
                            bool menuLancheAtivo = true;
                            while (menuLancheAtivo)
                            {
                                try
                                {
                                    ExibirCabecalho();
                                    Console.WriteLine("LANCHES\n");
                                    Console.WriteLine("1. X-Burguer (R$ 15,00) [Cod: LAN-101]");
                                    Console.WriteLine("2. X-Salada (R$ 18,00)  [Cod: LAN-102]");
                                    Console.WriteLine("3. X-Bacon (R$ 22,00)   [Cod: LAN-103]");
                                    Console.WriteLine("0. Voltar ao menu principal\n");
                                    Console.Write("Escolha o lanche: ");

                                    string opLanche = Console.ReadLine();

                                    if (opLanche == "0") break;

                                    string codigoLanche = "";
                                    string nomeLanche = "";
                                    double precoLanche = 0.0;

                                    switch (opLanche)
                                    {
                                        case "1":
                                            codigoLanche = "LAN-101";
                                            nomeLanche = "X-Burguer";
                                            precoLanche = 15.00;
                                            break;
                                        case "2":
                                            codigoLanche = "LAN-102";
                                            nomeLanche = "X-Salada";
                                            precoLanche = 18.00;
                                            break;
                                        case "3":
                                            codigoLanche = "LAN-103";
                                            nomeLanche = "X-Bacon";
                                            precoLanche = 22.00;
                                            break;
                                        default:
                                            throw new ArgumentException("Opção de lanche inválida.");
                                    }

                                    Lanche lanche = new Lanche(codigoLanche, nomeLanche, precoLanche);

                                    bool menuAdicionalAtivo = true;
                                    while (menuAdicionalAtivo)
                                    {
                                        try
                                        {
                                            ExibirCabecalho();
                                            Console.WriteLine($"LANCHE SELECIONADO: {lanche.Descricao}\n");
                                            Console.WriteLine("ADICIONAIS (R$ 2,00 cada ingrediente extra)");
                                            Console.WriteLine("1. Queijo Extra");
                                            Console.WriteLine("2. Bacon Extra");
                                            Console.WriteLine("3. Hambúrguer Extra");
                                            Console.WriteLine("4. Sem adicionais");
                                            Console.WriteLine("0. Voltar ao menu principal\n");
                                            Console.Write("Opção: ");

                                            string opAdicional = Console.ReadLine();

                                            if (opAdicional == "0") return;

                                            switch (opAdicional)
                                            {
                                                case "1":
                                                    lanche.Ingredientes.Add("Queijo Extra");
                                                    lanche.Descricao += " + Queijo Extra";
                                                    break;
                                                case "2":
                                                    lanche.Ingredientes.Add("Bacon Extra");
                                                    lanche.Descricao += " + Bacon Extra";
                                                    break;
                                                case "3":
                                                    lanche.Ingredientes.Add("Hambúrguer Extra");
                                                    lanche.Descricao += " + Hambúrguer Extra";
                                                    break;
                                                case "4":
                                                    break;
                                                default:
                                                    throw new ArgumentException("Opção de adicional inválida.");
                                            }

                                            pedido.Itens.Add(lanche);
                                            menuAdicionalAtivo = false;
                                            menuLancheAtivo = false;
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                            Console.ReadKey();
                                        }
                                    }
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                    Console.ReadKey();
                                }
                            }
                            break;

                        case "2":
                            bool menuBebidaAtivo = true;
                            while (menuBebidaAtivo)
                            {
                                try
                                {
                                    ExibirCabecalho();
                                    Console.WriteLine("BEBIDAS\n");
                                    Console.WriteLine("1. Água Mineral (R$ 3,00) [Cod: BEB-201]");
                                    Console.WriteLine("2. Coca-Cola (R$ 5,00)     [Cod: BEB-202]");
                                    Console.WriteLine("3. Suco Natural (R$ 8,00)  [Cod: BEB-203]");
                                    Console.WriteLine("0. Voltar ao menu principal\n");
                                    Console.Write("Escolha a bebida: ");

                                    string opBebida = Console.ReadLine();

                                    if (opBebida == "0") break;

                                    string codigoBebida = "";
                                    string nomeBebida = "";
                                    double precoBaseBebida = 0.0;

                                    switch (opBebida)
                                    {
                                        case "1":
                                            codigoBebida = "BEB-201";
                                            nomeBebida = "Água Mineral";
                                            precoBaseBebida = 3.00;
                                            break;
                                        case "2":
                                            codigoBebida = "BEB-202";
                                            nomeBebida = "Coca-Cola";
                                            precoBaseBebida = 5.00;
                                            break;
                                        case "3":
                                            codigoBebida = "BEB-203";
                                            nomeBebida = "Suco Natural";
                                            precoBaseBebida = 8.00;
                                            break;
                                        default:
                                            throw new ArgumentException("Opção de bebida inválida.");
                                    }

                                    bool menuTamanhoAtivo = true;
                                    while (menuTamanhoAtivo)
                                    {
                                        try
                                        {
                                            ExibirCabecalho();
                                            Console.WriteLine($"TAMANHO ({nomeBebida})\n");
                                            Console.WriteLine("1. Pequeno (Preço padrão)");
                                            Console.WriteLine("2. Médio (+ R$ 1,50)");
                                            Console.WriteLine("3. Grande (+ R$ 3,00)");
                                            Console.WriteLine("0. Voltar ao menu principal\n");
                                            Console.Write("Opção: ");

                                            string opTamanho = Console.ReadLine();

                                            if (opTamanho == "0") return;

                                            string tamanhoTexto = "";

                                            switch (opTamanho)
                                            {
                                                case "1":
                                                    tamanhoTexto = "pequeno";
                                                    break;
                                                case "2":
                                                    tamanhoTexto = "medio";
                                                    break;
                                                case "3":
                                                    tamanhoTexto = "grande";
                                                    break;
                                                default:
                                                    throw new ArgumentException("Opção de tamanho inválida.");
                                            }

                                            Bebida bebida = new Bebida(codigoBebida, $"{nomeBebida} ({tamanhoTexto})", precoBaseBebida, tamanhoTexto);

                                            pedido.Itens.Add(bebida);
                                            menuTamanhoAtivo = false;
                                            menuBebidaAtivo = false;
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                            Console.ReadKey();
                                        }
                                    }
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                    Console.ReadKey();
                                }
                            }
                            break;

                        case "3":
                            bool menuRemocaoAtivo = true;
                            while (menuRemocaoAtivo)
                            {
                                try
                                {
                                    ExibirCabecalho();
                                    Console.WriteLine("REMOVER ITEM\n");

                                    if (pedido.Itens.Count == 0)
                                    {
                                        Console.WriteLine("O carrinho está vazio.");
                                        Console.WriteLine("\nPressione qualquer tecla para voltar...");
                                        Console.ReadKey();
                                        break;
                                    }

                                    Console.WriteLine("Selecione a posição do item que deseja remover:\n");
                                    for (int i = 0; i < pedido.Itens.Count; i++)
                                    {
                                        Console.WriteLine($"  [{i + 1}] {pedido.Itens[i].Descricao} (Cod: {pedido.Itens[i].Codigo})");
                                    }
                                    Console.WriteLine("  [0] Voltar ao menu principal");
                                    Console.Write("\nDigite a posição na lista: ");

                                    string entradaRemocao = Console.ReadLine();

                                    if (entradaRemocao == "0") break;

                                    if (!int.TryParse(entradaRemocao, out int numeroItem) || numeroItem < 1 || numeroItem > pedido.Itens.Count)
                                    {
                                        throw new ArgumentOutOfRangeException(null, "A posição selecionada não existe na lista.");
                                    }

                                    int indiceRemover = numeroItem - 1;
                                    var itemParaRemover = pedido.Itens[indiceRemover];

                                    bool menuConfirmacaoAtivo = true;
                                    while (menuConfirmacaoAtivo)
                                    {
                                        try
                                        {
                                            ExibirCabecalho();
                                            Console.WriteLine("CONFIRMAÇÃO DE REMOÇÃO\n");
                                            Console.WriteLine($"Tem certeza que deseja remover '{itemParaRemover.Descricao}' (Cod: {itemParaRemover.Codigo})?");
                                            Console.WriteLine("1. Sim, remover");
                                            Console.WriteLine("2. Não, cancelar");
                                            Console.Write("\nOpção: ");

                                            string opConfirmacao = Console.ReadLine();

                                            switch (opConfirmacao)
                                            {
                                                case "1":
                                                    pedido.Itens.RemoveAt(indiceRemover);
                                                    Console.WriteLine($"\n'{itemParaRemover.Descricao}' foi removido com sucesso!");
                                                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                                    Console.ReadKey();
                                                    menuConfirmacaoAtivo = false;
                                                    menuRemocaoAtivo = false;
                                                    break;

                                                case "2":
                                                    Console.WriteLine("\nOperação cancelada. O item permanece no carrinho.");
                                                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                                    Console.ReadKey();
                                                    menuConfirmacaoAtivo = false;
                                                    menuRemocaoAtivo = false;
                                                    break;

                                                default:
                                                    throw new ArgumentException("Opção de confirmação inválida. Digite 1 para Sim ou 2 para Não.");
                                            }
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                            Console.ReadKey();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                                    Console.ReadKey();
                                }
                            }
                            break;

                        case "4":
                            ExibirCabecalho();

                            if (pedido.Itens.Count == 0)
                            {
                                Console.WriteLine("O carrinho está vazio. Adicione itens antes de finalizar.");
                                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                                Console.ReadKey();
                                break;
                            }

                            Console.WriteLine("RESUMO DO PEDIDO\n");

                            foreach (var item in pedido.Itens)
                            {
                                Console.WriteLine($"  • {item.Descricao} (Cod: {item.Codigo}): R$ {item.CalcularPrecoFinal():F2}");
                            }

                            Console.WriteLine($"\nTotal: R$ {pedido.CalcularTotal():F2}\n");
                            Console.WriteLine("Pedido enviado com sucesso! Obrigado pela preferência.");
                            Console.WriteLine("\nPressione qualquer tecla para sair...");
                            Console.ReadKey();

                            executando = false;
                            break;

                        case "0":
                            ExibirCabecalho();
                            Console.WriteLine("SISTEMA ENCERRADO\n");
                            Console.WriteLine("Obrigado por utilizar o AKLanches.");
                            Console.WriteLine("\nPressione qualquer tecla para fechar...");
                            Console.ReadKey();

                            executando = false;
                            break;

                        default:
                            throw new ArgumentException("Opção do menu principal inválida.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }
    }
}