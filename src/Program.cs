using System;
using System.Net;
using FluentFTP;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
           FtpClient client = new FtpClient("hypersweet.com");
            client.Port = 21;

            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential("cs410", "cs410");

            // begin connecting to the server
            client.Connect();

            bool running = true;

            while (running)
            {
                Console.WriteLine("Hello World!");
                Console.ReadLine();
                running = false;
            }
        }
    }
}
