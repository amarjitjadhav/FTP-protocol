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

        
        if (TestCode.SearchFileRemote(client, "TEST_FILE_DONT_DELETE", "/", true).Type() == DFtpResult.Result.Error)
        {
            Console.WriteLine("\nTEST_FILE_DONT_DELETE was not found on the server, did someone delete it?");
        }
        Console.ReadLine();
    }
}