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
    public IAnimalState CurrentState { get; private set; } = HealthyState.Instance;

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
    public event EventHandler<AnimalEventArgs> StateChanged;

    protected Animal(string name)
    {
        Name = name;
        LastFeedingTime = SimulationClock.Now;
        LastCleaningTime = SimulationClock.Now;
        _lastDayCheck = SimulationClock.Today;
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

            return _cleanedToday && _lastCleanDay == SimulationClock.Today;
        }
    }

    private void UpdateState()
    {
        IAnimalState next = !IsAlive
            ? DeadState.Instance
            : (SimulationClock.Now - LastFeedingTime).TotalHours > 8
                ? HungryState.Instance
                : HealthyState.Instance;

        if (next == CurrentState) return;
        CurrentState = next;
        OnStateChanged(new AnimalEventArgs(this, $"{Name}: стан → «{next.Description}»"));
    }

    public void CheckVitalStatus()
    {
        if (!IsAlive) return;

        // Перевірка нового дня
        if (SimulationClock.Today > _lastDayCheck)
        {
            if (FeedingsToday < MinFeedingsPerDay)
            {
                Die("не їла достатньо вчора");
                return;
            }

            FeedingsToday = 0;
            _lastDayCheck = SimulationClock.Today;

            // скидаємо прибирання
            _cleanedToday = false;
        }

        // Перевірка максимального часу без їжі
        var hoursSinceFeeding = (SimulationClock.Now - LastFeedingTime).TotalHours;
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

        UpdateState();
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
        LastFeedingTime = SimulationClock.Now;
        OnFed(new AnimalEventArgs(this, $"{Name} поїла ({FeedingsToday}/{MaxFeedingsPerDay} за сьогодні)"));
        UpdateState();
        return true;
    }

    public void Clean()
    {
        if (!IsAlive)
        {
            OnActionPerformed(new AnimalEventArgs(this, $"{Name} мертва"));
            return;
        }

        LastCleaningTime = SimulationClock.Now;

        _cleanedToday = true;
        _lastCleanDay = SimulationClock.Today;

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
    protected virtual void OnStateChanged(AnimalEventArgs e) => StateChanged?.Invoke(this, e);

    public IReadOnlyList<BodyPart> GetBodyParts() => BodyParts.AsReadOnly();
    public int GetLimbCount() =>
    BodyParts.OfType<Legs>().Sum(l => l.Count) +
    BodyParts.OfType<Wings>().Sum(w => w.Count);

    protected void RaiseActionPerformed(string message) =>
        OnActionPerformed(new AnimalEventArgs(this, message));
}