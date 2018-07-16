using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XUnitTests
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

        [Fact]
        public void SearchFileNotExists()
        {
            EstablishConnection();
            
            DFtpAction action = new SearchFileRemote(client, "this_file_shouldnt_exist_for_any_reason", "/");
            
            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Error);
        }

        [Fact]
        public void SearchFileExists()
        {
            EstablishConnection();

            DFtpAction action = new SearchFileRemote(client, "TEST_FILE_DONT_DELETE", "/");

            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Ok);
        }

        [Fact]
        public void SearchFileExistsDeepSearch()
        {
            EstablishConnection();

            DFtpAction action = new SearchFileRemote(client, "IM_HIDING", "/TEST_DIRECTORY_DONT_DELETE");

            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Ok);
        }

        [Fact]
        public void SearchFileExistsDeepSearchNotIncludeSubDirectories()
        {
            EstablishConnection();

            DFtpAction action = new SearchFileRemote(client, "IM_HIDING", "/TEST_DIRECTORY_DONT_DELETE", false);

            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Error);
        }

        [Fact]
        public void SearchForFilesWithPatternExists()
        {
            EstablishConnection();
            
            DFtpAction action = new SearchFileRemote(client, "TEST_PATTERN", "/", true);

            DFtpListResult result = (DFtpListResult)action.Run();

            Assert.True(result.Files.Count == 3);
        }

    }
}
