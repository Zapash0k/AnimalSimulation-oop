public interface IAnimalFactory
{
    string AnimalType { get; }
    Animal Create(string name);
}