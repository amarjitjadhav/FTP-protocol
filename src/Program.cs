using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Renci.SshNet;


[ExcludeFromCodeCoverage]
public class Program
{

    static void Main()
    {
        // Mmmmm. This new background color is nice.
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.ForegroundColor = ConsoleColor.Green;

        // This is a code example from https://gist.github.com/piccaso/d963331dcbf20611b094#file-ssh-cs-L33
        
        // Setup Credentials and Server Information
        ConnectionInfo ConnNfo = new ConnectionInfo("192.168.1.205", 22, "rando",
            new AuthenticationMethod[]{

                // Pasword based Authentication
                new PasswordAuthenticationMethod("rando","all"),

                // Key Based Authentication (using keys in OpenSSH Format)
                //new PrivateKeyAuthenticationMethod("rando",new PrivateKeyFile[]{
                //    new PrivateKeyFile(@"..\openssh.key","passphrase")
                //}),
            }
        );
        
        // Execute a (SHELL) Command - prepare upload directory
        using (var sshclient = new SshClient(ConnNfo))
        {
            sshclient.Connect();
            Console.WriteLine(sshclient.CreateCommand("ls -lah").Execute());
            sshclient.Disconnect();
        }

        bool running = true;

        while (running)
        {
            //Console.Clear();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            running = false;
        }

        return;
    }

}