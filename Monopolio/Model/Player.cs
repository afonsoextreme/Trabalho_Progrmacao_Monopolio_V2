public class Player
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Money { get; set; }
    public bool IsBankrupt { get; private set; }
    public bool LancouDadosEsteTurno { get; set; }
    public bool EstaPreso { get; set; }
    public int TurnosNaPrisao { get; set; }
    public int NumJogos { get; set; }
    public int NumVitórias { get; set; }
    public int NumEmpates { get; set; }
    public int NumDerrotas { get; set; }

    public Player(string name)
    {
        Name = name;
        Position = 24; // Start está no centro do tabuleiro (índice 24)
        Money = 1200;
        IsBankrupt = false;
        LancouDadosEsteTurno = false;
        EstaPreso = false;
        TurnosNaPrisao = 0;
        NumJogos = 0;
        NumVitórias = 0;
        NumEmpates = 0;
        NumDerrotas = 0;
    }

    public void Move(int novaPosicao)
    {
        Position = novaPosicao;
    }

    public bool AdjustBalance(int amount)
    {
        if (IsBankrupt) return false;

        Money += amount;

        if (Money < 0)
        {
            IsBankrupt = true;
            return false;
        }

        return true;
    }

    public bool Pay(int amount)
    {
        return AdjustBalance(-amount);
    }

    public bool Receive(int amount)
    {
        return AdjustBalance(amount);
    }

    public void DeclareBankrupt()
    {
        IsBankrupt = true;
    }
}
