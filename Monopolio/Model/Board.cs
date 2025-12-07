using System;
using System.Collections.Generic;

public class Board
{
    public List<Space> Spaces { get; private set; } = new List<Space>();

    private Dictionary<string, (int Preco, string Cor)> EspacosInfo = new Dictionary<string, (int Preco, string Cor)>()
    {
        {"Brown1", (100, "Brown")}, {"Brown2", (120, "Brown")},
        {"Teal1", (90, "Teal")}, {"Teal2", (130, "Teal")},
        {"Orange1", (120, "Orange")}, {"Orange2", (120, "Orange")}, {"Orange3", (140, "Orange")},
        {"Black1", (110, "Black")}, {"Black2", (120, "Black")}, {"Black3", (130, "Black")},
        {"Red1", (130, "Red")}, {"Red2", (130, "Red")}, {"Red3", (160, "Red")},
        {"Green1", (120, "Green")}, {"Green2", (140, "Green")}, {"Green3", (160, "Green")},
        {"Blue1", (140, "Blue")}, {"Blue2", (140, "Blue")}, {"Blue3", (170, "Blue")},
        {"Pink1", (160, "Pink")}, {"Pink2", (180, "Pink")},
        {"White1", (160, "White")}, {"White2", (180, "White")}, {"White3", (190, "White")},
        {"Yellow1", (140, "Yellow")}, {"Yellow2", (140, "Yellow")}, {"Yellow3", (170, "Yellow")},
        {"Violet1", (150, "Violet")}, {"Violet2", (130, "Violet")},
        {"Train1", (150, "Train")}, {"Train2", (150, "Train")}, {"Train3", (150, "Train")}, {"Train4", (150, "Train")},
        {"Electric Company", (120, "Utility")}, {"Water Works", (120, "Utility")}
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

            if (EspacosInfo.TryGetValue(name, out var info))
            {
                space.Preco = info.Preco;
                space.Color = info.Cor;
                space.PodeSerComprado = true;
            }
            else
            {
                space.PodeSerComprado = false;
                space.Color = string.Empty;
            }

            // Lux Tax nunca é comprável
            if (name == "Lux Tax")
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
