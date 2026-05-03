public class Legs : BodyPart
{
    public int Count { get; }

    public Legs(int count) : base("Лапи")
    {
        Count = count;
    }
}