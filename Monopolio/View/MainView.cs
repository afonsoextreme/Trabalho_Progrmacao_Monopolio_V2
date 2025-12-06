using System;
using System.Linq;

public static class MainView
{
    public static void Run()
    {
        GameController controller = new GameController();

        Console.WriteLine("Comandos: RJ <nome>, IJ, LD <nome> <x> <y>, CE <nome>, LJ, SB, Q");

        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();
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
                if (tokens.Length < 4)
                {
                    Console.WriteLine("Uso correto: LD Nome X Y");
                }
                else
                {
                    string nome = tokens[1];
                    int x = int.Parse(tokens[2]);
                    int y = int.Parse(tokens[3]);

                    controller.LancarDados(nome, x, y);
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
