using System;

public class Shelter
{
    public int Food { get; private set; }
    public int Water { get; private set; }
    public int Medicine { get; private set; } = 5;

    public Shelter(int food, int water)
    {
        Food = food;
        Water = water;
    }

    public void ChangeResources(int foodChange, int waterChange)
    {
        Food = Math.Max(0, Food + foodChange);
        Water = Math.Max(0, Water + waterChange);
    }

    public void UseMedicine()
    {
        Medicine = Math.Max(0, Medicine - 1);
    }
}
