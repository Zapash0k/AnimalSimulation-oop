public class OwlFactory : IAnimalFactory
{
    public string AnimalType => "Сова";
    public Animal Create(string name) => new Owl(name);
}