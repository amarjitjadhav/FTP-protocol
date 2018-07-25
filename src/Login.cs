using FluentFTP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using IO;

namespace DumbFTP
{
    public class Login
    {
      
        public static bool TryConnect()
        {
            // Get input from user?
            // Server name
            String server = IOHelper.AskString("Enter server, or press [Enter] for 'Hypersweet.com'.");
            if (server == "") { server = "hypersweet.com"; }

            // User name
            String user = IOHelper.AskString("Enter username, or press [Enter] for 'cs410'.");
            if (user == "") { user = "cs410"; }

            // Password
            String password = IOHelper.AskString("Enter password, or press [Enter] for 'cs410'.");
            if (password == "") { password = "cs410"; }

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

            if (IOHelper.AskBool("Would you like to save this connection information?", "yes", "no"))
            {
                ConnectionInformation connInfo = new ConnectionInformation(user, server);
                connInfo.Save();
            }
            
            return Client.ftpClient != null && Client.ftpClient.IsConnected;
        }
    }
}
