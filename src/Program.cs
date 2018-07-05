

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Program
{

    static void Main()
    {
        // Mmmmm. This new background color is nice.
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.ForegroundColor = ConsoleColor.Green;

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            running = false;
        }

        return;
    }
}