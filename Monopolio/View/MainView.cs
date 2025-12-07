using System;
using System.Linq;

public static class MainView
{
    public static void Run()
    {
        GameController controller = new GameController();

        while (true)
        {
            // Mostrar comandos consoante o estado do jogo
            if (controller.IsGameStarted())
            {
                Console.WriteLine("Comandos: LD <nome>, CE <nome>, TT, S, Q");
            }
            else
            {
                Console.WriteLine("Comandos: RJ <nome>, IJ, S, LJ, SB, Q");
            }

            Console.Write("> ");
            string line = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string operation = tokens[0].ToUpperInvariant();

            if (operation == "RJ")
            {
                string name = tokens.Length > 1
                    ? string.Join(' ', tokens.Skip(1))
                    : string.Empty;

                controller.registarJogador(name);
            }
            else if (operation == "IJ")
            {
                controller.IniciarJogo();
            }
            else if (operation == "LD")
            {
                if (tokens.Length < 2)
                {
                    Console.WriteLine("Uso correto: LD Nome");
                }
                else
                {
                    string nome = string.Join(' ', tokens.Skip(1));
                    controller.LancarDados(nome);
                }
            }
            else if (operation == "CE")
            {
                if (tokens.Length < 2)
                {
                    Console.WriteLine("Uso correto: CE Nome");
                }
                else
                {
                    string nome = string.Join(' ', tokens.Skip(1));
                    controller.ComprarEspaco(nome);
                }
            }
            else if (operation == "S")
            {
                if (!controller.IsGameStarted())
                {
                    var players = controller.GetPlayers();
                    if (players.Count == 0)
                    {
                        Console.WriteLine("Nenhum jogador registado.");
                    }
                    else
                    {
                        Console.WriteLine("Jogadores registados:");
                        int i = 1;
                        foreach (var p in players)
                        {
                            string state = p.IsBankrupt ? "Falido" : "Ativo";
                            Console.WriteLine($"[{state}] Jogador {i}: {p.Name} - Dinheiro: {p.Money} - Pos: {p.Position}");
                            i++;
                        }
                    }
                }
                else
                {
                    var cur = controller.GetCurrentPlayer();
                    if (cur == null)
                    {
                        Console.WriteLine("Nenhum jogador ativo no jogo.");
                    }
                    else
                    {
                        var board = controller.GetBoard();
                        string spaceName = board.GetSpaceAt(cur.Position).Name;
                        Console.WriteLine($"Jogador atual: {cur.Name}");
                        Console.WriteLine($"  Dinheiro: {cur.Money}");
                        Console.WriteLine($"  Posição: {cur.Position} ({spaceName})");
                        Console.WriteLine($"  Preso: {cur.EstaPreso}");
                        Console.WriteLine($"  Lancou dados este turno: {cur.LancouDadosEsteTurno}");
                        int pending = controller.GetPendingRent();
                        if (pending > 0)
                        {
                            var owner = controller.GetRentOwner();
                            string ownerName = owner != null ? owner.Name : "desconhecido";
                            Console.WriteLine($"  Aluguer pendente: {pending} (a pagar a {ownerName})");
                        }
                        if (controller.IsCardMandatory())
                        {
                            Console.WriteLine("  Tem de tirar carta (TC).");
                        }
                    }
                }
            }
            else if (operation == "PA")
            {
                controller.PagarAluguer();
            }
            else if (operation == "TC")
            {
                controller.TirarCarta();
            }
            else if (operation == "TT")
            {
                controller.TerminarTurno();
            }
            else if (operation == "LJ")
            {
                var players = controller.GetPlayers();
                if (players.Count == 0)
                {
                    Console.WriteLine("Nenhum jogador registado.");
                }
                else
                {
                    int i = 1;
                    foreach (var p in players)
                    {
                        string state = p.IsBankrupt ? "Falido" : "Ativo";
                        Console.WriteLine($"[{state}] Jogador {i}: {p.Name} - Dinheiro: {p.Money}");
                        i++;
                    }
                }
            }
            else if (operation == "SB" || operation == "BOARD")
            {
                controller.GetBoard().PrintBoard();
            }
            else if (operation == "Q" || operation == "QUIT")
            {
                Console.WriteLine("A sair...");
                break;
            }
            else
            {
                Console.WriteLine("Comando desconhecido.");
            }
        }
    }
}
