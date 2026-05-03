public class Wings : BodyPart
{
    public int Count { get; }

    public Wings(int count = 2) : base("Крила")
    {
        Count = count;
    }
}