public class DogFactory : IAnimalFactory
{
    public string AnimalType => "Собака";
    public Animal Create(string name) => new Dog(name);
}