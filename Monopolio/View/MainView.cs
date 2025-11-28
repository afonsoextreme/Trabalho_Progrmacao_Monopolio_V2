using System;
using System.Linq;

public static class MainView
{
    public static void Run()
    {
        GameController controller = new GameController();

        Console.WriteLine("Comandos: RJ <nome> (registar jogador), LJ (listar jogadores), SB (mostrar tabuleiro), Q (sair)");
        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string operation = tokens[0].ToUpperInvariant();

            if (operation == "RJ")
            {
                string name = tokens.Length > 1 ? string.Join(' ', tokens.Skip(1)) : string.Empty;
                var player = controller.registarJogador(name);
                if (player != null)
                {
                    Console.WriteLine($"Jogador '{player.Name}' registado com sucesso.");
                }
                else
                {
                    Console.WriteLine("Registo falhou.");
                }
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
                        string state = (p.IsBankrupt ? "Falido" : "Ativo");
                        Console.WriteLine($"[{state}] Jogador {i}: {p.Name} - Dinheiro: {p.Money}");
                        i++;
                    }
                }
            }
            else if (operation == "SB" || operation == "BOARD")
            {
                var board = new Board();
                board.PrintBoard();
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