using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentFTP;

[ExcludeFromCodeCoverage]
public class Program
{

    static void Main()
    {
        // Mmmmm. This new background color is nice.
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.ForegroundColor = ConsoleColor.Green;

        FtpClient client = new FtpClient("98.246.48.95");
        client.Port = 21;

        // if you don't specify login credentials, we use the "anonymous" user account
        client.Credentials = new NetworkCredential("", "");

        // begin connecting to the server
        client.Connect();

        bool running = true;

        while (running)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            running = false;
        }

        return;
    }

}