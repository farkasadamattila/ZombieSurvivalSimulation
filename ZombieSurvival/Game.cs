public class Game
{
    private Shelter shelter;
    private List<Survivor> survivors;
    private ConsoleHelper console;
    private string[] menuOptions = { "Simulate Days", "View Inventory", "Manage Survivors", "Gather Resources", "Exit" };
    private Random rand = new Random();
    private bool running = true;
    private int dayCounter = 0;

    public Game(ConsoleHelper console)
    {
        this.console = console;
        shelter = new Shelter(rand.Next(5, 15), rand.Next(5, 15), rand.Next(5, 15));
        survivors = new List<Survivor>();
        for (int i = 0; i < rand.Next(3, 7); i++)
        {
            survivors.Add(new Survivor());
        }
    }

    public async Task Start()
    {
        while (running)
        {
            console.ShowMenu(menuOptions, dayCounter);
            await HandleInput();
        }
    }

    private async Task HandleInput()
    {
        string input = await console.GetInput();
        if (int.TryParse(input, out int selectedOption) && selectedOption > 0 && selectedOption <= menuOptions.Length)
        {
            switch (selectedOption)
            {
                case 1:
                    await SimulateDays();
                    break;
                case 2:
                    ViewInventory();
                    break;
                case 3:
                    await ManageSurvivors();
                    break;
                case 4:
                    await GatherResources();
                    break;
                case 5:
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
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
        }
    }

    private async Task SimulateDays()
    {
        console.ClearScreen();
        Console.Write("Enter the number of days to simulate (1-5): ");
        string input = await console.GetInput();
        if (int.TryParse(input, out int days) && days > 0 && days <= 5)
        {
            Console.WriteLine($"Simulating {days} days...");
            for (int i = 0; i < days; i++)
            {
                Console.Write("-");
                await Task.Delay(1000);
                SimulateDay();
            }
            Console.WriteLine("\nSimulation complete. Press any key to return...");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
        }
        Console.ReadKey(true);
    }

    private void SimulateDay()
    {
        dayCounter++;
        survivors.RemoveAll(survivor => !survivor.UpdateStatus());

        //shelter.ChangeResources(-rand.Next(2, 4), -rand.Next(2, 4));

        TriggerEvent();
        CheckEndGameConditions();
    }

    private void TriggerEvent()
    {
        int eventChance = rand.Next(1, 101);
        if (eventChance <= 30)
        {
            int eventType = rand.Next(1, 28);
            switch (eventType)
            {
                case 1:
                    Console.WriteLine("Scavenger’s Luck – A survivor finds a small stash of food and water.");
                    shelter.ChangeResources(5, 5);
                    break;
                case 2:
                    Console.WriteLine("Military Broadcast – A weak radio signal suggests a possible safe zone nearby.");
                    break;
                case 3:
                    Console.WriteLine("Lone Survivor – A new survivor arrives, bringing some supplies.");
                    survivors.Add(new Survivor());
                    shelter.ChangeResources(3, 3);
                    break;
                case 4:
                    Console.WriteLine("Secret Stash – While cleaning the shelter, a survivor finds hidden food.");
                    shelter.ChangeResources(5, 0);
                    break;
                case 5:
                    Console.WriteLine("Fortification Upgrade – Survivors reinforce the shelter, increasing its defense.");
                    break;
                case 6:
                    Console.WriteLine("Altruistic Trader – A passing stranger trades valuable supplies at a fair deal.");
                    shelter.ChangeResources(5, 5);
                    break;
                case 7:
                    Console.WriteLine("Rainwater Collection – A storm fills available containers with fresh water.");
                    shelter.ChangeResources(0, 10);
                    break;
                case 8:
                    Console.WriteLine("Improvised Medicine – A survivor crafts homemade medicine, curing someone’s sickness.");
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
                    Console.WriteLine("Pet Companion – A stray dog joins the shelter, providing protection and morale.");
                    break;
                case 10:
                    Console.WriteLine("Unlooted Pharmacy – A lucky scavenger trip recovers medical supplies.");
                    shelter.ChangeResources(0, 5);
                    break;
                case 11:
                    Console.WriteLine("Raider Ambush – Hostile survivors attack, stealing resources.");
                    shelter.ChangeResources(-5, -5);
                    break;
                case 12:
                    Console.WriteLine("Food Spoilage – Some food goes bad, reducing supplies.");
                    shelter.ChangeResources(-5, 0);
                    break;
                case 13:
                    Console.WriteLine("Water Contamination – Drinking water becomes unsafe, causing sickness.");
                    shelter.ChangeResources(0, -5);
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.GetSick();
                            break;
                        }
                    }
                    break;
                case 14:
                    Console.WriteLine("Zombie Infestation – A zombie finds its way into the shelter, injuring someone.");
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.GetSick();
                            break;
                        }
                    }
                    break;
                case 15:
                    Console.WriteLine("Shelter Collapse – A structural issue damages the shelter, reducing safety.");
                    break;
                case 16:
                    Console.WriteLine("Mental Breakdown – A survivor suffers from stress, refusing to cooperate.");
                    break;
                case 17:
                    Console.WriteLine("Supplies Stolen – A sneaky thief steals items during the night.");
                    shelter.ChangeResources(-5, -5);
                    break;
                case 18:
                    Console.WriteLine("Illness Outbreak – A flu spreads among survivors, requiring medicine.");
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.GetSick();
                        }
                    }
                    break;
                case 19:
                    Console.WriteLine("Equipment Failure – A tool breaks, making scavenging less effective.");
                    break;
                case 20:
                    Console.WriteLine("Horde Sighting – Large groups of zombies are spotted, increasing future danger.");
                    break;
                case 21:
                    Console.WriteLine("Nuke – A nuke is thrown down, instantly ending the game.");
                    running = false;
                    break;
                case 22:
                    Console.WriteLine("Mutant – A mutant is at the front door, requiring lots of ammunition to kill.");
                    shelter.ChangeResources(0, 0, -10);
                    break;
                case 23:
                    Console.WriteLine("Ammunition Cache – A survivor finds a stash of ammunition.");
                    shelter.ChangeResources(0, 0, 10);
                    break;
                case 24:
                    Console.WriteLine("Resource Spoilage – Some resources spoil, reducing supplies.");
                    shelter.ChangeResources(-rand.Next(1, 3), -rand.Next(1, 3));
                    break;
                case 25:
                    Console.WriteLine("Random Death – A survivor dies unexpectedly.");
                    if (survivors.Count > 0)
                    {
                        survivors.RemoveAt(rand.Next(survivors.Count));
                    }
                    break;
                case 26:
                    Console.WriteLine("Thief Attack – A thief steals a significant amount of resources.");
                    shelter.ChangeResources(-rand.Next(3, 6), -rand.Next(3, 6));
                    break;
                case 27:
                    Console.WriteLine("Fire – A fire breaks out, destroying some resources.");
                    shelter.ChangeResources(-rand.Next(2, 5), -rand.Next(2, 5));
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckEndGameConditions()
    {
        if (shelter.Food <= 0 || shelter.Water <= 0)
        {
            Console.WriteLine("All resources depleted. Game over.");
            running = false;
        }
        else if (survivors.Count == 0)
        {
            Console.WriteLine("All survivors are dead. Game over.");
            running = false;
            return;
        }
    }

    private void ViewInventory()
    {
        console.ClearScreen();
        Console.WriteLine("=== Inventory ===");
        Console.WriteLine($"Food: {shelter.Food}");
        Console.WriteLine($"Water: {shelter.Water}");
        Console.WriteLine($"Medicine: {shelter.Medicine}");
        Console.WriteLine($"Ammunition: {shelter.Ammunition}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey(true);
    }

    private async Task ManageSurvivors()
    {
        bool managing = true;
        while (managing)
        {
            console.ClearScreen();
            Console.WriteLine("Managing survivors...");
            Console.WriteLine("1. Feed survivors (-5 Food)");
            Console.WriteLine("2. Give water (-5 Water)");
            Console.WriteLine("3. Provide medicine (-1 Medicine)");
            Console.WriteLine("4. Boost morale (-2 Food, -2 Water)");
            Console.WriteLine("5. Check survivor status");
            Console.WriteLine("6. Stop managing survivors");
            Console.WriteLine("Choose an action: ");
            string choice = await console.GetInput();
            switch (choice)
            {
                case "1":
                    await GiveResource("food");
                    break;
                case "2":
                    await GiveResource("water");
                    break;
                case "3":
                    await GiveMedicine();
                    break;
                case "4":
                    if (shelter.Food >= 2 && shelter.Water >= 2)
                    {
                        shelter.ChangeResources(-2, -2);
                        foreach (var survivor in survivors)
                            survivor.BoostMorale();
                        Console.WriteLine("You organized a morale-boosting activity. Survivors feel better!");
                    }
                    else
                        Console.WriteLine("Not enough resources to boost morale!");
                    break;
                case "5":
                    Console.WriteLine("--- Survivor Status ---");
                    foreach (var survivor in survivors)
                        Console.WriteLine(survivor.GetStatus());
                    break;
                case "6":
                    managing = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey(true);
        }
    }

    private async Task GiveResource(string resourceType)
    {
        int requiredAmount = 5;
        if ((resourceType == "food" && shelter.Food >= requiredAmount) || (resourceType == "water" && shelter.Water >= requiredAmount))
        {
            Console.WriteLine($"Select a survivor to give {resourceType} to:");
            for (int i = 0; i < survivors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {survivors[i].Name}");
            }
            string input = await console.GetInput();
            if (int.TryParse(input, out int selectedSurvivor) && selectedSurvivor > 0 && selectedSurvivor <= survivors.Count)
            {
                if (resourceType == "food")
                {
                    shelter.ChangeResources(-5, 0);
                    survivors[selectedSurvivor - 1].Eat();
                }
                else if (resourceType == "water")
                {
                    shelter.ChangeResources(0, -5);
                    survivors[selectedSurvivor - 1].Drink();
                }
                Console.WriteLine($"You gave {resourceType} to {survivors[selectedSurvivor - 1].Name}.");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
        else
        {
            Console.WriteLine($"Not enough {resourceType}!");
        }
    }

    private async Task GiveMedicine()
    {
        if (shelter.Medicine > 0)
        {
            List<Survivor> sickSurvivors = survivors.FindAll(s => s.IsSick);
            if (sickSurvivors.Count > 0)
            {
                Console.WriteLine("Select a sick survivor to treat:");
                for (int i = 0; i < sickSurvivors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {sickSurvivors[i].Name}");
                }
                string input = await console.GetInput();
                if (int.TryParse(input, out int selectedSurvivor) && selectedSurvivor > 0 && selectedSurvivor <= sickSurvivors.Count)
                {
                    shelter.UseMedicine();
                    sickSurvivors[selectedSurvivor - 1].Heal();
                    Console.WriteLine($"You treated {sickSurvivors[selectedSurvivor - 1].Name}.");
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }
            else
            {
                Console.WriteLine("Let's celebrate, no one is sick!");
            }
        }
        else
        {
            Console.WriteLine("No medicine available!");
        }
    }

    private async Task GatherResources()
    {
        console.ClearScreen();
        Console.WriteLine("Gathering resources...");
        Console.WriteLine("1. Scavenge for food");
        Console.WriteLine("2. Collect water");
        Console.WriteLine("3. Search for medicine");
        Console.WriteLine("4. Hunt for food (uses ammunition)");
        Console.WriteLine("5. Return to main menu");
        Console.WriteLine("Choose an action: ");
        string choice = await console.GetInput();
        switch (choice)
        {
            case "1":
                ScavengeForFood();
                break;
            case "2":
                CollectWater();
                break;
            case "3":
                SearchForMedicine();
                break;
            case "4":
                HuntForFood();
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
        Console.WriteLine("Press any key to return...");
        Console.ReadKey(true);
    }

    private void ScavengeForFood()
    {
        int foodFound = rand.Next(1, 6);
        int failChance = rand.Next(1, 101);
        if (failChance <= 10)
        {
            Console.WriteLine("Scavenging failed. No food found.");
        }
        else
        {
            shelter.ChangeResources(foodFound, 0);
            Console.WriteLine($"You scavenged and found {foodFound} units of food.");
        }
    }

    private void CollectWater()
    {
        int waterCollected = rand.Next(1, 6);
        int failChance = rand.Next(1, 101);
        if (failChance <= 10)
        {
            Console.WriteLine("Water collection failed. No water collected.");
        }
        else
        {
            shelter.ChangeResources(0, waterCollected);
            Console.WriteLine($"You collected {waterCollected} units of water.");
        }
    }

    private void SearchForMedicine()
    {
        int medicineFound = rand.Next(0, 3);
        int failChance = rand.Next(1, 101);
        if (failChance <= 10)
        {
            Console.WriteLine("Medicine search failed. No medicine found.");
        }
        else
        {
            shelter.ChangeResources(0, 0, 0);
            shelter.Medicine += medicineFound;
            Console.WriteLine($"You searched and found {medicineFound} units of medicine.");
        }
    }

    private void HuntForFood()
    {
        if (shelter.Ammunition > 0)
        {
            int foodHunted = rand.Next(3, 8);
            int failChance = rand.Next(1, 101);
            if (failChance <= 10)
            {
                Console.WriteLine("Hunting failed. No food found.");
            }
            else
            {
                shelter.ChangeResources(foodHunted, 0, -1);
                Console.WriteLine($"You hunted and found {foodHunted} units of food, using 1 unit of ammunition.");
            }
        }
        else
        {
            Console.WriteLine("Not enough ammunition to hunt for food.");
        }
    }


    public class ConsoleHelper
    {
        public async Task<string> GetInput()
        {
            return await Task.Run(() => Console.ReadLine());
        }

        public void ShowMenu(string[] options, int dayCounter)
        {
            Console.Clear();
            Console.WriteLine($"Day: {dayCounter}");
            Console.WriteLine("=== Menu ===");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowInlineMessage(string message)
        {
            Console.Write(message);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}