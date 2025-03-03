using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Game
{
    private Shelter shelter;
    private List<Survivor> survivors;
    private ConsoleHelper console;
    private string[] menuOptions = { "Simulate Days", "View Inventory", "Manage Survivors", "Exit" };
    private Random rand = new Random();
    private bool running = true;

    public Game(ConsoleHelper console)
    {
        this.console = console;
        shelter = new Shelter(rand.Next(10, 30), rand.Next(10, 30));
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
            console.ShowMenu(menuOptions);
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
            console.ShowMessage("Invalid input. Please enter a number between 1 and 4.");
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
                await Task.Delay(500); // Simulate some delay
                SimulateDay();
            }
            console.ShowMessage("\nSimulation complete. Press any key to return...");
        }
        else
        {
            console.ShowMessage("Invalid input. Please enter a number between 1 and 5.");
        }
        Console.ReadKey();
    }

    private void SimulateDay()
    {
        // Adjust survivor status and remove dead survivors
        survivors.RemoveAll(survivor => !survivor.UpdateStatus());

        // Adjust inventory
        shelter.ChangeResources(-rand.Next(1, 3), -rand.Next(1, 3));

        // Trigger random event
        TriggerEvent();
    }

    private void TriggerEvent()
    {
        int eventChance = rand.Next(1, 101);
        if (eventChance <= 20) // 20% chance for an event
        {
            int eventType = rand.Next(1, 21);
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
                    shelter.ChangeResources(0, -10);
                    break;
                default:
                    break;
            }
        }
    }

    private void ViewInventory()
    {
        console.ClearScreen();
        console.ShowMessage("=== Inventory ===");
        console.ShowMessage($"Food: {shelter.Food}");
        console.ShowMessage($"Water: {shelter.Water}");
        console.ShowMessage($"Medicine: {shelter.Medicine}");
        console.ShowMessage("Press any key to return...");
        Console.ReadKey();
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
            Console.ReadKey();
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
}
