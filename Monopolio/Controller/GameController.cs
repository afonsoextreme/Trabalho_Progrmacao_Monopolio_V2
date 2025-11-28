using System;
using System.Collections.Generic;

public class GameController
{
    // Lista de jogadores registados no jogo
    private List<Player> players;
    private const int MaxPlayers = 5;
    private Board board;

    // Regista um jogador com o nome fornecido e retorna o objecto Player criado.
    // Se já existir um jogador com o mesmo nome, devolve null.

    public GameController()
    {
        players = new List<Player>();
        board = new Board();
    }
    public Player registarJogador(string nome)
    {
        // Não permitir mais do que MaxPlayers
        if (players.Count >= MaxPlayers)
        {
            ConsoleView.ShowError($"Número máximo de jogadores ({MaxPlayers}) atingido.");
            return null;
        }

        // Se o nome não for passado, pedir ao utilizador (usa ConsoleView)
        if (string.IsNullOrWhiteSpace(nome))
        {
            nome = ConsoleView.Prompt("Introduza o nome do jogador: ");
        }

        // Validação final: nome ainda inválido?
        if (string.IsNullOrWhiteSpace(nome))
        {
            ConsoleView.ShowError("Nome inválido.");
            return null;
        }

        // Evitar nomes duplicados
        foreach (var p in players)
        {
            if (p.Name == nome)
            {
                ConsoleView.ShowError($"Já existe um jogador com o nome '{nome}'.");
                return null;
            }
        }

        var player = new Player(nome);
        players.Add(player);
        ConsoleView.ShowInfo($"Jogador '{nome}' registado com sucesso.");
        return player;
    }

    // Permite obter a lista (somente leitura) de jogadores registados
    public IReadOnlyList<Player> GetPlayers() => players.AsReadOnly();

    // Aceder ao tabuleiro do jogo (instância única)
    public Board GetBoard() => board;

    // Imprime o tabuleiro actual
    public void PrintBoard()
    {
        board.PrintBoard();
    }
}
