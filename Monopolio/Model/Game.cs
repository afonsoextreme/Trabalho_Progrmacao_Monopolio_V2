using System;
using System.Collections.Generic;
using System.Linq;

public class Game
{
    private readonly Random _rng = new Random();

    public bool JogoemCurso { get; set; } = false;
    public List<Player> Jogadores { get; } = new List<Player>();
    public Player Jogadorajogar { get; set; }
    public Board Tabuleiro { get; } = new Board();

    // estados por jogador (mantemos em Game para não alterar a classe Player)
    private readonly Dictionary<string, int> _lancamentoDuplicados = new Dictionary<string, int>();
    private readonly Dictionary<string, bool> _podeLancarNovamente = new Dictionary<string, bool>();

    // dado para gerar o movimento sem o 0
    private int GerarValorDado()
    {
        int[] valores = { -3, -2, -1, 1, 2, 3 };
        return valores[_rng.Next(valores.Length)];
    }

    public string ExecutarLD(string nomeJogador)
    {
        if (!JogoemCurso)
            return "Não existe um jogo em curso.";

        Player j = Jogadores.FirstOrDefault(x => x.Name == nomeJogador);
        if (j == null)
            return "Jogador não encontrado.";

        if (Jogadorajogar != null && j != Jogadorajogar)
            return "Não é a vez do jogador.";

        // lançar dados e obter valores
        int dado1 = GerarValorDado();
        int dado2 = GerarValorDado();

        // regra do duplo lançamento
        bool dadoIguais = (dado1 == dado2);

        int novoPos = CalcularNovaPosicao(j.Position, dado1, dado2);

        Space esp = Tabuleiro.Spaces.ElementAtOrDefault(novoPos);

        j.Position = novoPos;

        string resposta = ProcessarEspaco(j, esp, dado1, dado2);

        if (!_lancamentoDuplicados.ContainsKey(j.Name))
            _lancamentoDuplicados[j.Name] = 0;
        if (!_podeLancarNovamente.ContainsKey(j.Name))
            _podeLancarNovamente[j.Name] = false;

        if (dadoIguais)
        {
            _lancamentoDuplicados[j.Name]++;

            if (_lancamentoDuplicados[j.Name] == 2)
            {
                EnviarJogadorParaPrisao(j);
                _lancamentoDuplicados[j.Name] = 0;
                return $"{resposta}\nJogador preso por lançar valores iguais duas vezes.";
            }
            else
            {
                _podeLancarNovamente[j.Name] = true;
            }
        }
        else
        {
            _lancamentoDuplicados[j.Name] = 0;
            _podeLancarNovamente[j.Name] = false;
        }

        return resposta;
    }

    private int CalcularNovaPosicao(int posAtual, int dado1, int dado2)
    {
        int total = dado1 + dado2;
        int count = Tabuleiro.Spaces.Count;
        int novo = (posAtual + total) % count;
        if (novo < 0) novo += count;
        return novo;
    }

    private string ProcessarEspaco(Player j, Space esp, int dado1, int dado2)
    {
        if (esp == null) return "Espaço inválido.";
        // implementação mínima: apenas retorna uma mensagem descritiva
        if (esp.Name.Contains("Prison") || esp.Name.Contains("Police"))
        {
            return $"{j.Name} foi para {esp.Name}.";
        }

        return $"{j.Name} moveu-se para {esp.Name}.";
    }

    private void EnviarJogadorParaPrisao(Player j)
    {
        int idx = Tabuleiro.Spaces.FindIndex(s => s.Name.Contains("Prison") || s.Name.Contains("Police"));
        if (idx < 0) idx = 0;
        j.Position = idx;
    }
}

