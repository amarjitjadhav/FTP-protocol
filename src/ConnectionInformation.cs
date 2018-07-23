using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DumbFTP
{
    /// <summary>
    /// Class that holds information about a particular connection.
    /// </summary>
    public class ConnectionInformation
    {
        public String Username { get; private set; } = "";
        
        public String ServerAddress { get; private set; } = "";

        /// <summary>
        /// Constructor to build a connection from information about the user passed in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="serverAddress"></param>
        public ConnectionInformation(String username, String serverAddress)
        {
            this.Username = username;
            this.ServerAddress = serverAddress;
        }
        /// <summary>
        /// Constructor that loads the connection information from a username, if
        /// a user connection by that name has already been saved to a file.
        /// </summary>
        /// <param name="user"></param>
        public ConnectionInformation(String username) => Load(username);

        /// <summary>
        /// Save the connection information to a file.
        /// </summary>
        public void Save()
        {
            String filepath = "saved_connections/" + Username + ".txt";

            File.WriteAllText(filepath, String.Empty);

            if (!Directory.Exists("saved_connections"))
            {
                Directory.CreateDirectory("saved_connections");
            }

            StreamWriter writer = new StreamWriter(File.Open(filepath, System.IO.FileMode.Create))
            {
                AutoFlush = true
            };

            writer.WriteLine("{0},{1}", Username, ServerAddress);
            writer.Close();
            return;
        }

        /// <summary>
        /// Load the connection information from a file by the username.
        /// </summary>
        /// <param name="username"></param>
        public void Load(String username)
        {
            String filepath = "saved_connections/" + username + ".txt";

            StreamReader reader = new StreamReader(File.Open(filepath, System.IO.FileMode.Open));
            String line = reader.ReadLine();
            reader.Close();

            String[] tokens = line.Split(',');
            if (tokens != null && tokens.Length == 2)
            {
                this.Username = tokens[0];
                this.ServerAddress = tokens[1];
            }
            
            return;
        }
    }
}
