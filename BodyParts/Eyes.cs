public class Eyes : BodyPart
{
    public int Count { get; }

    public Eyes(int count = 2) : base("Очі")
    {
        Count = count;
    }
}