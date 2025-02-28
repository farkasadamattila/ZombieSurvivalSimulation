using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        GameClass game = new GameClass();
        game.Start();
    }
}

class GameClass
{
    private Shelter shelter;
    private List<Survivor> survivors;
    private int difficulty;
    private bool running = true;
    private int selectedOption = 0;
    private readonly string[] menuOptions = { "Simulate Days", "View Inventory", "Manage Survivors", "Exit" };
    private Random rand = new Random();

    public GameClass()
    {
        difficulty = rand.Next(1, 5);
        shelter = new Shelter(rand.Next(10, 30), rand.Next(10, 30), rand.Next(10, 30));
        survivors = new List<Survivor>();
        for (int i = 0; i < rand.Next(3, 7); i++)
        {
            survivors.Add(new Survivor());
        }
    }

    private void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Main Menu ===");
        for (int i = 0; i < menuOptions.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {menuOptions[i]}");
        }
        Console.Write("Select an option: ");
    }

    public void HandleInput()
    {
        string input = Console.ReadLine();
        if (int.TryParse(input, out selectedOption) && selectedOption > 0 && selectedOption <= menuOptions.Length)
        {
            switch (selectedOption)
            {
                case 1:
                    SimulateDays();
                    break;
                case 2:
                    ViewInventory();
                    break;
                case 3:
                    ManageSurvivors();
                    break;
                case 4:
                    running = false;
                    Console.WriteLine("Exiting game...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
        }
    }

    private void SimulateDays()
    {
        Console.Clear();
        Console.WriteLine("Simulating days...");
        // Add logic to simulate days
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void ViewInventory()
    {
        Console.Clear();
        Console.WriteLine("=== Inventory ===");
        Console.WriteLine($"Food: {shelter.Food}");
        Console.WriteLine($"Water: {shelter.Water}");
        Console.WriteLine($"Ammo: {shelter.Ammo}");
        Console.WriteLine($"Medicine: {shelter.Medicine}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    public void Start()
    {
        Console.WriteLine("Welcome to Shelter 17 - A Zombie Survival Simulation\n");
        Console.WriteLine("The world has fallen into chaos... the undead roam the streets...");
        Console.Clear();
        while (running)
        {
            DisplayMenu();
            HandleInput();
        }
    }

    private void ManageSurvivors()
    {
        Console.Clear();
        Console.WriteLine("Managing survivors...");
        Console.WriteLine("1. Feed survivors (-5 Food)");
        Console.WriteLine("2. Give water (-5 Water)");
        Console.WriteLine("3. Provide medicine (-1 Medicine)");
        Console.WriteLine("4. Check survivor status");
        Console.WriteLine("5. Boost morale (-2 Food, -2 Water)");
        Console.Write("Choose an action: ");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                if (shelter.Food >= 5)
                {
                    shelter.ModifyResources(-5, 0, 0);
                    foreach (var survivor in survivors)
                        survivor.Eat();
                    Console.WriteLine("You fed the survivors.");
                }
                else
                    Console.WriteLine("Not enough food!");
                break;
            case "2":
                if (shelter.Water >= 5)
                {
                    shelter.ModifyResources(0, -5, 0);
                    foreach (var survivor in survivors)
                        survivor.Drink();
                    Console.WriteLine("You provided water to the survivors.");
                }
                else
                    Console.WriteLine("Not enough water!");
                break;
            case "3":
                if (shelter.Medicine > 0)
                {
                    shelter.UseMedicine();
                    foreach (var survivor in survivors)
                        survivor.Heal();
                    Console.WriteLine("You treated the sick survivors.");
                }
                else
                    Console.WriteLine("No medicine available!");
                break;
            case "4":
                Console.WriteLine("--- Survivor Status ---");
                foreach (var survivor in survivors)
                    Console.WriteLine(survivor.GetStatus());
                break;
            case "5":
                if (shelter.Food >= 2 && shelter.Water >= 2)
                {
                    shelter.ModifyResources(-2, -2, 0);
                    foreach (var survivor in survivors)
                        survivor.BoostMorale();
                    Console.WriteLine("You organized a morale-boosting activity. Survivors feel better!");
                }
                else
                    Console.WriteLine("Not enough resources to boost morale!");
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }
}

class Shelter
{
    public int Food { get; private set; }
    public int Water { get; private set; }
    public int Ammo { get; private set; }
    public int Medicine { get; private set; } = 5;

    public Shelter(int food, int water, int ammo)
    {
        Food = food;
        Water = water;
        Ammo = ammo;
    }

    public void ModifyResources(int foodChange, int waterChange, int ammoChange)
    {
        Food = Math.Max(0, Food + foodChange);
        Water = Math.Max(0, Water + waterChange);
        Ammo = Math.Max(0, Ammo + ammoChange);
    }

    public void UseMedicine()
    {
        Medicine = Math.Max(0, Medicine - 1);
    }
}

class Survivor
{
    private int health;
    private int hunger;
    private int thirst;
    private bool isSick;
    private int morale;

    private Random rand = new Random();

    public Survivor()
    {
        health = rand.Next(15, 21);
        hunger = rand.Next(5, 10);
        thirst = rand.Next(5, 10);
        isSick = rand.Next(0, 10) < 2;
        morale = rand.Next(3, 7);
    }

    public void Eat()
    {
        hunger = Math.Max(0, hunger - 3);
    }

    public void Drink()
    {
        thirst = Math.Max(0, thirst - 3);
    }

    public void Heal()
    {
        if (isSick)
        {
            isSick = false;
            health = Math.Min(20, health + 5);
        }
    }

    public void BoostMorale()
    {
        morale = Math.Min(10, morale + 2);
    }

    public string GetStatus()
    {
        return $"Health: {health}, Hunger: {hunger}, Thirst: {thirst}, Sick: {(isSick ? "Yes" : "No")}, Morale: {morale}/10";
    }
}
