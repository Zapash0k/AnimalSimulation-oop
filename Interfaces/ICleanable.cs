public interface ICleanable
{
    DateTime LastCleaningTime { get; }
    void Clean();
}