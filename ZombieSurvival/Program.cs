using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        IGame game = new GameClass(new ConsoleInputHandler(), new ConsoleDisplayHandler());
        await game.StartAsync();
    }
}

public interface IGame
{
    Task StartAsync();
}

public interface IInputHandler
{
    Task<string> GetInputAsync();
}

public interface IDisplayHandler
{
    void DisplayMenu(string[] options);
    void DisplayMessage(string message);
    void DisplayInlineMessage(string message);
    void Clear();
}

public class ConsoleInputHandler : IInputHandler
{
    public async Task<string> GetInputAsync()
    {
        return await Task.Run(() => Console.ReadLine());
    }
}

public class ConsoleDisplayHandler : IDisplayHandler
{
    public void DisplayMenu(string[] options)
    {
        Console.Clear();
        Console.WriteLine("=== Main Menu ===");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.Write("Select an option: ");
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void DisplayInlineMessage(string message)
    {
        Console.Write(message);
    }

    public void Clear()
    {
        Console.Clear();
    }
}

public class GameClass : IGame
{
    private readonly Shelter shelter;
    private readonly List<Survivor> survivors;
    private readonly IInputHandler inputHandler;
    private readonly IDisplayHandler displayHandler;
    private readonly string[] menuOptions = { "Simulate Days", "View Inventory", "Manage Survivors", "Exit" };
    private readonly Random rand = new Random();
    private bool running = true;

    public GameClass(IInputHandler inputHandler, IDisplayHandler displayHandler)
    {
        this.inputHandler = inputHandler;
        this.displayHandler = displayHandler;
        shelter = new Shelter(rand.Next(10, 30), rand.Next(10, 30), rand.Next(10, 30));
        survivors = new List<Survivor>();
        for (int i = 0; i < rand.Next(3, 7); i++)
        {
            survivors.Add(new Survivor());
        }
    }

    public async Task StartAsync()
    {
        displayHandler.DisplayMessage("Welcome to Shelter 17 - A Zombie Survival Simulation\n");
        displayHandler.DisplayMessage("The world has fallen into chaos... the undead roam the streets...");
        displayHandler.Clear();
        while (running)
        {
            displayHandler.DisplayMenu(menuOptions);
            await HandleInputAsync();
        }
    }

    private async Task HandleInputAsync()
    {
        string input = await inputHandler.GetInputAsync();
        if (int.TryParse(input, out int selectedOption) && selectedOption > 0 && selectedOption <= menuOptions.Length)
        {
            switch (selectedOption)
            {
                case 1:
                    await SimulateDaysAsync();
                    break;
                case 2:
                    ViewInventory();
                    break;
                case 3:
                    await ManageSurvivorsAsync();
                    break;
                case 4:
                    running = false;
                    displayHandler.DisplayMessage("Exiting game...");
                    break;
                default:
                    displayHandler.DisplayMessage("Invalid option. Please try again.");
                    break;
            }
        }
        else
        {
            displayHandler.DisplayMessage("Invalid input. Please enter a number between 1 and 4.");
        }
    }

    private async Task SimulateDaysAsync()
    {
        displayHandler.Clear();
        displayHandler.DisplayInlineMessage("Enter the number of days to simulate (1-5): ");
        string input = await inputHandler.GetInputAsync();
        if (int.TryParse(input, out int days) && days > 0 && days <= 5)
        {
            displayHandler.DisplayMessage($"Simulating {days} days...");
            for (int i = 0; i < days; i++)
            {
                displayHandler.DisplayInlineMessage("-");
                await Task.Delay(500); // Simulate some delay
                SimulateDay();
            }
            displayHandler.DisplayMessage("\nSimulation complete. Press any key to return...");
        }
        else
        {
            displayHandler.DisplayMessage("Invalid input. Please enter a number between 1 and 5.");
        }
        Console.ReadKey();
    }

    private void SimulateDay()
    {
        // Adjust survivor status
        foreach (var survivor in survivors)
        {
            survivor.AdjustStatus();
        }

        // Adjust inventory
        shelter.ModifyResources(-rand.Next(1, 3), -rand.Next(1, 3), 0);

        // Trigger random event
        TriggerRandomEvent();
    }

    private void TriggerRandomEvent()
    {
        int eventChance = rand.Next(1, 101);
        if (eventChance <= 20) // 20% chance for an event
        {
            int eventType = rand.Next(1, 21);
            switch (eventType)
            {
                case 1:
                    displayHandler.DisplayMessage("Scavenger’s Luck – A survivor finds a small stash of food and water.");
                    shelter.ModifyResources(5, 5, 0);
                    break;
                case 2:
                    displayHandler.DisplayMessage("Military Broadcast – A weak radio signal suggests a possible safe zone nearby.");
                    break;
                case 3:
                    displayHandler.DisplayMessage("Lone Survivor – A new survivor arrives, bringing some supplies.");
                    survivors.Add(new Survivor());
                    shelter.ModifyResources(3, 3, 0);
                    break;
                case 4:
                    displayHandler.DisplayMessage("Secret Stash – While cleaning the shelter, a survivor finds hidden food and ammo.");
                    shelter.ModifyResources(5, 0, 5);
                    break;
                case 5:
                    displayHandler.DisplayMessage("Fortification Upgrade – Survivors reinforce the shelter, increasing its defense.");
                    break;
                case 6:
                    displayHandler.DisplayMessage("Altruistic Trader – A passing stranger trades valuable supplies at a fair deal.");
                    shelter.ModifyResources(5, 5, 5);
                    break;
                case 7:
                    displayHandler.DisplayMessage("Rainwater Collection – A storm fills available containers with fresh water.");
                    shelter.ModifyResources(0, 10, 0);
                    break;
                case 8:
                    displayHandler.DisplayMessage("Improvised Medicine – A survivor crafts homemade medicine, curing someone’s sickness.");
                    foreach (var survivor in survivors)
                    {
                        if (survivor.IsSick)
                        {
                            survivor.Heal();
                            break;
                        }
                    }
                    break;
                case 9:
                    displayHandler.DisplayMessage("Pet Companion – A stray dog joins the shelter, providing protection and morale.");
                    break;
                case 10:
                    displayHandler.DisplayMessage("Unlooted Pharmacy – A lucky scavenger trip recovers medical supplies.");
                    shelter.ModifyResources(0, 0, 5);
                    break;
                case 11:
                    displayHandler.DisplayMessage("Raider Ambush – Hostile survivors attack, stealing resources.");
                    shelter.ModifyResources(-5, -5, -5);
                    break;
                case 12:
                    displayHandler.DisplayMessage("Food Spoilage – Some food goes bad, reducing supplies.");
                    shelter.ModifyResources(-5, 0, 0);
                    break;
                case 13:
                    displayHandler.DisplayMessage("Water Contamination – Drinking water becomes unsafe, causing sickness.");
                    shelter.ModifyResources(0, -5, 0);
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.MakeSick();
                            break;
                        }
                    }
                    break;
                case 14:
                    displayHandler.DisplayMessage("Zombie Infestation – A zombie finds its way into the shelter, injuring someone.");
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.MakeSick();
                            break;
                        }
                    }
                    break;
                case 15:
                    displayHandler.DisplayMessage("Shelter Collapse – A structural issue damages the shelter, reducing safety.");
                    break;
                case 16:
                    displayHandler.DisplayMessage("Mental Breakdown – A survivor suffers from stress, refusing to cooperate.");
                    break;
                case 17:
                    displayHandler.DisplayMessage("Supplies Stolen – A sneaky thief steals items during the night.");
                    shelter.ModifyResources(-5, -5, -5);
                    break;
                case 18:
                    displayHandler.DisplayMessage("Illness Outbreak – A flu spreads among survivors, requiring medicine.");
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.MakeSick();
                        }
                    }
                    break;
                case 19:
                    displayHandler.DisplayMessage("Equipment Failure – A tool breaks, making scavenging less effective.");
                    break;
                case 20:
                    displayHandler.DisplayMessage("Horde Sighting – Large groups of zombies are spotted, increasing future danger.");
                    break;
                case 21:
                    displayHandler.DisplayMessage("Nuke – A nuke is thrown down, instantly ending the game.");
                    running = false;
                    break;
                case 22:
                    displayHandler.DisplayMessage("Mutant – A mutant is at the front door, requiring lots of ammunition to kill.");
                    shelter.ModifyResources(0, 0, -10);
                    break;
                default:
                    break;
            }
        }
    }

    private void ViewInventory()
    {
        displayHandler.Clear();
        displayHandler.DisplayMessage("=== Inventory ===");
        displayHandler.DisplayMessage($"Food: {shelter.Food}");
        displayHandler.DisplayMessage($"Water: {shelter.Water}");
        displayHandler.DisplayMessage($"Ammo: {shelter.Ammo}");
        displayHandler.DisplayMessage($"Medicine: {shelter.Medicine}");
        displayHandler.DisplayMessage("Press any key to return...");
        Console.ReadKey();
    }

    private async Task ManageSurvivorsAsync()
    {
        bool managing = true;
        while (managing)
        {
            displayHandler.Clear();
            displayHandler.DisplayMessage("Managing survivors...");
            displayHandler.DisplayMessage("1. Feed survivors (-5 Food)");
            displayHandler.DisplayMessage("2. Give water (-5 Water)");
            displayHandler.DisplayMessage("3. Provide medicine (-1 Medicine)");
            displayHandler.DisplayMessage("4. Boost morale (-2 Food, -2 Water)");
            displayHandler.DisplayMessage("5. Check survivor status");
            displayHandler.DisplayMessage("6. Stop managing survivors");
            displayHandler.DisplayMessage("Choose an action: ");
            string choice = await inputHandler.GetInputAsync();
            switch (choice)
            {
                case "1":
                    await ProvideResourceAsync("food");
                    break;
                case "2":
                    await ProvideResourceAsync("water");
                    break;
                case "3":
                    await ProvideMedicineAsync();
                    break;
                case "4":
                    if (shelter.Food >= 2 && shelter.Water >= 2)
                    {
                        shelter.ModifyResources(-2, -2, 0);
                        foreach (var survivor in survivors)
                            survivor.BoostMorale();
                        displayHandler.DisplayMessage("You organized a morale-boosting activity. Survivors feel better!");
                    }
                    else
                        displayHandler.DisplayMessage("Not enough resources to boost morale!");
                    break;
                case "5":
                    displayHandler.DisplayMessage("--- Survivor Status ---");
                    foreach (var survivor in survivors)
                        displayHandler.DisplayMessage(survivor.GetStatus());
                    break;
                case "6":
                    managing = false;
                    break;
                default:
                    displayHandler.DisplayMessage("Invalid choice.");
                    break;
            }
            displayHandler.DisplayMessage("Press any key to return...");
            Console.ReadKey();
        }
    }

    private async Task ProvideResourceAsync(string resourceType)
    {
        int requiredAmount = resourceType == "food" ? 5 : 5;
        if ((resourceType == "food" && shelter.Food >= requiredAmount) || (resourceType == "water" && shelter.Water >= requiredAmount))
        {
            displayHandler.DisplayMessage($"Select a survivor to give {resourceType} to:");
            for (int i = 0; i < survivors.Count; i++)
            {
                displayHandler.DisplayMessage($"{i + 1}. {survivors[i].Name}");
            }
            string input = await inputHandler.GetInputAsync();
            if (int.TryParse(input, out int selectedSurvivor) && selectedSurvivor > 0 && selectedSurvivor <= survivors.Count)
            {
                if (resourceType == "food")
                {
                    shelter.ModifyResources(-5, 0, 0);
                    survivors[selectedSurvivor - 1].Eat();
                }
                else if (resourceType == "water")
                {
                    shelter.ModifyResources(0, -5, 0);
                    survivors[selectedSurvivor - 1].Drink();
                }
                displayHandler.DisplayMessage($"You gave {resourceType} to {survivors[selectedSurvivor - 1].Name}.");
            }
            else
            {
                displayHandler.DisplayMessage("Invalid selection.");
            }
        }
        else
        {
            displayHandler.DisplayMessage($"Not enough {resourceType}!");
        }
    }

    private async Task ProvideMedicineAsync()
    {
        if (shelter.Medicine > 0)
        {
            List<Survivor> sickSurvivors = survivors.FindAll(s => s.IsSick);
            if (sickSurvivors.Count > 0)
            {
                displayHandler.DisplayMessage("Select a sick survivor to treat:");
                for (int i = 0; i < sickSurvivors.Count; i++)
                {
                    displayHandler.DisplayMessage($"{i + 1}. {sickSurvivors[i].Name}");
                }
                string input = await inputHandler.GetInputAsync();
                if (int.TryParse(input, out int selectedSurvivor) && selectedSurvivor > 0 && selectedSurvivor <= sickSurvivors.Count)
                {
                    shelter.UseMedicine();
                    sickSurvivors[selectedSurvivor - 1].Heal();
                    displayHandler.DisplayMessage($"You treated {sickSurvivors[selectedSurvivor - 1].Name}.");
                }
                else
                {
                    displayHandler.DisplayMessage("Invalid selection.");
                }
            }
            else
            {
                displayHandler.DisplayMessage("Let's celebrate, no one is sick!");
            }
        }
        else
        {
            displayHandler.DisplayMessage("No medicine available!");
        }
    }
}

public class Shelter
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

public class Survivor
{
    private static readonly string[] Names = { "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Riley", "Quinn", "Avery", "Parker", "Reese" };
    private static readonly Random rand = new Random();

    private int health;
    private int hunger;
    private int thirst;
    private bool isSick;
    private int morale;
    public string Name { get; private set; }
    public bool IsSick => isSick;

    public Survivor()
    {
        Name = Names[rand.Next(Names.Length)];
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

    public void MakeSick()
    {
        isSick = true;
    }

    public void AdjustStatus()
    {
        hunger = Math.Max(0, hunger - 2);
        thirst = Math.Max(0, thirst - 2);
        if (hunger == 0 || thirst == 0)
        {
            health = Math.Max(0, health - 5);
        }
    }

    public void BoostMorale()
    {
        morale = Math.Min(10, morale + 2);
    }

    public string GetStatus()
    {
        return $"{Name} - Health: {health}, Hunger: {hunger}, Thirst: {thirst}, Sick: {(isSick ? "Yes" : "No")}, Morale: {morale}/10";
    }
}
