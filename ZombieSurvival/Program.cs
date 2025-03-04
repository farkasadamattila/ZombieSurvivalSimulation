using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Game.ConsoleHelper consoleHelper = new Game.ConsoleHelper();
        Game game = new Game(consoleHelper);
        await game.Start();
    }
}
