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

    }
}
