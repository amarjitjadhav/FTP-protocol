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
        FtpClient client = new FtpClient("hypersweet.com");
        client.Port = 21;
        client.Credentials = new NetworkCredential("cs410", "cs410");

        TestCode.PutFile(client);
        TestCode.Listing(client);
        Console.ReadLine();
    }
}