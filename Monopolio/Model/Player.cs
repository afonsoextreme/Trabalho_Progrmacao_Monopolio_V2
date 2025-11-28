using System;

public class Player
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Money { get; private set; }
    public bool IsBankrupt { get; private set; }

    public Player(string name)
    {
        Name = name;
        Position = 0;
        Money = 1200; // Initial money
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