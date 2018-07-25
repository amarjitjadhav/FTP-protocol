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

        private static readonly String saveFolder = "saved_connections/";

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
        public ConnectionInformation(String username) => LoadFromUser(username);

        /// <summary>
        /// Save the connection information to a file.
        /// </summary>
        public void Save()
        {
            String filepath = saveFolder + Username + ".txt";


            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            File.WriteAllText(filepath, String.Empty);

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
        public void LoadFromUser(String username)
        {
            String filepath = "saved_connections/" + username + ".txt";

            LoadFromFile(filepath);

            return;
        }

        /// <summary>
        /// Load the connection information from a file the filename.
        /// </summary>
        /// <param name="username"></param>
        public void LoadFromFile(String filepath)
        {
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

        public static List<ConnectionInformation> GetAllSavedConnections()
        {
            List<ConnectionInformation> result = new List<ConnectionInformation>();

            if (Directory.Exists(saveFolder))
            {
                foreach (String file in Directory.GetFiles(saveFolder))
                {
                    ConnectionInformation toAdd = new ConnectionInformation("", "");
                    toAdd.LoadFromFile(file);
                    result.Add(toAdd);
                    
                }
            }


            return result;
        }

        /// <summary>
        /// Compares this object with another for equivalence
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True, if this object is equal to obj</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is ConnectionInformation other))
            {
                return false;
            }
            else
            {
                return
                    this.Username.Equals(other.Username) &&
                    this.ServerAddress.Equals(other.ServerAddress);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return this.Username.GetHashCode() + this.ServerAddress.GetHashCode(); ;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "User: " + this.Username + "  Server: " + this.ServerAddress;
        }
    }
}
