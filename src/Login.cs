using FluentFTP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DumbFTP
{
    public class Login
    {
      
        public static bool TryConnect()
        {
            // Get input from user?
            // String server, String user, String password
            Console.Write("Enter server, or press [Enter] for 'Hypersweet.com': ");
            String server = Console.ReadLine();
            if (server == "") { server = "hypersweet.com"; }
            Console.Write("\nEnter username, or press [Enter] for 'cs410': ");
            String user = Console.ReadLine();
            if (user == "") { user = "cs410"; }
            Console.Write("\nEnter password, or press [Enter] for 'cs410': ");
            String password = Console.ReadLine();
            if (password == "") { password = "cs410"; }
            Console.WriteLine();

            try {
                // Connect the ftp client
                FtpClient client = new FtpClient(server)
                {
                    Port = 21,
                    Credentials = new NetworkCredential(user, password),
                };

                client.Connect();
                if (client.IsConnected)
                { 
                    Client.ftpClient = client;
                }

            }
            catch (Exception e)
            {
                Client.ftpClient = null;
                Console.WriteLine("Could not connect to server: " + e.Message);
            }

            return Client.ftpClient != null && Client.ftpClient.IsConnected;
        }
    }
}
