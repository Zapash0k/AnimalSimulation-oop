public sealed class HungryState : IAnimalState
{
    public static readonly HungryState Instance = new();
    private HungryState() { }
    public bool CanRun => false;
    public bool CanFly => false;
    public bool CanWalk => true;
    public bool CanCrawl => true;
    public string Description => "Голодна та ослаблена";
}