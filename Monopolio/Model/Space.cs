public class Space
{
    public string Name { get; set; }
    public int Preco { get; set; }
    public Player Dono { get; set; }
    public bool PodeSerComprado { get; set; }

    public Space(string name)
    {
        Name = name;
    }
}