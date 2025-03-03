using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Game game = new Game(new ConsoleHelper());
        await game.Start();
    }
}
