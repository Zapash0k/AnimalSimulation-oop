class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        //створюємо об'єкти тварин
        var dog = new Dog("Бобік");
        var owl = new Owl("Совунья");
        var lizard = new Lizard("Гоша");
        //підписуємося на них
        SubscribeToAnimalEvents(dog);
        SubscribeToAnimalEvents(owl);
        SubscribeToAnimalEvents(lizard);
        //створюємо "середовища"
        var owner = new Owner("Іван");
        var petShop = new PetShop();
        var wild = new Wild();
        //призначаємо власників
        owner.AddAnimal(dog);
        owner.AddAnimal(owl);
        petShop.AddAnimal(lizard);
        //список об'єктів тварин
        var wildOwl = new Owl("Дикун");
        wildOwl.Habitat = wild;

        var animals = new List<Animal> { dog, owl, lizard, wildOwl};
        foreach (var a in animals) SubscribeToAnimalEvents(a);

        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Показати тварин");
            Console.WriteLine("2. Нагодувати тварину");
            Console.WriteLine("3. Прибрати за твариною");
            Console.WriteLine("4. Виконати дію (рух)");
            Console.WriteLine("5. Перевірити стан");
            Console.WriteLine("6. Завести нову тварину");
            Console.WriteLine("7. Перемістити тварину");
            Console.WriteLine("8. Кількість кінцівок");
            Console.WriteLine("9. Перемотати час вперед");
            Console.WriteLine("0. Вийти");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAnimals(animals);
                    break;

                case "2":
                    FeedAnimalMenu(animals, owner, petShop);
                    break;

                case "3":
                    CleanAnimalMenu(animals, owner, petShop);
                    break;

                case "4":
                    ActionMenu(animals);
                    break;

                case "5":
                    CheckStatus(animals);
                    break;

                case "6":
                    CreateAnimalMenu(animals, owner, wild);
                    break;

                case "7":
                    MoveMenu(animals, owner, petShop, wild);
                    break;
                case "8":
                    LimbsMenu(animals);
                    break;
                case "9":
                    AdvanceTimeMenu();
                    break;

                case "0":
                    return;
            }
        }
    }

    static Animal SelectAnimal(List<Animal> animals)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {animals[i].Name}");
        }

        int index = int.Parse(Console.ReadLine()) - 1;
        return animals[index];
    }

    static void FeedAnimalMenu(List<Animal> animals, Owner owner, PetShop shop)
    {
        var animal = SelectAnimal(animals);

        if (animal.Habitat is Owner)
            owner.FeedAnimal(animal);
        else if (animal.Habitat is PetShop)
            shop.FeedAnimal(animal);
        else
            animal.Eat();
    }

    static void CleanAnimalMenu(List<Animal> animals, Owner owner, PetShop shop)
    {
        var animal = SelectAnimal(animals);

        if (animal.Habitat is Owner)
            owner.CleanAnimal(animal);
        else if (animal.Habitat is PetShop)
            shop.CleanAnimal(animal);
    }

    static void ActionMenu(List<Animal> animals)
    {
        var animal = SelectAnimal(animals);

        Console.WriteLine("1. Ходити");
        Console.WriteLine("2. Бігати");
        Console.WriteLine("3. Літати");
        Console.WriteLine("4. Повзати");

        var action = Console.ReadLine();

        switch (action)
        {
            case "1":
                if (animal is IWalkable w) w.Walk();
                break;

            case "2":
                if (animal is IRunnable r) r.Run();
                break;

            case "3":
                if (animal is IFlyable f) f.Fly();
                break;

            case "4":
                if (animal is ICrawlable c) c.Crawl();
                break;
        }
    }

    static void CheckStatus(List<Animal> animals)
    {
        foreach (var animal in animals)
        {
            animal.CheckVitalStatus();

            Console.WriteLine($"{animal.Name}:");
            Console.WriteLine($"  Жива: {animal.IsAlive}");
            Console.WriteLine($"  Щаслива: {animal.IsHappy}");
        }
    }

    static void ShowAnimals(List<Animal> animals)
    {
        foreach (var a in animals)
        {
            Console.WriteLine($"{a.Name} ({a.Habitat?.Name})");
        }
    }

    static void SubscribeToAnimalEvents(Animal animal)
    {
        animal.Died += (s, e) => Console.WriteLine($"[ПОДІЯ] {e.Message}");
        animal.BecameHungry += (s, e) => Console.WriteLine($"[ПОДІЯ] {e.Message}");
        animal.Fed += (s, e) => Console.WriteLine($"[ПОДІЯ] {e.Message}");
        animal.Cleaned += (s, e) => Console.WriteLine($"[ПОДІЯ] {e.Message}");
        animal.HabitatChanged += (s, e) => Console.WriteLine($"[ПОДІЯ] {e.Message}");
        animal.ActionPerformed += (s, e) => Console.WriteLine($"[ДІЯ] {e.Message}");
    }

    static void CreateAnimalMenu(List<Animal> animals, Owner owner, Wild wild)
    {
        Console.WriteLine("Оберіть тип тварини:");
        Console.WriteLine("1. Собака");
        Console.WriteLine("2. Сова");
        Console.WriteLine("3. Ящірка");

        var type = Console.ReadLine();

        Console.Write("Введіть ім'я: ");
        var name = Console.ReadLine();

        Animal newAnimal = null;

        switch (type)
        {
            case "1":
                newAnimal = new Dog(name);
                break;

            case "2":
                newAnimal = new Owl(name);
                break;

            case "3":
                newAnimal = new Lizard(name);
                break;
        }

        if (newAnimal != null)
        {
            SubscribeToAnimalEvents(newAnimal);

            Console.WriteLine("Де буде жити тварина?");
            Console.WriteLine($"1. {owner.Name}");
            Console.WriteLine($"2. {wild.Name}");

            if (Console.ReadLine() == "2")
                newAnimal.Habitat = wild;
            else
                owner.AddAnimal(newAnimal);
            animals.Add(newAnimal);

            Console.WriteLine("Тварину створено!");
        }
    }

    static void MoveMenu(List<Animal> animals, Owner owner, PetShop petShop, Wild wild)
    {
        Console.WriteLine("Тварину для переміщення:");
        var a = SelectAnimal(animals);

        Console.WriteLine($"1. {owner.Name}");
        Console.WriteLine($"2. {petShop.Name}");
        Console.WriteLine($"3. {wild.Name}");

        IHabitat dest = Console.ReadLine() switch
        {
            "1" => owner,
            "2" => petShop,
            "3" => wild,
            _ => null
        };
        if (dest == null) { Console.WriteLine("Невірний вибір."); return; }

        if (a.Habitat is Owner co) co.RemoveAnimal(a);
        else if (a.Habitat is PetShop cp) cp.RemoveAnimal(a);

        if (dest is Owner no) no.AddAnimal(a);
        else if (dest is PetShop np) np.AddAnimal(a);
        else a.Habitat = wild;

        Console.WriteLine($"{a.Name} переміщено до «{dest.Name}».");
    }

    static void LimbsMenu(List<Animal> animals)
    {
        Console.WriteLine("Тварину:");
        var a = SelectAnimal(animals);

        foreach (var bp in a.GetBodyParts().Where(p => p is Legs or Wings))
        {
            int cnt = bp switch { Legs l => l.Count, Wings w => w.Count, _ => 0 };
            Console.WriteLine($"  {bp.Name}: {cnt}");
        }
        Console.WriteLine($"  Разом кінцівок: {a.GetLimbCount()}");
    }

    static void AdvanceTimeMenu()
    {
        Console.Write("На скільки годин перемотати? ");
        if (double.TryParse(Console.ReadLine(), out double h))
        {
            SimulationClock.AdvanceTime(TimeSpan.FromHours(h));
            Console.WriteLine($"Час: {SimulationClock.Now:HH:mm dd.MM.yyyy}");
        }
    }
}

