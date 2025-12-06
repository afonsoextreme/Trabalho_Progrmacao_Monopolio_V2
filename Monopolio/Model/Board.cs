using System;
using System.Collections.Generic;

public class Board
{
    public List<Space> Spaces { get; private set; } = new List<Space>();

    private Dictionary<string, int> PrecosEspacos = new Dictionary<string, int>()
    {
        {"Brown1", 100}, {"Brown2", 120},
        {"Teal1", 90}, {"Teal2", 130},
        {"Orange1", 120}, {"Orange2", 120}, {"Orange3", 140},
        {"Black1", 110}, {"Black2", 120}, {"Black3", 130},
        {"Red1", 130}, {"Red2", 130}, {"Red3", 160},
        {"Green1", 120}, {"Green2", 140}, {"Green3", 160},
        {"Blue1", 140}, {"Blue2", 140}, {"Blue3", 170},
        {"Pink1", 160}, {"Pink2", 180},
        {"White1", 160}, {"White2", 180}, {"White3", 190},
        {"Yellow1", 140}, {"Yellow2", 140}, {"Yellow3", 170},
        {"Violet1", 150}, {"Violet2", 130},
        {"Train1", 150}, {"Train2", 150}, {"Train3", 150}, {"Train4", 150},
        {"Electric Company", 120}, {"Water Works", 120},
        {"Lux Tax", 80}
    };

    public Board()
    {
        string[] names =
        {
            "Prison", "Green3", "Violet1", "Train2", "Red3", "White1", "BackToStart",
            "Blue3", "Community", "Red2", "Violet2", "Water Works", "Chance", "White2",
            "Blue2", "Red1", "Chance", "Brown2", "Community", "Black1", "Lux Tax",
            "Train1", "Green2", "Teal1", "Start", "Teal2", "Black2", "Train3",
            "Blue1", "Green1", "Community", "Brown1", "Chance", "Black3", "White3",
            "Pink1", "Chance", "Orange1", "Orange2", "Orange3", "Community", "Yellow3",
            "FreePark", "Pink2", "Electric Company", "Train4", "Yellow1", "Yellow2", "Police"
        };

        foreach (var name in names)
        {
            var space = new Space(name);

            if (PrecosEspacos.ContainsKey(name))
            {
                space.Preco = PrecosEspacos[name];
                space.PodeSerComprado = name != "Lux Tax";
            }
            else
            {
                space.PodeSerComprado = false;
            }

            Spaces.Add(space);
        }
    }

    public Space GetSpaceAt(int posicao)
    {
        return Spaces[posicao];
    }

    public void PrintBoard()
    {
        for (int i = 0; i < Spaces.Count; i++)
        {
            Console.Write(Spaces[i].Name);

            if (i < Spaces.Count - 1)
                Console.Write(" | ");

            if ((i + 1) % 7 == 0)
                Console.WriteLine();
        }
        Console.WriteLine();
    }
}
