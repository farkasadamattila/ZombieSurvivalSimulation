public class Shelter
{
    public int Food { get; set; }
    public int Water { get; set; }
    public int Medicine { get; set; }
    public int Ammunition { get; set; }

    public Shelter(int food, int water, int ammunition)
    {
        Food = food;
        Water = water;
        Medicine = 0;
        Ammunition = ammunition;
    }

    public void ChangeResources(int foodChange, int waterChange, int ammunitionChange = 0)
    {
        Food += foodChange;
        Water += waterChange;
        Ammunition += ammunitionChange;
    }

    public void UseMedicine()
    {
        if (Medicine > 0)
        {
            Medicine--;
        }
    }
}
