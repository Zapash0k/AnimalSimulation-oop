public class Owl : Animal, IFlyable, IWalkable
{
    public Owl(string name) : base(name) { }

    protected override void InitializeBodyParts()
    {
        BodyParts.Add(new Eyes(2));
        BodyParts.Add(new Legs(2));
        BodyParts.Add(new Wings(2));
    }

    public bool Fly()
    {
        if (!IsAlive)
        {
            RaiseActionPerformed($"{Name} мертва і не може літати");
            return false;
        }

        if (!CurrentState.CanFly)
        {
            RaiseActionPerformed($"{Name} занадто голодна щоб літати");
            return false;
        }

        RaiseActionPerformed($"{Name} летить");
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