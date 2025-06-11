// See https://aka.ms/new-console-template for more information

namespace CarGame;

class Program
{
    
    static void Main(string[] args)
    {
        Game game = new Game(1980, 1080);
        Console.WriteLine("It works");
        while (game.running)
        {
            game.Run();
        }
        
    }    
}