public class Owner : Habitat
{
    public override bool ProvidesCare => true;

    public string OwnerName { get; }
    private readonly List<Animal> _animals = new();

    public Owner(string ownerName) : base($"Хазяїн {ownerName}")
    {
        OwnerName = ownerName;
    }

    public void AddAnimal(Animal animal)
    {
        if (!_animals.Contains(animal))
        {
            _animals.Add(animal);
            animal.Habitat = this;
        }
    }

    public void RemoveAnimal(Animal animal)
    {
        if (_animals.Remove(animal))
        {
            animal.Habitat = null;
        }
    }

    public void FeedAnimal(Animal animal)
    {
        if (_animals.Contains(animal))
        {
            animal.Eat();
        }
    }

    public void CleanAnimal(Animal animal)
    {
        if (_animals.Contains(animal))
        {
            animal.Clean();
        }
    }

    public IReadOnlyList<Animal> GetAnimals() => _animals.AsReadOnly();
}