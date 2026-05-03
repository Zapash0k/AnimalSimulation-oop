public class AnimalFactoryRegistry
{
    private readonly Dictionary<string, IAnimalFactory> _factories = new();

    public AnimalFactoryRegistry()
    {
        Register("1", new DogFactory());
        Register("2", new OwlFactory());
        Register("3", new LizardFactory());
    }

    public void Register(string key, IAnimalFactory factory) => _factories[key] = factory;

    public IAnimalFactory? Get(string key) =>
        _factories.TryGetValue(key, out var f) ? f : null;

    public IEnumerable<(string Key, IAnimalFactory Factory)> GetAll() =>
        _factories.Select(kvp => (kvp.Key, kvp.Value));
}