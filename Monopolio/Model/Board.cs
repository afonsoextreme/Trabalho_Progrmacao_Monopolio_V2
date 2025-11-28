using System;
using System.Collections.Generic;
using System.Xml;

public class Board
{
    public List<Space> Spaces { get; private set; } = new List<Space>();

    public Board()
    {
        string[]names = {
            "Prison", "Green3", "Violet1", "Train2", "Red3", "White1", "BackToStart",
            "Blue3", "Community", "Red2", "Violet2", "Water Works", "Chance", "White2",
            "Blue2", "Red1", "Chance", "Brown2", "Community", "Black1", "Lux Tax",
            "Train1", "Green2", "Teal1 (Nome1) Nome3", "Start Nome1 Nome2", "Teal2", "Black2", "Train3",
            "Blue1", "Green1", "Community", "Brown1", "Chance", "Black3 (Nome2 - 2)", "White3",
            "Pink1", "Chance", "Orange1", "Orange2", "Orange3", "Community", "Yellow3",
            "FreePark", "Pink2", "Electric Company", "Train4", "Yellow1", "Yellow2", "Police"
        };
        foreach (var name in names)
        {
            Spaces.Add(new Space(name));
        }
    }
    
    public void PrintBoard()
    {
        for (int i = 0; i < Spaces.Count; i++)
        {
            Console.Write(Spaces[i].Name);
            if (i < Spaces.Count - 1)
            {
                Console.Write(" | ");
            }
            if ((i + 1) % 7 == 0)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
    }   
}
