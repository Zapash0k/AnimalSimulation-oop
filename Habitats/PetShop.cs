public class PetShop : Habitat
{
    public override bool ProvidesCare => true;

    private readonly List<Animal> _animals = new();

    public PetShop(string name = "Зоомагазин") : base(name) { }

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

    public void CleanAnimal(Animal animal)
    {
        if (_animals.Contains(animal))
        {
            animal.Clean();
        }
    }

    public void FeedAnimal(Animal animal)
    {
        if (_animals.Contains(animal))
        {
            animal.Eat();
        }
    }

    public IReadOnlyList<Animal> GetAnimals() => _animals.AsReadOnly();
}