using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class SearchPutRemoveRemoteFileTests
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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient)
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath);

            String remoteDirectory = "/";

            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file)
        {
            String remoteDirectory = "/";
            DFtpFile remoteSelection = file;

            DFtpAction action = new DeleteFileRemoteAction(ftpClient, remoteDirectory, remoteSelection);

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
        public void CreateAndUploadThenRemoveFileTest()
        {
            EstablishConnection();

            // 1. Create and put file on server.
            DFtpFile newFile = CreateAndPutFileOnServer(client);

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForFileOnServer(client, newFile.GetName()));

            // 3. Delete it
            RemoveFileOnServer(client, newFile);

            // 4. We should NOT see the file on the server anymore
            Assert.False(SearchForFileOnServer(client, newFile.GetName()));
            return;
        }
        
        [Fact]
        public void SearchFileNotExists()
        {
            EstablishConnection();
            
            DFtpAction action = new SearchFileRemoteAction(client, "this_file_shouldnt_exist_for_any_reason", "/");
            
            DFtpResult result = action.Run();

            Assert.True(result.Type == DFtpResultType.Error);
        }

        [Fact]
        public void SearchFileExists()
        {
            EstablishConnection();

            //String newFilePath = CreateAndPutFileOnServer();

            DFtpAction action = new SearchFileRemoteAction(client, "TEST_FILE_DONT_DELETE", "/");

            DFtpResult result = action.Run();

            Assert.True(result.Type == DFtpResultType.Ok);
        }

        [Fact]
        public void SearchFileExistsDeepSearch()
        {
            EstablishConnection();

            DFtpAction action = new SearchFileRemoteAction(client, "IM_HIDING", "/TEST_DIRECTORY_DONT_DELETE");

            DFtpResult result = action.Run();

            Assert.True(result.Type == DFtpResultType.Ok);
        }

        [Fact]
        public void SearchFileExistsDeepSearchNotIncludeSubDirectories()
        {
            EstablishConnection();

            DFtpAction action = new SearchFileRemoteAction(client, "IM_HIDING", "/TEST_DIRECTORY_DONT_DELETE", false);

            DFtpResult result = action.Run();

            Assert.True(result.Type == DFtpResultType.Error);
        }

        [Fact]
        public void SearchForFilesWithPatternExists()
        {
            EstablishConnection();
            
            DFtpAction action = new SearchFileRemoteAction(client, "TEST_PATTERN", "/", true);

            DFtpListResult result = (DFtpListResult)action.Run();

            Assert.True(result.Files.Count == 3);
        }

    }
}
