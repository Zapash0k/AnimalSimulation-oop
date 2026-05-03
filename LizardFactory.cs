public class LizardFactory : IAnimalFactory
{
    public string AnimalType => "Ящірка";
    public Animal Create(string name) => new Lizard(name);
}