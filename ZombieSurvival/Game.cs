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
                    console.ShowMessage("Exiting game...");
                    break;
                default:
                    console.ShowMessage("Invalid option. Please try again.");
                    break;
            }
        }
        else
        {
            console.ShowMessage("Invalid input. Please enter a number between 1 and 5.");
        }
    }

    private async Task SimulateDays()
    {
        console.ClearScreen();
        console.ShowInlineMessage("Enter the number of days to simulate (1-5): ");
        string input = await console.GetInput();
        if (int.TryParse(input, out int days) && days > 0 && days <= 5)
        {
            console.ShowMessage($"Simulating {days} days...");
            for (int i = 0; i < days; i++)
            {
                console.ShowInlineMessage("-");
                await Task.Delay(1000);
                SimulateDay();
            }
            console.ShowMessage("\nSimulation complete. Press any key to return...");
        }
        else
        {
            console.ShowMessage("Invalid input. Please enter a number between 1 and 5.");
        }
        Console.ReadKey(true);
    }

    private void SimulateDay()
    {
        dayCounter++;
        survivors.RemoveAll(survivor => !survivor.UpdateStatus());

        shelter.ChangeResources(-rand.Next(2, 4), -rand.Next(2, 4));

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
                    console.ShowMessage("Scavenger’s Luck – A survivor finds a small stash of food and water.");
                    shelter.ChangeResources(5, 5);
                    break;
                case 2:
                    console.ShowMessage("Military Broadcast – A weak radio signal suggests a possible safe zone nearby.");
                    break;
                case 3:
                    console.ShowMessage("Lone Survivor – A new survivor arrives, bringing some supplies.");
                    survivors.Add(new Survivor());
                    shelter.ChangeResources(3, 3);
                    break;
                case 4:
                    console.ShowMessage("Secret Stash – While cleaning the shelter, a survivor finds hidden food.");
                    shelter.ChangeResources(5, 0);
                    break;
                case 5:
                    console.ShowMessage("Fortification Upgrade – Survivors reinforce the shelter, increasing its defense.");
                    break;
                case 6:
                    console.ShowMessage("Altruistic Trader – A passing stranger trades valuable supplies at a fair deal.");
                    shelter.ChangeResources(5, 5);
                    break;
                case 7:
                    console.ShowMessage("Rainwater Collection – A storm fills available containers with fresh water.");
                    shelter.ChangeResources(0, 10);
                    break;
                case 8:
                    console.ShowMessage("Improvised Medicine – A survivor crafts homemade medicine, curing someone’s sickness.");
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
                    console.ShowMessage("Pet Companion – A stray dog joins the shelter, providing protection and morale.");
                    break;
                case 10:
                    console.ShowMessage("Unlooted Pharmacy – A lucky scavenger trip recovers medical supplies.");
                    shelter.ChangeResources(0, 5);
                    break;
                case 11:
                    console.ShowMessage("Raider Ambush – Hostile survivors attack, stealing resources.");
                    shelter.ChangeResources(-5, -5);
                    break;
                case 12:
                    console.ShowMessage("Food Spoilage – Some food goes bad, reducing supplies.");
                    shelter.ChangeResources(-5, 0);
                    break;
                case 13:
                    console.ShowMessage("Water Contamination – Drinking water becomes unsafe, causing sickness.");
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
                    console.ShowMessage("Zombie Infestation – A zombie finds its way into the shelter, injuring someone.");
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
                    console.ShowMessage("Shelter Collapse – A structural issue damages the shelter, reducing safety.");
                    break;
                case 16:
                    console.ShowMessage("Mental Breakdown – A survivor suffers from stress, refusing to cooperate.");
                    break;
                case 17:
                    console.ShowMessage("Supplies Stolen – A sneaky thief steals items during the night.");
                    shelter.ChangeResources(-5, -5);
                    break;
                case 18:
                    console.ShowMessage("Illness Outbreak – A flu spreads among survivors, requiring medicine.");
                    foreach (var survivor in survivors)
                    {
                        if (!survivor.IsSick)
                        {
                            survivor.GetSick();
                        }
                    }
                    break;
                case 19:
                    console.ShowMessage("Equipment Failure – A tool breaks, making scavenging less effective.");
                    break;
                case 20:
                    console.ShowMessage("Horde Sighting – Large groups of zombies are spotted, increasing future danger.");
                    break;
                case 21:
                    console.ShowMessage("Nuke – A nuke is thrown down, instantly ending the game.");
                    running = false;
                    break;
                case 22:
                    console.ShowMessage("Mutant – A mutant is at the front door, requiring lots of ammunition to kill.");
                    shelter.ChangeResources(0, 0, -10);
                    break;
                case 23:
                    console.ShowMessage("Ammunition Cache – A survivor finds a stash of ammunition.");
                    shelter.ChangeResources(0, 0, 10);
                    break;
                case 24:
                    console.ShowMessage("Resource Spoilage – Some resources spoil, reducing supplies.");
                    shelter.ChangeResources(-rand.Next(1, 4), -rand.Next(1, 4));
                    break;
                case 25:
                    console.ShowMessage("Random Death – A survivor dies unexpectedly.");
                    if (survivors.Count > 0)
                    {
                        survivors.RemoveAt(rand.Next(survivors.Count));
                    }
                    break;
                case 26:
                    console.ShowMessage("Thief Attack – A thief steals a significant amount of resources.");
                    shelter.ChangeResources(-rand.Next(3, 6), -rand.Next(3, 6));
                    break;
                case 27:
                    console.ShowMessage("Fire – A fire breaks out, destroying some resources.");
                    shelter.ChangeResources(-rand.Next(2, 5), -rand.Next(2, 5));
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckEndGameConditions()
    {
        if (shelter.Food <= 0 && shelter.Water <= 0 && shelter.Medicine <= 0 && shelter.Ammunition <= 0)
        {
            console.ShowMessage("All resources depleted. Game over.");
            running = false;
        }
        else if (survivors.Count == 0)
        {
            console.ShowMessage("All survivors are dead. Game over.");
            running = false;
        }
    }

    private void ViewInventory()
    {
        console.ClearScreen();
        console.ShowMessage("=== Inventory ===");
        console.ShowMessage($"Food: {shelter.Food}");
        console.ShowMessage($"Water: {shelter.Water}");
        console.ShowMessage($"Medicine: {shelter.Medicine}");
        console.ShowMessage($"Ammunition: {shelter.Ammunition}");
        console.ShowMessage("Press any key to return...");
        Console.ReadKey(true);
    }

    private async Task ManageSurvivors()
    {
        bool managing = true;
        while (managing)
        {
            console.ClearScreen();
            console.ShowMessage("Managing survivors...");
            console.ShowMessage("1. Feed survivors (-5 Food)");
            console.ShowMessage("2. Give water (-5 Water)");
            console.ShowMessage("3. Provide medicine (-1 Medicine)");
            console.ShowMessage("4. Boost morale (-2 Food, -2 Water)");
            console.ShowMessage("5. Check survivor status");
            console.ShowMessage("6. Stop managing survivors");
            console.ShowMessage("Choose an action: ");
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
                        console.ShowMessage("You organized a morale-boosting activity. Survivors feel better!");
                    }
                    else
                        console.ShowMessage("Not enough resources to boost morale!");
                    break;
                case "5":
                    console.ShowMessage("--- Survivor Status ---");
                    foreach (var survivor in survivors)
                        console.ShowMessage(survivor.GetStatus());
                    break;
                case "6":
                    managing = false;
                    break;
                default:
                    console.ShowMessage("Invalid choice.");
                    break;
            }
            console.ShowMessage("Press any key to return...");
            Console.ReadKey(true);
        }
    }

    private async Task GiveResource(string resourceType)
    {
        int requiredAmount = 5;
        if ((resourceType == "food" && shelter.Food >= requiredAmount) || (resourceType == "water" && shelter.Water >= requiredAmount))
        {
            console.ShowMessage($"Select a survivor to give {resourceType} to:");
            for (int i = 0; i < survivors.Count; i++)
            {
                console.ShowMessage($"{i + 1}. {survivors[i].Name}");
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
                console.ShowMessage($"You gave {resourceType} to {survivors[selectedSurvivor - 1].Name}.");
            }
            else
            {
                console.ShowMessage("Invalid selection.");
            }
        }
        else
        {
            console.ShowMessage($"Not enough {resourceType}!");
        }
    }

    private async Task GiveMedicine()
    {
        if (shelter.Medicine > 0)
        {
            List<Survivor> sickSurvivors = survivors.FindAll(s => s.IsSick);
            if (sickSurvivors.Count > 0)
            {
                console.ShowMessage("Select a sick survivor to treat:");
                for (int i = 0; i < sickSurvivors.Count; i++)
                {
                    console.ShowMessage($"{i + 1}. {sickSurvivors[i].Name}");
                }
                string input = await console.GetInput();
                if (int.TryParse(input, out int selectedSurvivor) && selectedSurvivor > 0 && selectedSurvivor <= sickSurvivors.Count)
                {
                    shelter.UseMedicine();
                    sickSurvivors[selectedSurvivor - 1].Heal();
                    console.ShowMessage($"You treated {sickSurvivors[selectedSurvivor - 1].Name}.");
                }
                else
                {
                    console.ShowMessage("Invalid selection.");
                }
            }
            else
            {
                console.ShowMessage("Let's celebrate, no one is sick!");
            }
        }
        else
        {
            console.ShowMessage("No medicine available!");
        }
    }

    private async Task GatherResources()
    {
        console.ClearScreen();
        console.ShowMessage("Gathering resources...");
        console.ShowMessage("1. Scavenge for food");
        console.ShowMessage("2. Collect water");
        console.ShowMessage("3. Search for medicine");
        console.ShowMessage("4. Hunt for food (uses ammunition)");
        console.ShowMessage("5. Return to main menu");
        console.ShowMessage("Choose an action: ");
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
                console.ShowMessage("Invalid choice.");
                break;
        }
        console.ShowMessage("Press any key to return...");
        Console.ReadKey(true);
    }

    private void ScavengeForFood()
    {
        int foodFound = rand.Next(1, 6);
        shelter.ChangeResources(foodFound, 0);
        console.ShowMessage($"You scavenged and found {foodFound} units of food.");
    }

    private void CollectWater()
    {
        int waterCollected = rand.Next(1, 6);
        shelter.ChangeResources(0, waterCollected);
        console.ShowMessage($"You collected {waterCollected} units of water.");
    }

    private void SearchForMedicine()
    {
        int medicineFound = rand.Next(0, 3);
        shelter.ChangeResources(0, 0, 0);
        shelter.Medicine += medicineFound;
        console.ShowMessage($"You searched and found {medicineFound} units of medicine.");
    }

    private void HuntForFood()
    {
        if (shelter.Ammunition > 0)
        {
            int foodHunted = rand.Next(3, 8);
            shelter.ChangeResources(foodHunted, 0, -1);
            console.ShowMessage($"You hunted and found {foodHunted} units of food, using 1 unit of ammunition.");
        }
        else
        {
            console.ShowMessage("Not enough ammunition to hunt for food.");
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