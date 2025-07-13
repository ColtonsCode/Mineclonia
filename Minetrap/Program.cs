using System.Reflection.Metadata;
using Minecodel;

namespace Minetrap;

public class Program
{
    public static void Main(string[] args)
    {
        const int width = 100;
        const int height = 100;

        var field = new Minefield(width, height);
        
        Console.WriteLine("Hello, World!");
    }
}