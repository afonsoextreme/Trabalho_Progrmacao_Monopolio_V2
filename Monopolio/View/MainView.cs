using System;
using System.Linq;

public static class MainView
{
    public static void Run()
    {
        GameController controller = new GameController();

        Console.WriteLine("Comandos: RJ <nome>, IJ <nome1> <nome2> [<nome3>] [<nome4>] [<nome5>], LD, PA, TC, TT, CE, CC <NomeJogador> <NomeEspaço> <numeroCasas>, LJ, DJ, S");

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string operation = tokens[0].ToUpperInvariant();

            if (operation == "RJ")
            {
                string name = tokens.Length > 1
                    ? string.Join(' ', tokens.Skip(1))
                    : string.Empty;

                controller.registarJogador(name);
                Console.WriteLine("Comandos: RJ <nome>, IJ <nome1> <nome2> [<nome3>] [<nome4>] [<nome5>], LD, PA, TC, TT, CE, CC <NomeJogador> <NomeEspaço> <numeroCasas>, LJ, DJ, S");
            }
            else if (operation == "IJ")
            {
                if (tokens.Length < 3)
                {
                    Console.WriteLine("Uso correto: IJ <nome1> <nome2> [<nome3>] [<nome4>] [<nome5>]");
                }
                else
                {
                    var nomesJogadores = tokens.Skip(1).ToList();
                    controller.IniciarJogo(nomesJogadores);
                }
            }
            else if (operation == "LD")
            {
                if (tokens.Length == 1)
                {
                    controller.LancarDados();
                }
                else
                {
                    string nome = string.Join(' ', tokens.Skip(1));
                    controller.LancarDados(nome);
                }
            }
            else if (operation == "CE")
            {
                if (tokens.Length == 1)
                {
                    controller.ComprarEspaco();
                }
                else
                {
                    string nome = string.Join(' ', tokens.Skip(1));
                    controller.ComprarEspaco(nome);
                }
            }
            else if (operation == "PA")
            {
                controller.PagarAluguer();
            }
            else if (operation == "CC")
            {
                if (tokens.Length < 4 || !int.TryParse(tokens[3], out int nCasas))
                {
                    Console.WriteLine("Uso correto: CC <NomeJogador> <NomeEspaço> <numeroCasas>");
                }
                else
                {
                    string nomeJogador = tokens[1];
                    string nomeEspaco = tokens[2];
                    controller.ComprarCasa(nomeJogador, nomeEspaco, nCasas);
                }
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
                    Console.WriteLine("NomeJogador NumJogos NumVitórias NumEmpates NumDerrotas");
                    foreach (var p in players)
                    {
                        Console.WriteLine($"{p.Name} {p.NumJogos} {p.NumVitórias} {p.NumEmpates} {p.NumDerrotas}");
                    }
                }
            }
            else if (operation == "DJ" || operation == "BOARD")
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
