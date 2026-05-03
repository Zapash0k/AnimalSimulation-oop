public sealed class HealthyState : IAnimalState
{
    public static readonly HealthyState Instance = new();
    private HealthyState() { }
    public bool CanRun => true;
    public bool CanFly => true;
    public bool CanWalk => true;
    public bool CanCrawl => true;
    public string Description => "Здорова";
}