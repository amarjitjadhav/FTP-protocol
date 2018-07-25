using System;
using Xunit;
using Actions;
using System.Collections.Generic;
using System.IO;
using DumbFTP;

namespace XIntegrationTests
{
    public class ConnectionInformationTests
    {

        [Fact]
        public void SaveConnectionInformation_FileExists()
        {   
            ConnectionInformation connInfo = new ConnectionInformation("cs410", "hypersweet.com");
            connInfo.Save();

            Assert.True(File.Exists("saved_connections/" + connInfo.Username + ".txt"));

            File.Delete("saved_connections/" + connInfo.Username + ".txt");
            return;
        }

        [Fact]
        public void SaveConnectionInformation_LoadInformationMatches()
        {
            String user = "cs410";
            String server = "hypersweet.com";

            ConnectionInformation connInfo = new ConnectionInformation(user, server);
            connInfo.Save();

            ConnectionInformation loadedConnInfo = new ConnectionInformation(user);

            File.Delete("saved_connections/" + connInfo.Username + ".txt");

            Assert.True(
                loadedConnInfo.Username == user && 
                loadedConnInfo.ServerAddress == server);
            return;
        }


        [Fact]
        public void LoadAllConnections()
        {
            // Setup.
            new ConnectionInformation("user1", "server1").Save();
            new ConnectionInformation("user2", "server2").Save();
            new ConnectionInformation("user3", "server3").Save();

            // Action.
            List<ConnectionInformation> list = ConnectionInformation.GetAllSavedConnections();

            Assert.Contains(new ConnectionInformation("user1", "server1"), list);
            Assert.Contains(new ConnectionInformation("user2", "server2"), list);
            Assert.Contains(new ConnectionInformation("user3", "server3"), list);


            File.Delete("saved_connections/user1.txt");
            File.Delete("saved_connections/user2.txt");
            File.Delete("saved_connections/user3.txt");
            return;
        }

    }
}
