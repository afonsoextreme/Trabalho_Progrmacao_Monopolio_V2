using System;

public class Player
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Dinheiro { get; private set; }
    public bool IsBankrupt { get; private set; }

    public Player(string nome)
    {
        Name = nome;
        Position = 0;
        Dinheiro = 1200; // Initial money
        IsBankrupt = false;
    }

    public void Move(int spaces)
    {
        Position = (Position + spaces) % 49; // Total de 49 espaços no tabuleiro
    }
    public bool AdjustBalance(int amount)
    {
        if (IsBankrupt)
        {
            return false;
        }

        Dinheiro += amount;

        if (Dinheiro < 0)
        {
            IsBankrupt = true;
            return false;
        }

        return true;
    }

    public bool Pay(int amount)
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive", nameof(amount));
        return AdjustBalance(-amount);
    }

    public bool Receive(int amount)
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive", nameof(amount));
        return AdjustBalance(amount);
    }

    public void DeclareBankrupt()
    {
        IsBankrupt = true;
    }
}