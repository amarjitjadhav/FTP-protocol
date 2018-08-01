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
      
        public static bool TryConnect(String lastAttemptMessage = "")
        {
            ConnectionInformation connInfo = new ConnectionInformation("", "");
            
            // Get input from user?
            bool newConnection = IOHelper.AskBool("Welcome to the DumbFTP Client " + lastAttemptMessage, "New connection", "Load saved connection");

            if (newConnection)
            {
                // Server name
                String server = IOHelper.AskString("Enter server, or press [Enter] for 'Hypersweet.com'.");
                if (server == "") { server = "hypersweet.com"; }

                // User name
                String user = IOHelper.AskString("Enter username, or press [Enter] for 'cs410'.");
                if (user == "") { user = "cs410"; }

                connInfo = new ConnectionInformation(user, server);
              
            }
            else
            {
                List<ConnectionInformation> savedConnections = ConnectionInformation.GetAllSavedConnections();
                if (savedConnections == null || savedConnections.Count == 0)
                {
                    return TryConnect("[No saved connections]");
                }
                connInfo = IOHelper.Select<ConnectionInformation>("Which connection would you like?", ConnectionInformation.GetAllSavedConnections().ToArray(), true);
            }
            
            // Password
            String password = IOHelper.AskString("Enter password, or press [Enter] for 'cs410'.");
            if (password == "") { password = "cs410"; }

            // Connect the ftp client
            try
            {
                FtpClient client = new FtpClient(connInfo.ServerAddress)
                {
                    Port = 21,
                    Credentials = new NetworkCredential(connInfo.Username, password),
                };

                client.Connect();
                if (client.IsConnected)
                {
                    Client.serverName = connInfo.ServerAddress;
                    Client.ftpClient = client;
                }

            }
            catch (Exception e)
            {
                // Oh, jeeze.
                Client.ftpClient = null;
                Console.WriteLine("Could not connect to server: " + e.Message);
            }

            // See if the user wants to save this new connection.
            if (newConnection && IOHelper.AskBool("Would you like to save this connection information?", "yes", "no"))
            {
                new ConnectionInformation(connInfo.Username, connInfo.ServerAddress).Save();
            }
            
            return Client.ftpClient != null && Client.ftpClient.IsConnected;
        }
    }
}
