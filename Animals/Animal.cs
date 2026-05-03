public abstract class Animal : IEdible, ICleanable
{
    private const int MinFeedingsPerDay = 1;
    private const int MaxFeedingsPerDay = 5;
    private const int HoursWithoutFoodForWeakness = 8;
    private const int MaxHoursBetweenFeedings = 24;

    public string Name { get; }
    public bool IsAlive { get; private set; } = true;
    public DateTime LastFeedingTime { get; private set; }
    public DateTime LastCleaningTime { get; private set; }
    public int FeedingsToday { get; private set; }

    private DateTime _lastDayCheck;
    private IHabitat _habitat;
    private bool _cleanedToday = false;
    private DateTime _lastCleanDay;

    public IHabitat Habitat
    {
        get => _habitat;
        set
        {
            _habitat = value;
            OnHabitatChanged(new AnimalEventArgs(this, $"{Name} тепер живе: {value?.Name ?? "ніде"}"));
        }
    }

    protected List<BodyPart> BodyParts { get; } = new();

    // Events
    public event EventHandler<AnimalEventArgs> Died;
    public event EventHandler<AnimalEventArgs> BecameHungry;
    public event EventHandler<AnimalEventArgs> Fed;
    public event EventHandler<AnimalEventArgs> Cleaned;
    public event EventHandler<AnimalEventArgs> HabitatChanged;
    public event EventHandler<AnimalEventArgs> ActionPerformed;

    protected Animal(string name)
    {
        Name = name;
        LastFeedingTime = DateTime.Now;
        LastCleaningTime = DateTime.Now;
        _lastDayCheck = DateTime.Today;
        FeedingsToday = 0;
        InitializeBodyParts();
    }

    protected abstract void InitializeBodyParts();

    public bool IsHappy
    {
        get
        {
            if (!IsAlive) return false;

            if (Habitat is Wild) return true;

            return _cleanedToday && _lastCleanDay == DateTime.Today;
        }
    }

    protected bool CanPerformIntenseActivity
    {
        get
        {
            if (!IsAlive) return false;
            var hoursSinceFeeding = (DateTime.Now - LastFeedingTime).TotalHours;
            return hoursSinceFeeding <= HoursWithoutFoodForWeakness;
        }
    }

    public void CheckVitalStatus()
    {
        if (!IsAlive) return;

        // Перевірка нового дня
        if (DateTime.Today > _lastDayCheck)
        {
            if (FeedingsToday < MinFeedingsPerDay)
            {
                Die("не їла достатньо вчора");
                return;
            }

            FeedingsToday = 0;
            _lastDayCheck = DateTime.Today;

            // скидаємо прибирання
            _cleanedToday = false;
        }

        // Перевірка максимального часу без їжі
        var hoursSinceFeeding = (DateTime.Now - LastFeedingTime).TotalHours;
        if (hoursSinceFeeding > MaxHoursBetweenFeedings)
        {
            Die("занадто довго не їла");
            return;
        }

        // Сповіщення про голод
        if (hoursSinceFeeding > HoursWithoutFoodForWeakness)
        {
            OnBecameHungry(new AnimalEventArgs(this, $"{Name} голодна і ослаблена"));
        }
    }

    public virtual bool Eat()
    {
        if (!IsAlive)
        {
            OnActionPerformed(new AnimalEventArgs(this, $"{Name} мертва і не може їсти"));
            return false;
        }

        if (FeedingsToday >= MaxFeedingsPerDay)
        {
            OnActionPerformed(new AnimalEventArgs(this, $"{Name} вже їла максимальну кількість разів сьогодні"));
            return false;
        }

        FeedingsToday++;
        LastFeedingTime = DateTime.Now;
        OnFed(new AnimalEventArgs(this, $"{Name} поїла ({FeedingsToday}/{MaxFeedingsPerDay} за сьогодні)"));
        return true;
    }

    public void Clean()
    {
        if (!IsAlive)
        {
            OnActionPerformed(new AnimalEventArgs(this, $"{Name} мертва"));
            return;
        }

        LastCleaningTime = DateTime.Now;

        _cleanedToday = true;
        _lastCleanDay = DateTime.Today;

        OnCleaned(new AnimalEventArgs(this, $"За {Name} прибрано"));
    }

    private void Die(string reason)
    {
        IsAlive = false;
        OnDied(new AnimalEventArgs(this, $"{Name} померла: {reason}"));
    }

    protected virtual void OnDied(AnimalEventArgs e) => Died?.Invoke(this, e);
    protected virtual void OnBecameHungry(AnimalEventArgs e) => BecameHungry?.Invoke(this, e);
    protected virtual void OnFed(AnimalEventArgs e) => Fed?.Invoke(this, e);
    protected virtual void OnCleaned(AnimalEventArgs e) => Cleaned?.Invoke(this, e);
    protected virtual void OnHabitatChanged(AnimalEventArgs e) => HabitatChanged?.Invoke(this, e);
    protected virtual void OnActionPerformed(AnimalEventArgs e) => ActionPerformed?.Invoke(this, e);

    public IReadOnlyList<BodyPart> GetBodyParts() => BodyParts.AsReadOnly();

    protected void RaiseActionPerformed(string message) =>
        OnActionPerformed(new AnimalEventArgs(this, message));
}