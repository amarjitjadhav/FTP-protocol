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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient, String remoteDirectory = "/")
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);


            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        internal void CreatedeepDir(FtpClient client)
        {
            //Make sure the test directory does not already exist
            if (client.DirectoryExists("/Test"))
            {
                client.DeleteDirectory("/Test", FtpListOption.AllFiles);
            }
            //create the new directories
            client.CreateDirectory("/Test");
            client.CreateDirectory("/Test/test3");
            client.CreateDirectory("/Test/test2");
            client.CreateDirectory("/Test/test3/tesst");
            client.CreateDirectory("/Test/test3/tesst/finaltest");

            //insert files into each newly created directory
            for(int x = 0; x < 3; x++)
            {
                CreateAndPutFileOnServer(client, "/Test");
            }
            for (int x = 0; x < 4; x++)
            {
                CreateAndPutFileOnServer(client, "/Test/test2");
            }
            for (int x = 0; x < 4; x++)
            {
                CreateAndPutFileOnServer(client, "/Test/test3");
            }
            for (int x = 0; x < 5; x++)
            {
                CreateAndPutFileOnServer(client, "/Test/test3/tesst");
            }
            for (int x = 0; x < 6; x++)
            {
                CreateAndPutFileOnServer(client, "/Test/test3/tesst/finaltest");
            }

        }
        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file, String remoteDirectory = "/")
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new DeleteFileRemoteAction(ftpClient, remoteDirectory, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }
        internal void GetFileFromRemoteServer(FtpClient ftpClient, String localDirectory, DFtpFile file, String remoteDirectory = "/")
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
        public void CreateAndPutFileOnServerSearchThenRemoveTest()
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

            // Some random new file path.
            String filepath = Path.GetTempFileName();

            // A random file really shouldnt exist on the server.
            DFtpAction action = new SearchFileRemoteAction(client, filepath, "/");

            DFtpResult result = action.Run();

            Assert.True(result.Type == DFtpResultType.Error);
        }

        [Fact]
        public void SearchFileExists()
        {
            EstablishConnection();

            // 1. Create and put file on server.
            DFtpFile tempFile = CreateAndPutFileOnServer(client);

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForFileOnServer(client, tempFile.GetName()));

            // 3. Delete it
            RemoveFileOnServer(client, tempFile);

            return;
        }

        [Fact]
        public void SearchFileExistsDeepSearch()
        {
            EstablishConnection();

            String remoteDirectory = "/way/down/here/in/deep/folder/";

            // 1. Create and put file on server.
            DFtpFile tempFile = CreateAndPutFileOnServer(client, remoteDirectory);

            // 2. Force a recursive search action.
            DFtpAction action = new SearchFileRemoteAction(client, tempFile.GetName(), "/", true);

            DFtpListResult result = (DFtpListResult)action.Run();

            // 3. Delete file.
            RemoveFileOnServer(client, tempFile, remoteDirectory);

            // 4. Delete Directories.
            client.DeleteDirectory("/way", FtpListOption.AllFiles);

            Assert.True(result.Type == DFtpResultType.Ok && result.Files.Count == 1 &&
                result.Files[0].GetName() == tempFile.GetName());
            return;
        }

        [Fact]
        public void SearchFileExistsDeepSearchNotIncludeSubDirectories()
        {
            EstablishConnection();
            String remoteDirectory = "/way/down/here/in/deep/folder/";

            // 1. Create and put file on server.
            DFtpFile tempFile = CreateAndPutFileOnServer(client, remoteDirectory);

            // 2. This should return error, because the file is deep within a directory tree and 
            // we are searching the root directory, non-recursively.
            bool recursiveSearch = false;
            DFtpAction action = new SearchFileRemoteAction(client, tempFile.GetName(), "/", recursiveSearch);

            DFtpResult result = action.Run();

            // 3. Delete file.
            RemoveFileOnServer(client, tempFile, remoteDirectory);

            // 4. Delete Directory.
            client.DeleteDirectory("/way", FtpListOption.AllFiles);

            Assert.True(result.Type == DFtpResultType.Error);
            return;
        }
        [Fact]
        public void CopyDirTest()
        {
            EstablishConnection();
            //check if dir already exists
            if (client.DirectoryExists("/JTest"))
            {
                client.DeleteDirectory("/JTest", FtpListOption.AllFiles);
            }
            //set local and remote paths
            String localdir = Path.GetTempPath();
            String Remotedir = "/";
            //create new directories with files
            CreatedeepDir(client);
            DFtpFile test = new DFtpFile("/Test", FtpFileSystemObjectType.Directory);

            //run the copy directory action
            DFtpAction action = new CopyDirectoryRemoteAction(client, localdir, Remotedir, test, "JTest");
            DFtpResult result = action.Run();

            //get the listings for all the directories we copied
            FtpListItem[] fluentListing = client.GetListing("/JTest", FtpListOption.AllFiles);
            FtpListItem[] fluentListing2 = client.GetListing("/JTest" + "/test2", FtpListOption.AllFiles);
            FtpListItem[] fluentListing3 = client.GetListing("/JTest" + "/test3", FtpListOption.AllFiles);
            FtpListItem[] fluentListing4 = client.GetListing("/JTest" + "/test3" + "/tesst", FtpListOption.AllFiles);
            FtpListItem[] fluentListing5 = client.GetListing("/JTest" + "/test3" + "/tesst" + "/finaltest", FtpListOption.AllFiles);
            
            //compare the file numbers in each directory
            Assert.True(fluentListing.Length == 5);
            Assert.True(fluentListing2.Length == 4);
            Assert.True(fluentListing3.Length == 5);
            Assert.True(fluentListing4.Length == 6);
            Assert.True(fluentListing5.Length == 6);
            //clean up
            client.DeleteDirectory("/JTest", FtpListOption.AllFiles);
            client.DeleteDirectory("/Test", FtpListOption.AllFiles);

            //make sure everything is cleaned up
            Assert.False(client.DirectoryExists("/JTest"));
            Assert.False(client.DirectoryExists("/Test"));
        }
    }
}