using System;
using System.Threading.Tasks;

public class ConsoleHelper
{
    public async Task<string> GetInput()
    {
        return Console.ReadLine();
    }

    public void ShowMenu(string[] options)
    {
        Console.Clear();
        Console.WriteLine("=== Main Menu ===");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.Write("Select an option: ");
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
