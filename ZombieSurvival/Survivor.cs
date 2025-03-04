using System;

public class Survivor
{
    private static readonly string[] Names = { "Árpád", "Dezsõ", "Köbi Kapitány", "Lézer Jani", "Piros angry bird", "Ferec", "Német ember", "Sósavkapitány", "Szecskamester", "Kacsa" };
    private static readonly Random rand = new Random();

    private int health;
    private int hunger;
    private int thirst;
    private bool isSick;
    private int morale;
    public string Name { get; private set; }
    public bool IsSick;

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

    public void GetSick()
    {
        isSick = true;
    }

    public bool UpdateStatus()
    {
        hunger = Math.Max(0, hunger - 2);
        thirst = Math.Max(0, thirst - 2);
        if (hunger == 0 || thirst == 0)
        {
            health = Math.Max(0, health - 5);
        }
        return health > 0;
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
