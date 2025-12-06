using System;
using System.Collections.Generic;
using System.Linq;

public class GameController
{
    private List<Player> players;
    private const int MaxPlayers = 5;
    private Board board;
    private Random random = new Random();

    private bool jogoIniciado = false;
    private int indiceJogadorAtual = 0;
    private int aluguerPendente = 0;
    private Player donoAluguerPendente = null;
    private bool cartaObrigatoria = false;

    private Player JogadorAtual => players.Count == 0 ? null : players[indiceJogadorAtual];

    public GameController()
    {
        players = new List<Player>();
        board = new Board();
    }

    // ===== REGISTO =====
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

        if (players.Any(p => p.Name == nome))
        {
            Console.WriteLine($"Já existe um jogador com o nome '{nome}'.");
            return null;
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

        if (jogoIniciado)
        {
            Console.WriteLine("Jogo já se encontra em curso.");
            return;
        }

        jogoIniciado = true;
        indiceJogadorAtual = 0;
        ReiniciarEstadoTurno();
        Console.WriteLine($"Jogo iniciado. Turno de {JogadorAtual.Name}.");
    }

    // ===== LANÇAR DADOS (LD - aleatório) =====
    public void LancarDados(string nomeJogador)
    {
        if (!PrepararTurnoPara(nomeJogador)) return;

        var jogador = JogadorAtual;

        int d1 = random.Next(1, 7);
        int d2 = random.Next(1, 7);

        if (jogador.EstaPreso)
        {
            if (d1 == d2)
            {
                jogador.EstaPreso = false;
                jogador.TurnosNaPrisao = 0;
                Console.WriteLine($"{jogador.Name} saiu da prisão com duplos.");
            }
            else
            {
                jogador.TurnosNaPrisao++;
                Console.WriteLine($"{jogador.Name} continua preso.");
                if (jogador.TurnosNaPrisao >= 3)
                {
                    jogador.EstaPreso = false;
                    jogador.TurnosNaPrisao = 0;
                    Console.WriteLine($"{jogador.Name} saiu da prisão após 3 turnos.");
                }
                jogador.LancouDadosEsteTurno = true;
                return;
            }
        }

        int steps = d1 + d2;
        int novaPosicao = (jogador.Position + steps) % board.Spaces.Count;
        MoverPara(jogador, novaPosicao, d1, d2);
    }

    // ===== LANÇAR DADOS (LD X Y) =====
    public void LancarDados(string nomeJogador, int x, int y)
    {
        if (!PrepararTurnoPara(nomeJogador)) return;

        var jogador = JogadorAtual;

        int pos = jogador.Position;
        int linha = pos / 7;
        int coluna = pos % 7;

        int novaLinha = Math.Max(0, Math.Min(6, linha + y));
        int novaColuna = Math.Max(0, Math.Min(6, coluna + x));
        int novaPosicao = novaLinha * 7 + novaColuna;

        MoverPara(jogador, novaPosicao, null, null);
    }

    private void MoverPara(Player jogador, int novaPosicao, int? d1, int? d2)
    {
        jogador.Position = novaPosicao;
        jogador.LancouDadosEsteTurno = true;

        Space espacoAtual = board.Spaces[novaPosicao];

        if (d1.HasValue && d2.HasValue)
            Console.WriteLine($"{jogador.Name} lançou {d1} e {d2} e moveu-se para {espacoAtual.Name}");
        else
            Console.WriteLine($"{jogador.Name} moveu-se para {espacoAtual.Name}");

        if (espacoAtual.Dono != null && espacoAtual.Dono != jogador)
        {
            aluguerPendente = CalcularAluguer(espacoAtual);
            donoAluguerPendente = espacoAtual.Dono;
            Console.WriteLine($"O espaço pertence a {donoAluguerPendente.Name}. Deve pagar aluguer de {aluguerPendente}.");
        }
        else
        {
            aluguerPendente = 0;
            donoAluguerPendente = null;
        }

        cartaObrigatoria = espacoAtual.Name.Equals("Chance", StringComparison.OrdinalIgnoreCase)
                           || espacoAtual.Name.Equals("Community", StringComparison.OrdinalIgnoreCase);

        if (cartaObrigatoria)
            Console.WriteLine("Jogador tem de tirar uma carta (TC).");
    }

    // ===== COMPRAR ESPAÇO (CE) =====
    public void ComprarEspaco(string nomeJogador)
    {
        if (!PrepararTurnoPara(nomeJogador)) return;

        var jogador = JogadorAtual;

        if (!jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("O jogador tem de lançar os dados antes de comprar.");
            return;
        }

        var espaco = board.Spaces[jogador.Position];

        if (!espaco.PodeSerComprado)
        {
            Console.WriteLine("Este espaço não está para venda.");
            return;
        }

        if (espaco.Dono != null)
        {
            Console.WriteLine("O espaço já se encontra comprado.");
            return;
        }

        if (jogador.Money < espaco.Preco)
        {
            Console.WriteLine("O jogador não tem dinheiro suficiente para adquirir o espaço.");
            return;
        }

        jogador.Money -= espaco.Preco;
        espaco.Dono = jogador;

        Console.WriteLine("Espaço comprado.");
    }

    // ===== PAGAR ALUGUER (PA) =====
    public void PagarAluguer()
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        var jogador = JogadorAtual;

        if (aluguerPendente <= 0 || donoAluguerPendente == null)
        {
            Console.WriteLine("Nenhum aluguer pendente.");
            return;
        }

        if (jogador.Money >= aluguerPendente)
        {
            jogador.Pay(aluguerPendente);
            donoAluguerPendente.Receive(aluguerPendente);
            Console.WriteLine("Aluguer pago.");
        }
        else
        {
            int tudo = jogador.Money;
            jogador.Pay(tudo);
            donoAluguerPendente.Receive(tudo);
            jogador.DeclareBankrupt();
            Console.WriteLine($"{jogador.Name} ficou falido.");
        }

        aluguerPendente = 0;
        donoAluguerPendente = null;
    }

    // ===== TIRAR CARTA (TC) =====
    public void TirarCarta()
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        if (!cartaObrigatoria)
        {
            Console.WriteLine("Nenhuma carta por tirar.");
            return;
        }

        AplicarEfeitoCarta(JogadorAtual);
        cartaObrigatoria = false;
    }

    // ===== TERMINAR TURNO (TT) =====
    public void TerminarTurno()
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        var jogador = JogadorAtual;

        if (!jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("É obrigatório lançar os dados (LD) antes de terminar o turno.");
            return;
        }

        if (aluguerPendente > 0)
        {
            Console.WriteLine("Existe aluguer por pagar (PA).");
            return;
        }

        if (cartaObrigatoria)
        {
            Console.WriteLine("Tem de tirar carta (TC) antes de terminar o turno.");
            return;
        }

        AvancarParaProximoJogador();
    }

    public IReadOnlyList<Player> GetPlayers() => players.AsReadOnly();
    public Board GetBoard() => board;

    // ===== Helpers =====

    private bool PrepararTurnoPara(string nomeJogador)
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return false;
        }

        var jogador = players.FirstOrDefault(p => p.Name == nomeJogador);
        if (jogador == null)
        {
            Console.WriteLine("Jogador não participa no jogo em curso.");
            return false;
        }

        if (JogadorAtual != jogador)
        {
            Console.WriteLine("Não é o turno do jogador.");
            return false;
        }

        if (jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("O jogador já lançou os dados neste turno.");
            return false;
        }

        return true;
    }

    private void ReiniciarEstadoTurno()
    {
        if (players.Count == 0) return;

        var jogador = JogadorAtual;
        jogador.LancouDadosEsteTurno = false;
        aluguerPendente = 0;
        donoAluguerPendente = null;
        cartaObrigatoria = false;
    }

    private int CalcularAluguer(Space espaco)
    {
        return Math.Max(10, (int)(espaco.Preco * 0.2));
    }

    private void AvancarParaProximoJogador()
    {
        int vivos = players.Count(p => !p.IsBankrupt);
        if (vivos <= 1)
        {
            var vencedor = players.FirstOrDefault(p => !p.IsBankrupt);
            Console.WriteLine($"Jogo terminou. Vencedor: {(vencedor != null ? vencedor.Name : "nenhum")}");
            jogoIniciado = false;
            return;
        }

        int tentativas = players.Count;
        do
        {
            indiceJogadorAtual = (indiceJogadorAtual + 1) % players.Count;
            tentativas--;
        } while (players[indiceJogadorAtual].IsBankrupt && tentativas > 0);

        ReiniciarEstadoTurno();
        Console.WriteLine($"Agora é a vez de {JogadorAtual.Name}.");
    }

    private void AplicarEfeitoCarta(Player jogador)
    {
        int efeito = random.Next(3);

        switch (efeito)
        {
            case 0:
                jogador.Receive(150);
                Console.WriteLine($"{jogador.Name} recebe 150 da banca.");
                break;
            case 1:
                if (!jogador.Pay(100))
                {
                    jogador.DeclareBankrupt();
                    Console.WriteLine($"{jogador.Name} ficou falido ao pagar 100.");
                }
                else
                {
                    Console.WriteLine($"{jogador.Name} paga 100 à banca.");
                }
                break;
            default:
                jogador.Position = 24;
                Console.WriteLine($"{jogador.Name} volta ao Start.");
                break;
        }
    }
}
