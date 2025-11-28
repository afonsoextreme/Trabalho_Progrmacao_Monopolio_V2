using System;
using System.Collections.Generic;

public static class ConsoleView
{
    public static void WriteLine(string message = "")
    {
        Console.WriteLine(message);
    }

    public static void Write(string message)
    {
        Console.Write(message);
    }

    public static string Prompt(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? string.Empty;
    }

    public static int PromptInt(string message, int defaultValue = 0)
    {
        Console.Write(message);
        var s = Console.ReadLine();
        if (int.TryParse(s, out int value))
            return value;
        return defaultValue;
    }

    public static void ShowPlayers(IReadOnlyList<Player> players)
    {
        Console.WriteLine("Jogadores registados:");
        foreach (var p in players)
        {
            Console.WriteLine($"- {p.Name} (Dinheiro: {p.Money})");
        }
    }

    public static void ShowBoard(Board board)
    {
        if (board == null) return;
        board.PrintBoard();
    }

    public static void ShowError(string message)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = prev;
    }

    public static void ShowInfo(string message)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ForegroundColor = prev;
    }
}