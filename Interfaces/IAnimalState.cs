public interface IAnimalState
{
    bool CanRun { get; }
    bool CanFly { get; }
    bool CanWalk { get; }
    bool CanCrawl { get; }
    string Description { get; }
}