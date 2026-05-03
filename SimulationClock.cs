public static class SimulationClock
{
    private static TimeSpan _advance = TimeSpan.Zero;

    public static DateTime Now => DateTime.Now + _advance;
    public static DateTime Today => Now.Date;

    public static void AdvanceTime(TimeSpan duration) => _advance += duration;
    public static void Reset() => _advance = TimeSpan.Zero;
}