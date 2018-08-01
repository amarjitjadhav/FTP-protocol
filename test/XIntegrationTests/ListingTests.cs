using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class ListingTests
    {
        private static FtpClient client = null;
        private const String testDirectory = "/remote_listing_test_directory";
        private const String LocalTestDirectory = @"C:\TestD";

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

        internal DFtpFile CreateAndPutFileInDirectoryOnServer(FtpClient ftpClient, String remoteDirectory = testDirectory)
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);

            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file, String remoteDirectory = testDirectory)
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new DeleteFileRemoteAction(ftpClient, remoteDirectory, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }

        internal void GetFileFromRemoteServer(FtpClient ftpClient, String localDirectory, DFtpFile file, String remoteDirectory = testDirectory)
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new GetFileFromRemoteServerAction(ftpClient, localDirectory, remoteDirectory, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }

        internal bool SearchForFileOnServer(FtpClient ftpClient, String pattern)
        {
            DFtpAction action = new SearchFileRemoteAction(ftpClient, pattern, "/");

            DFtpResult result = action.Run();

            return result.Type == DFtpResultType.Ok ? true : false;
        }

        [Fact]
        public void GetListingRemoteTest()
        {
            EstablishConnection();
            if (client.DirectoryExists(testDirectory))
            {
                client.DeleteDirectory(testDirectory);
            }
            List<DFtpFile> files = new List<DFtpFile>();
            // Create and put 3 files on server.
            for (int i = 0; i < 3; ++i)
            {
                files.Add(CreateAndPutFileInDirectoryOnServer(client));
            }

            // Get listing of the directory
            DFtpAction action = new GetListingRemoteAction(client, testDirectory);
            DFtpResult result = action.Run();
            DFtpListResult listResult = null;
            if (result is DFtpListResult)
            {
                listResult = (DFtpListResult)result;
            }
            
            else
            {
                return;
            }
            

            // Check that there are three files
            Assert.True(listResult.Files.Count == 3);

            foreach (DFtpFile file in files)
            {
                // Delete each file
                RemoveFileOnServer(client, file);

                // Make sure it's gone
                Assert.False(SearchForFileOnServer(client, file.GetName()));
            }
            if (client.DirectoryExists(testDirectory))
            {
                client.DeleteDirectory(testDirectory);
            }
            return;
        }
        [Fact]
        public void GetListingLocal()
        {
            DFtpAction action = new GetListingLocalAction(LocalTestDirectory);
            DFtpResult result = action.Run();
            DFtpListResult lresult = null;
            if (result is DFtpListResult)
            {
                lresult = (DFtpListResult)result;
            }
            else
            {
                return;
            }
            Assert.True(lresult.Files.Count >= 1);
        }
    }
}
