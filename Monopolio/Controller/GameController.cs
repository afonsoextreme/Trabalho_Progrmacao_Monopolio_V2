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
    private Player? donoAluguerPendente = null;
    private bool cartaObrigatoria = false;

    private Player? JogadorAtual => players.Count == 0 ? null : players[indiceJogadorAtual];

    public GameController()
    {
        players = new List<Player>();
        board = new Board();
    }

    // ===== REGISTOJOGADOR(RJ) =====
    public Player? registarJogador(string nome)
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
        if (JogadorAtual != null)
            Console.WriteLine($"Jogo iniciado. Turno de {JogadorAtual.Name}.");
    }

    // ===== LANÇAR DADOS (LD) =====
    public void LancarDados()
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        if (JogadorAtual == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

        LancarDados(JogadorAtual.Name);
    }

    public void LancarDados(string nomeJogador)
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

        if (jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("O jogador já lançou os dados neste turno.");
            return;
        }

        if (jogador.EstaPreso)
        {
            Console.WriteLine("O jogador está preso.");
            return;
        }

        int pos = jogador.Position;
        int roll1 = random.Next(1, 7);
        int roll2 = random.Next(1, 7);
        int steps = roll1 + roll2;
        int novaPosicao = (pos + steps) % board.Spaces.Count;

        jogador.Position = novaPosicao;
        jogador.LancouDadosEsteTurno = true;

        Space espacoAtual = board.Spaces[novaPosicao];
        Console.WriteLine($"{jogador.Name} lançou {roll1} e {roll2} e moveu-se para {espacoAtual.Name}");

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
        {
            Console.WriteLine("Jogador tem de tirar uma carta (TC).");
        }
    }

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

        if (jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("O jogador já lançou os dados neste turno.");
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

        Space espacoAtual = board.Spaces[novaPosicao];
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
        {
            Console.WriteLine("Jogador tem de tirar uma carta (TC).");
        }
    }

    // ===== COMPRAR ESPAÇO (CE) =====
    public void ComprarEspaco()
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        if (JogadorAtual == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

        ComprarEspaco(JogadorAtual.Name);
    }

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

        if (players[indiceJogadorAtual] != jogador)
        {
            Console.WriteLine("Não é o turno do jogador.");
            return;
        }

        if (!jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("O jogador tem de lançar os dados antes de comprar.");
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
        TerminarTurno();
    }

    // ===== Comprar Casa (CC) =====
    public void ComprarCasa(int numeroCasas)
    {
        if (!jogoIniciado)
        {
            Console.WriteLine("Não existe um jogo em curso.");
            return;
        }

        var jogador = JogadorAtual;
        if (jogador == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

        if (numeroCasas <= 0)
        {
            Console.WriteLine("Número de casas inválido.");
            return;
        }

        int pos = jogador.Position;
        var espaco = board.Spaces[pos];

        if (!espaco.PodeSerComprado)
        {
            Console.WriteLine("Este espaço não permite construção de casas.");
            return;
        }

        if (string.IsNullOrEmpty(espaco.Color))
        {
            Console.WriteLine("Não é possível construir casas neste tipo de espaço.");
            return;
        }

        if (espaco.Dono != jogador)
        {
            Console.WriteLine("Só o dono do espaço pode comprar casas aqui.");
            return;
        }

        if (!jogador.LancouDadosEsteTurno)
        {
            Console.WriteLine("É preciso lançar os dados antes de comprar casas.");
            return;
        }

        var espacosMesmaCor = board.Spaces.Where(s => s.Color == espaco.Color && s.PodeSerComprado);
        if (!espacosMesmaCor.Any() || !espacosMesmaCor.All(s => s.Dono == jogador))
        {
            Console.WriteLine("Para comprar casas, o jogador precisa ser dono de todos os espaços desta cor.");
            return;
        }

        if (espaco.Casas >= espaco.MaxCasas)
        {
            Console.WriteLine("Limite de casas atingido neste espaço.");
            return;
        }

        int casasPossiveis = Math.Min(numeroCasas, espaco.MaxCasas - espaco.Casas);
        int custoPorCasa = (int)Math.Round(espaco.Preco * 0.6, MidpointRounding.AwayFromZero);
        int custoTotal = casasPossiveis * custoPorCasa;

        if (jogador.Money < custoTotal)
        {
            Console.WriteLine("Saldo insuficiente para comprar as casas.");
            return;
        }

        jogador.Money -= custoTotal;
        espaco.Casas += casasPossiveis;
        Console.WriteLine($"Compradas {casasPossiveis} casa(s) em {espaco.Name}. Total de casas: {espaco.Casas}.");
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
        if (jogador == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

        if (aluguerPendente <= 0 || donoAluguerPendente == null)
        {
            Console.WriteLine("Nenhum aluguer pendente.");
            return;
        }

        bool pagou = jogador.Pay(aluguerPendente);
        if (!pagou)
        {
            Console.WriteLine($"{jogador.Name} não tem fundos suficientes e fica falido.");
            donoAluguerPendente.Receive(aluguerPendente);
        }
        else
        {
            donoAluguerPendente.Receive(aluguerPendente);
            Console.WriteLine("Aluguer pago.");
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

        var jogador = JogadorAtual;
        if (jogador == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

        AplicarEfeitoCarta(jogador);
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
        if (jogador == null)
        {
            Console.WriteLine("Nenhum jogador ativo.");
            return;
        }

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

    // Estado para a View
    public bool IsGameStarted() => jogoIniciado;
    public Player? GetCurrentPlayer() => JogadorAtual;
    public int GetPendingRent() => aluguerPendente;
    public Player? GetRentOwner() => donoAluguerPendente;
    public bool IsCardMandatory() => cartaObrigatoria;

    // ===== Helpers =====
    private void ReiniciarEstadoTurno()
    {
        if (players.Count == 0) return;

        var jogador = JogadorAtual;
        if (jogador == null) return;

        jogador.LancouDadosEsteTurno = false;
        aluguerPendente = 0;
        donoAluguerPendente = null;
        cartaObrigatoria = false;
    }

    private int CalcularAluguer(Space espaco)
    {
        int baseRent = Math.Max(10, (int)(espaco.Preco * 0.2));
        int bonusCasas = espaco.Casas * (baseRent / 2); // cada casa aumenta 50% do aluguer base
        return baseRent + bonusCasas;
    }

    private void AvancarParaProximoJogador()
    {
        int vivos = players.Count(p => !p.IsBankrupt);
        if (vivos <= 1)
        {
            var vencedor = players.FirstOrDefault(p => !p.IsBankrupt);
            string nome = vencedor != null ? vencedor.Name : "nenhum";
            Console.WriteLine($"Jogo terminou. Vencedor: {nome}");
            jogoIniciado = false;
            return;
        }

        int tentativas = players.Count;
        do
        {
            indiceJogadorAtual = (indiceJogadorAtual + 1) % players.Count;
            tentativas--;
        } while (players[indiceJogadorAtual].IsBankrupt && tentativas >= 0);

        ReiniciarEstadoTurno();
        if (JogadorAtual != null)
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
                bool pagou = jogador.Pay(100);
                if (!pagou)
                {
                    Console.WriteLine($"{jogador.Name} ficou falido ao pagar 100 à banca.");
                }
                else
                {
                    Console.WriteLine($"{jogador.Name} paga 100 à banca.");
                }
                break;
            default:
                jogador.Position = 24; // voltar ao Start
                Console.WriteLine($"{jogador.Name} volta ao Start.");
                break;
        }
    }
}
