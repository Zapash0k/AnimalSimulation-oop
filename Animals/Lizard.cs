public class Lizard : Animal, ICrawlable, IWalkable
{
    public Lizard(string name) : base(name) { }

    protected override void InitializeBodyParts()
    {
        BodyParts.Add(new Eyes(2));
        BodyParts.Add(new Legs(4));
    }

    public bool Crawl()
    {
        if (!IsAlive)
        {
            RaiseActionPerformed($"{Name} мертва і не може повзати");
            return false;
        }

        RaiseActionPerformed($"{Name} повзає");
        return true;
    }

    public bool Walk()
    {
        if (!IsAlive)
        {
            RaiseActionPerformed($"{Name} мертва і не може ходити");
            return false;
        }

        RaiseActionPerformed($"{Name} ходить");
        return true;
    }
}