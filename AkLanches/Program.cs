namespace AkLanches
{
    class Program
    {
        // Mostra o nome do sistema no começo das telas.
        static void ExibirCabecalho()
        {
            Console.Clear();
            EscreverColorido("AKLANCHES - SISTEMA DE PEDIDOS", ConsoleColor.Cyan);
            Console.WriteLine("--------------------------------------------------\n");
        }

        static void EscreverColorido(string texto, ConsoleColor cor, bool quebrarLinha = true)
        {
            Console.ForegroundColor = cor;
            if (quebrarLinha)
                Console.WriteLine(texto);
            else
                Console.Write(texto);
            Console.ResetColor();
        }

        // Exibe o estado atual do pedido e quanto falta para atingir a regra de desconto.
        // Mostra quantos itens existem e o valor do pedido.
        static void ExibirStatusCarrinhoEIncentivo(Pedido pedido, ItemCardapio itemEmConstrucao = null)
        {
            decimal subtotal = pedido.CalcularSubtotal();

            if (itemEmConstrucao != null)
            {
                subtotal += itemEmConstrucao.CalcularPrecoFinal();
            }

            const decimal META_DESCONTO = 30.00m;
            int quantidadeItens = pedido.Itens.Count + (itemEmConstrucao != null ? 1 : 0);

            Console.Write($"Itens no carrinho: {quantidadeItens} | Subtotal: ");
            EscreverColorido($"R$ {subtotal:F2}", ConsoleColor.White);

            if (subtotal == 0)
            {
                EscreverColorido("[Dica: Pedidos acima de R$ 30,00 ganham 10% de desconto!]", ConsoleColor.DarkGray);
            }
            else if (subtotal < META_DESCONTO)
            {
                decimal quantoFalta = META_DESCONTO - subtotal;
                Console.Write("[Faltam ");
                EscreverColorido($"R$ {quantoFalta:F2}", ConsoleColor.Yellow, false);
                EscreverColorido(" para você LIBERAR 10% DE DESCONTO!]", ConsoleColor.DarkYellow);
            }
            else
            {
                EscreverColorido("[PARABÉNS! Você conquistou 10% de DESCONTO!]", ConsoleColor.Green);
            }

            Console.WriteLine();
        }

        // Aqui começa o programa e o menu principal.
        static void Main(string[] args)
        {
            Pedido pedido = new Pedido();
            bool executando = true;

            while (executando)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    // Só exibe a lista quando há itens; mantém a tela inicial limpa quando o carrinho está vazio.
                    if (pedido.Itens.Count > 0)
                    {
                        Console.WriteLine("Itens do Pedido:");
                        for (int i = 0; i < pedido.Itens.Count; i++)
                        {
                            var item = pedido.Itens[i];
                            string descricaoItem = item is Lanche lanche
                                ? lanche.ObterDescricaoComExtras()
                                : item.Descricao;

                            Console.WriteLine($"   [{i + 1}] {descricaoItem} (Cod: {item.Codigo}) - R$ {item.CalcularPrecoFinal():F2}");
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine("MENU PRINCIPAL");
                    Console.WriteLine("1. Adicionar Lanche");
                    Console.WriteLine("2. Adicionar Bebida");
                    Console.WriteLine("3. Editar Item do Pedido");
                    Console.WriteLine("4. Remover Item do Pedido");
                    Console.WriteLine("5. Finalizar Pedido");
                    Console.WriteLine("0. Sair");
                    Console.Write("\nOpção: ");

                    string opcao = Console.ReadLine() ?? "";

                    switch (opcao)
                    {
                        case "1":
                            MenuAdicionarLanche(pedido);
                            break;
                        case "2":
                            MenuAdicionarBebida(pedido);
                            break;
                        case "3":
                            MenuEditarPedido(pedido);
                            break;
                        case "4":
                            MenuRemoverItem(pedido);
                            break;
                        case "5":
                            MenuFinalizarPedido(pedido);
                            break;
                        case "0":
                            ExibirCabecalho();
                            EscreverColorido("SISTEMA ENCERRADO\n", ConsoleColor.Yellow);
                            Console.WriteLine("Obrigado por utilizar o AKLanches.");
                            Console.WriteLine("\nPressione qualquer tecla para fechar...");
                            Console.ReadKey();
                            executando = false;
                            break;
                        default:
                            throw new ArgumentException("Opção do menu principal inválida.");
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Escolha do lanche e montagem dos adicionais.
        static void MenuAdicionarLanche(Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine("SELEÇÃO DE LANCHE\n");
                    Console.WriteLine("1. X-Burguer (R$ 15,00) [Cod: LAN-101]");
                    Console.WriteLine("2. X-Salada  (R$ 18,00) [Cod: LAN-102]");
                    Console.WriteLine("3. X-Bacon   (R$ 22,00) [Cod: LAN-103]\n");
                    Console.WriteLine("0. Voltar ao Menu Principal\n");
                    Console.Write("Escolha o lanche: ");

                    string opLanche = Console.ReadLine() ?? "";
                    if (opLanche == "0") return;

                    string codigo = opLanche switch
                    {
                        "1" => "LAN-101",
                        "2" => "LAN-102",
                        "3" => "LAN-103",
                        _ => throw new ArgumentException("Opção de lanche inválida.")
                    };

                    string nome = opLanche switch
                    {
                        "1" => "X-Burguer",
                        "2" => "X-Salada",
                        "3" => "X-Bacon",
                        _ => ""
                    };

                    decimal preco = opLanche switch
                    {
                        "1" => 15.00m,
                        "2" => 18.00m,
                        "3" => 22.00m,
                        _ => 0.0m
                    };

                    Lanche lanche = new Lanche(codigo, nome, preco);

                    // O item entra no carrinho antes da customização para o subtotal refletir os adicionais em tempo real.
                    pedido.Itens.Add(lanche);

                    ResultadoNavegacao resultado = MenuCustomizarAdicionaisLanche(lanche, pedido, ehNovoItem: true);

                    if (resultado == ResultadoNavegacao.VoltarTelaAnterior)
                    {
                        // Se o usuário desistir da customização, desfaz a inserção para não deixar um item incompleto no pedido.
                        pedido.Itens.Remove(lanche);
                    }
                    else if (resultado == ResultadoNavegacao.VoltarMenuPrincipal)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        enum ResultadoNavegacao
        {
            VoltarTelaAnterior,
            VoltarMenuPrincipal
        }

        // Tela para colocar ou tirar adicionais do lanche.
        static ResultadoNavegacao MenuCustomizarAdicionaisLanche(Lanche lanche, Pedido pedido, bool ehNovoItem = false)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    int totalAdicionais = lanche.IngredientesExtras.Sum(x => x.Quantidade);

                    Console.WriteLine($"CUSTOMIZANDO LANCHE: {lanche.ObterDescricaoComExtras()}");

                    EscreverColorido($"[Limite de Adicionais: {totalAdicionais}/10 no total]\n", ConsoleColor.Yellow);

                    Console.WriteLine("ADICIONAIS");
                    Console.WriteLine("1. Queijo Extra     (+ R$ 2,00)");
                    Console.WriteLine("2. Bacon Extra      (+ R$ 3,00)");
                    Console.WriteLine("3. Hambúrguer Extra (+ R$ 5,00)");
                    Console.WriteLine("4. Remover um Adicional\n");
                    Console.WriteLine("9. Voltar à tela anterior");
                    Console.WriteLine("0. Voltar ao Menu Principal\n");
                    Console.Write("Opção: ");

                    string opAdicional = Console.ReadLine() ?? "";

                    if (opAdicional == "9") return ResultadoNavegacao.VoltarTelaAnterior;
                    if (opAdicional == "0") return ResultadoNavegacao.VoltarMenuPrincipal;

                    if (opAdicional is "1" or "2" or "3")
                    {
                        if (totalAdicionais >= 10)
                        {
                            throw new InvalidOperationException("Limite máximo de 10 adicionais atingido para este lanche.");
                        }
                    }

                    switch (opAdicional)
                    {
                        case "1":
                            lanche.AdicionarIngrediente("Queijo Extra", 2.00m);
                            break;
                        case "2":
                            lanche.AdicionarIngrediente("Bacon Extra", 3.00m);
                            break;
                        case "3":
                            lanche.AdicionarIngrediente("Hambúrguer Extra", 5.00m);
                            break;
                        case "4":
                            SubMenuRemoverAdicional(lanche, pedido, ehNovoItem);
                            break;
                        default:
                            throw new ArgumentException("Opção de adicional inválida.");
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // Escolha da bebida antes de escolher o tamanho.
        static void MenuAdicionarBebida(Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine("BEBIDAS\n");
                    Console.WriteLine("1. Água Mineral (R$ 3,00) [Cod: BEB-201]");
                    Console.WriteLine("2. Coca-Cola     (R$ 5,00) [Cod: BEB-202]");
                    Console.WriteLine("3. Suco Natural  (R$ 8,00) [Cod: BEB-203]\n");
                    Console.WriteLine("0. Voltar ao Menu Principal\n");
                    Console.Write("Escolha a bebida: ");

                    string opBebida = Console.ReadLine() ?? "";
                    if (opBebida == "0") return;

                    string codigo = opBebida switch
                    {
                        "1" => "BEB-201",
                        "2" => "BEB-202",
                        "3" => "BEB-203",
                        _ => throw new ArgumentException("Opção de bebida inválida.")
                    };

                    string nome = opBebida switch
                    {
                        "1" => "Água Mineral",
                        "2" => "Coca-Cola",
                        "3" => "Suco Natural",
                        _ => ""
                    };

                    decimal precoBase = opBebida switch
                    {
                        "1" => 3.00m,
                        "2" => 5.00m,
                        "3" => 8.00m,
                        _ => 0.0m
                    };

                    ResultadoNavegacao resultado = MenuSelecionarTamanhoBebida(codigo, nome, precoBase, pedido);

                    if (resultado == ResultadoNavegacao.VoltarMenuPrincipal)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Escolha do tamanho da bebida.
        static ResultadoNavegacao MenuSelecionarTamanhoBebida(string codigo, string nome, decimal precoBase, Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine($"TAMANHO ({nome})\n");
                    Console.WriteLine("1. 300ml (Preço padrão)");
                    Console.WriteLine("2. 500ml (+ R$ 1,50)");
                    Console.WriteLine("3. 700ml (+ R$ 2,50)\n");
                    Console.WriteLine("9. Voltar à tela anterior");
                    Console.WriteLine("0. Voltar ao Menu Principal\n");
                    Console.Write("Opção: ");

                    string opTamanho = Console.ReadLine() ?? "";

                    if (opTamanho == "9") return ResultadoNavegacao.VoltarTelaAnterior;
                    if (opTamanho == "0") return ResultadoNavegacao.VoltarMenuPrincipal;

                    string tamanho = opTamanho switch
                    {
                        "1" => "300ml",
                        "2" => "500ml",
                        "3" => "700ml",
                        _ => throw new ArgumentException("Opção de tamanho inválida.")
                    };

                    Bebida bebida = new Bebida(codigo, $"{nome} ({tamanho})", precoBase, tamanho);
                    pedido.Itens.Add(bebida);
                    return ResultadoNavegacao.VoltarMenuPrincipal;
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Aqui o usuário escolhe um item que já está no pedido.
        static void MenuEditarPedido(Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine("EDITAR ITEM DO PEDIDO\n");

                    if (pedido.Itens.Count == 0)
                    {
                        Console.WriteLine("O carrinho está vazio. Não há itens para editar.");
                        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Selecione qual item deseja alterar:\n");
                    for (int i = 0; i < pedido.Itens.Count; i++)
                    {
                        var item = pedido.Itens[i];
                        string desc = item is Lanche lanche ? lanche.ObterDescricaoComExtras() : item.Descricao;
                        Console.WriteLine($"   [{i + 1}] {desc} (Cod: {item.Codigo})");
                    }
                    Console.WriteLine("\n0. Voltar ao Menu Principal");
                    Console.Write("\nOpção: ");

                    string entrada = Console.ReadLine() ?? "";
                    if (entrada == "0") return;

                    if (int.TryParse(entrada, out int pos) && pos >= 1 && pos <= pedido.Itens.Count)
                    {
                        var itemSelecionado = pedido.Itens[pos - 1];

                        if (itemSelecionado is Lanche lanche)
                        {
                            MenuCustomizarAdicionaisLanche(lanche, pedido, ehNovoItem: false);
                        }
                        else if (itemSelecionado is Bebida bebida)
                        {
                            MenuEditarTamanhoBebida(bebida, pedido);
                        }
                        return;
                    }
                    else
                    {
                        throw new ArgumentException("Opção de seleção inválida.");
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Altera somente o tamanho da bebida escolhida.
        static void MenuEditarTamanhoBebida(Bebida bebida, Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine($"ALTERAR TAMANHO DA BEBIDA ({bebida.Descricao})\n");
                    Console.WriteLine("1. 300ml (Preço padrão)");
                    Console.WriteLine("2. 500ml (+ R$ 1,50)");
                    Console.WriteLine("3. 700ml (+ R$ 2,50)\n");
                    Console.WriteLine("9. Voltar à tela anterior");
                    Console.WriteLine("0. Voltar ao Menu Principal\n");
                    Console.Write("Opção: ");

                    string opTamanho = Console.ReadLine() ?? "";
                    if (opTamanho == "9" || opTamanho == "0") return;

                    string novoTamanho = opTamanho switch
                    {
                        "1" => "300ml",
                        "2" => "500ml",
                        "3" => "700ml",
                        _ => throw new ArgumentException("Opção de tamanho inválida.")
                    };

                    bebida.AlterarTamanho(novoTamanho);

                    EscreverColorido("\nTamanho da bebida alterado com sucesso!", ConsoleColor.Green);
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Mostra os extras do lanche para escolher qual tirar.
        static void SubMenuRemoverAdicional(Lanche lanche, Pedido pedido, bool ehNovoItem)
        {
            while (true)
            {
                try
                {
                    if (lanche.IngredientesExtras.Count == 0)
                    {
                        EscreverColorido("\nEste lanche ainda não possui nenhum adicional para remover.", ConsoleColor.Yellow);
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        return;
                    }

                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine("SELECIONE O ADICIONAL PARA REMOVER:\n");
                    for (int i = 0; i < lanche.IngredientesExtras.Count; i++)
                    {
                        var extra = lanche.IngredientesExtras[i];
                        Console.WriteLine($"   [{i + 1}] {extra.Nome} (Qtd: {extra.Quantidade})");
                    }
                    Console.WriteLine("\n9. Voltar à tela anterior");
                    Console.Write("\nOpção: ");

                    string opRemover = Console.ReadLine() ?? "";
                    if (opRemover == "9") return;

                    if (int.TryParse(opRemover, out int pos) && pos >= 1 && pos <= lanche.IngredientesExtras.Count)
                    {
                        string nomeIngrediente = lanche.IngredientesExtras[pos - 1].Nome;
                        lanche.RemoverIngrediente(nomeIngrediente);
                        return;
                    }
                    else
                    {
                        throw new ArgumentException("Posição selecionada inválida.");
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                }
            }
        }

        // Remove um item inteiro do pedido.
        static void MenuRemoverItem(Pedido pedido)
        {
            while (true)
            {
                try
                {
                    ExibirCabecalho();
                    ExibirStatusCarrinhoEIncentivo(pedido);

                    Console.WriteLine("REMOVER ITEM DO PEDIDO\n");

                    if (pedido.Itens.Count == 0)
                    {
                        Console.WriteLine("O carrinho está vazio.");
                        Console.WriteLine("\nPressione qualquer tecla para voltar...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Selecione a posição do item que deseja remover:\n");
                    for (int i = 0; i < pedido.Itens.Count; i++)
                    {
                        var item = pedido.Itens[i];
                        string descricao = item is Lanche lanche ? lanche.ObterDescricaoComExtras() : item.Descricao;
                        Console.WriteLine($"   [{i + 1}] {descricao} (Cod: {item.Codigo})");
                    }
                    Console.WriteLine("\n0. Voltar ao Menu Principal");
                    Console.Write("\nDigite a posição na lista: ");

                    string entrada = Console.ReadLine() ?? "";
                    if (entrada == "0") return;

                    if (!int.TryParse(entrada, out int numeroItem) || numeroItem < 1 || numeroItem > pedido.Itens.Count)
                    {
                        throw new ArgumentOutOfRangeException(null, "A posição selecionada não existe na lista.");
                    }

                    int indice = numeroItem - 1;
                    var itemParaRemover = pedido.Itens[indice];

                    while (true)
                    {
                        try
                        {
                            ExibirCabecalho();
                            ExibirStatusCarrinhoEIncentivo(pedido);

                            Console.WriteLine("CONFIRMAÇÃO DE REMOÇÃO\n");
                            Console.WriteLine($"Tem certeza que deseja remover o item '{itemParaRemover.Descricao}'?");
                            Console.WriteLine("1. Sim, remover");
                            Console.WriteLine("2. Não, cancelar");
                            Console.Write("\nOpção: ");

                            string opConfirmacao = Console.ReadLine() ?? "";

                            if (opConfirmacao == "1")
                            {
                                pedido.Itens.RemoveAt(indice);
                                EscreverColorido("\nItem removido com sucesso!", ConsoleColor.Green);
                                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                Console.ReadKey();
                                return;
                            }
                            else if (opConfirmacao == "2")
                            {
                                Console.WriteLine("\nOperação cancelada.");
                                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                Console.ReadKey();
                                return;
                            }
                            else
                            {
                                throw new ArgumentException("Opção de confirmação inválida.");
                            }
                        }
                        catch (Exception ex)
                        {
                            EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                            Console.ReadKey();
                        }
                    }
                }
                catch (Exception ex)
                {
                    EscreverColorido($"\n[ERRO]: {ex.Message}", ConsoleColor.Red);
                    Console.WriteLine("Pressione qualquer tecla para tentar novamente na tela de remoção...");
                    Console.ReadKey();
                }
            }
        }

        // Mostra o resumo, calcula o desconto e fecha o pedido.
        static void MenuFinalizarPedido(Pedido pedido)
        {
            ExibirCabecalho();
            ExibirStatusCarrinhoEIncentivo(pedido);

            if (pedido.Itens.Count == 0)
            {
                Console.WriteLine("O carrinho está vazio. Adicione itens antes de finalizar.");
                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("RESUMO DO PEDIDO\n");

            foreach (var item in pedido.Itens)
            {
                string descricao = item is Lanche lanche ? lanche.ObterDescricaoComExtras() : item.Descricao;
                Console.WriteLine($"   • {descricao} (Cod: {item.Codigo}): R$ {item.CalcularPrecoFinal():F2}");
            }

            decimal subtotal = pedido.CalcularSubtotal();
            ICalculadorDesconto regraDesconto = new DescontoPedidoGrande();
            decimal valorDesconto = regraDesconto.CalcularDesconto(subtotal);
            decimal totalFinal = pedido.CalcularTotalFinal(regraDesconto);

            Console.WriteLine("--------------------------------------------------");
            Console.Write("Subtotal: R$ ");
            Console.WriteLine($"{subtotal:F2}");

            if (valorDesconto > 0)
            {
                Console.Write("Desconto Aplicado (Promoção > R$ 30,00): ");
                EscreverColorido($"- R$ {valorDesconto:F2}", ConsoleColor.Green);
            }

            Console.Write("Total a Pagar: ");
            EscreverColorido($"R$ {totalFinal:F2}\n", ConsoleColor.Green);

            Console.WriteLine("--------------------------------------------------");
            EscreverColorido("Pedido enviado com sucesso! Obrigado pela preferência.", ConsoleColor.Cyan);
            Console.WriteLine("\nPressione qualquer tecla para iniciar um novo atendimento...");
            Console.ReadKey();


            pedido.Itens.Clear();
        }
    }
}