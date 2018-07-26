using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class FileAndDirectoryManipTests
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
            return client;
        }



        [Fact]
        public void CreateDirectoryTest()
        {
            EstablishConnection();

            String directoryName = "temporary_test_directory";


            // Create directory
            DFtpAction action = new CreateDirectoryRemoteAction(client, "/" + directoryName);
            DFtpResult result = action.Run();

            // Check for error
            Assert.True(result.Type == DFtpResultType.Ok);

            // Get listing and check that directory exists
            FtpListItem[] files = client.GetListing("/");
            bool found = false;
            foreach (FtpListItem item in files)
            {
                if (item.Name == directoryName)
                    found = true;
            }
            Assert.True(found);

            return;
        }
    }
}
