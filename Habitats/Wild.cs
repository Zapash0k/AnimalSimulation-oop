public class Wild : Habitat
{
    public override bool ProvidesCare => false;

    public Wild() : base("Воля") { }
}