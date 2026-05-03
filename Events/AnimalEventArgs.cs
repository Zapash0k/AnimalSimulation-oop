public class AnimalEventArgs : EventArgs
{
    public Animal Animal { get; }
    public string Message { get; }

    public AnimalEventArgs(Animal animal, string message)
    {
        Animal = animal;
        Message = message;
    }
}