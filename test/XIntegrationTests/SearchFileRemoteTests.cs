using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class SearchFileRemoteTests
    {
        private static FtpClient client = null;

        internal void EstablishConnection()
        {
            if (client == null)
            {
                client = new FtpClient("hypersweet.com")
                {
                    Port = 21,
                    Credentials = new NetworkCredential("cs410", "cs410")
                };
            }
        }

        internal String CreateAndPutFileOnServer()
        {
            EstablishConnection();
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath);

            String remoteDirectory = "/";
            DFtpFile remoteSelection = null;

            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory, remoteSelection);

            DFtpResult result = action.Run();

            return filepath;
        }

        internal void RemoveFileOnServer(String filepath)
        {

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
