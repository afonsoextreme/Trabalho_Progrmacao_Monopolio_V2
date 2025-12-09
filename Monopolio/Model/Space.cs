public class Space
{
    public string Name { get; set; }
    public int Preco { get; set; }
    public Player? Dono { get; set; }
    public bool PodeSerComprado { get; set; }
    public string Color { get; set; } = string.Empty; // grupo de cor a que o espaço pertence
    public int Casas { get; set; } = 0; // número de casas já construídas
    public int MaxCasas { get; set; } = 4; // limite simples de casas por espaço

    public Space(string name)
    {
        Name = name;
    }
}