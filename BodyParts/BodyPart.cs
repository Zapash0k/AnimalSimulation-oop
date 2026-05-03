public abstract class BodyPart
{
    public string Name { get; protected set; }

    protected BodyPart(string name)
    {
        Name = name;
    }
}