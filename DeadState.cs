public sealed class DeadState : IAnimalState
{
    public static readonly DeadState Instance = new();
    private DeadState() { }
    public bool CanRun => false;
    public bool CanFly => false;
    public bool CanWalk => false;
    public bool CanCrawl => false;
    public string Description => "Мертва";
}