public abstract class Habitat : IHabitat
{
    public string Name { get; }
    public abstract bool ProvidesCare { get; }

    protected Habitat(string name)
    {
        Name = name;
    }
}