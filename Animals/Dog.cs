public class Dog : Animal, IRunnable, IWalkable
{
    public Dog(string name) : base(name) { }

    protected override void InitializeBodyParts()
    {
        BodyParts.Add(new Eyes(2));
        BodyParts.Add(new Legs(4));
    }

    public bool Run()
    {
        if (!IsAlive)
        {
            RaiseActionPerformed($"{Name} мертва і не може бігати");
            return false;
        }

        if (!CurrentState.CanRun)
        {
            RaiseActionPerformed($"{Name} занадто голодна щоб бігати");
            return false;
        }

        RaiseActionPerformed($"{Name} біжить");
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