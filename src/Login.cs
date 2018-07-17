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
            Console.Write("Enter server: ");
            String server = Console.ReadLine();
            Console.Write("\nEnter username: ");
            String user = Console.ReadLine();
            Console.Write("\nEnter password: ");
            String password = Console.ReadLine();
            Console.WriteLine();

            try {
                // Connect the ftp client
                FtpClient client = new FtpClient(server)
                {
                    Port = 21,
                    Credentials = new NetworkCredential(user, password),
                };
                
                Client.ftpClient = client;
            }
            catch (Exception e)
            {
                Client.ftpClient = null;
                Console.WriteLine("Could not connect to server: " + e.Message);
            }

            return true;
        }
    }
}
