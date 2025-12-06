using System;
using System.Collections.Generic;
using System.Linq;

public class GameController
{
    private List<Player> players;
    private const int MaxPlayers = 5;
    private Board board;

    private bool jogoIniciado = false;
    private int indiceJogadorAtual = 0;

    public GameController()
    {
        players = new List<Player>();
        board = new Board();
    }

    public Player registarJogador(string nome)
    {
        if (players.Count >= MaxPlayers)
        {
            Console.WriteLine($"Número máximo de jogadores ({MaxPlayers}) atingido.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido.");
            return null;
        }

        foreach (var p in players)
        {
            if (p.Name == nome)
            {
                Console.WriteLine($"Já existe um jogador com o nome '{nome}'.");
                return null;
            }
        }

        var player = new Player(nome);
        players.Add(player);
        Console.WriteLine($"Jogador '{nome}' registado com sucesso.");
        return player;
    }

    // ===== INICIAR JOGO (IJ) =====
    public void IniciarJogo()
    {
        if (players.Count < 2)
        {
            Console.WriteLine("Jogadores insuficientes para iniciar o jogo.");
            return;
        }

        jogoIniciado = true;
        indiceJogadorAtual = 0;
        Console.WriteLine("Jogo iniciado com sucesso.");
    }

    // ===== LANÇAR DADOS (LD) =====
    public void LancarDados(string nomeJogador, int x, int y)
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        var jogador = players.FirstOrDefault(p => p.Name == nomeJogador);
        if (jogador == null)
        {
            Console.WriteLine("Jogador não participa no jogo em curso.");
            return;
        }

        if (players[indiceJogadorAtual] != jogador)
        {
            Console.WriteLine("Não é o turno do jogador.");
            return;
        }

        if (jogador.EstaPreso)
        {
            Console.WriteLine("O jogador está preso.");
            return;
        }

        int pos = jogador.Position;
        int linha = pos / 7;
        int coluna = pos % 7;

        int novaLinha = Math.Max(0, Math.Min(6, linha + y));
        int novaColuna = Math.Max(0, Math.Min(6, coluna + x));

        int novaPosicao = novaLinha * 7 + novaColuna;

        jogador.Position = novaPosicao;
        jogador.LancouDadosEsteTurno = true;

        Console.WriteLine($"{jogador.Name} moveu-se para {board.Spaces[novaPosicao].Name}");
    }

    // ===== COMPRAR ESPAÇO (CE) =====
    public void ComprarEspaco(string nomeJogador)
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        var jogador = players.FirstOrDefault(p => p.Name == nomeJogador);
        if (jogador == null)
        {
            Console.WriteLine("Jogador não participa no jogo em curso.");
            return;
        }

        int pos = jogador.Position;
        Space espacoAtual = board.Spaces[pos];

        if (!espacoAtual.PodeSerComprado)
        {
            Console.WriteLine("Este espaço não está para venda.");
            return;
        }

        if (espacoAtual.Dono != null)
        {
            Console.WriteLine("O espaço já se encontra comprado.");
            return;
        }

        if (jogador.Money < espacoAtual.Preco)
        {
            Console.WriteLine("O jogador não tem dinheiro suficiente para adquirir o espaço.");
            return;
        }

        jogador.Money -= espacoAtual.Preco;
        espacoAtual.Dono = jogador;

        Console.WriteLine("Espaço comprado.");

        // Passa turno depois de comprar
        indiceJogadorAtual = (indiceJogadorAtual + 1) % players.Count;
    }

    public IReadOnlyList<Player> GetPlayers() => players.AsReadOnly();
    public Board GetBoard() => board;
}
