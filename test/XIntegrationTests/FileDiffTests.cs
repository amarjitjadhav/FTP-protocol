using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class FileDiffTests
    {
        private static FtpClient client = null;

        internal FtpClient EstablishConnection()
        {
            if (client == null)
            {
                client = new FtpClient("hypersweet.com")
                {
                    Port = 21,
                    Credentials = new NetworkCredential("cs410", "cs410")
                };
            }
            Client.ftpClient = client;
            return client;
        }

        [Fact]
        public void FileDiffFilesAreSame_FileDifFReturnsFalse()
        {
            EstablishConnection();

            // We need download functionality to test this further.
            Client.localSelection = new DFtpFile("C:\\Users\\User\\Downloads\\same.txt");
            Client.remoteSelection = new DFtpFile("/same.txt");

            // Same files should not be different.
            Assert.True(Client.AreFileSelectionsDifferent() == false);

            return;
        }
    }
}
