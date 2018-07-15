using System;
using System.Collections.Generic;
using System.Net;
using FluentFTP;

using IO;
using Actions;
using Experimental;

class Program
{
    static void Main(string[] args)
    {
        // Connect the ftp client
        FtpClient client = new FtpClient("hypersweet.com");
        client.Port = 21;
        client.Credentials = new NetworkCredential("cs410", "cs410");

        // Try out some actions
        TestCode.PutFile(client);
        TestCode.Listing(client);
        Console.ReadLine();
    }
}