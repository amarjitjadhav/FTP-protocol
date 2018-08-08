using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class RemotePutRenameRemoveTests
    {
        private static FtpClient client = null;
        private static string test_Dir = "/Test1";
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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient, String newFileName)
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpAction action = new CreateDirectoryRemoteAction(ftpClient, test_Dir);
            DFtpResult test = action.Run();
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);
            DFtpAction fileaction = new PutFileAction(client, localDirectory, localSelection, test_Dir);

            DFtpResult result = fileaction.Run();

            return localSelection;
        }

        internal void RenameFileOnServer(FtpClient ftpClient, DFtpFile file, String newName)
        {
            DFtpFile remoteSelection = file;
            DFtpAction action = new RenameFileRemoteAction(ftpClient, test_Dir, remoteSelection, newName);
            DFtpResult result = action.Run();
            return;
        }

        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file)
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new DeleteFileRemoteAction(ftpClient, test_Dir, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }
        internal void GetFileFromRemoteServer(FtpClient ftpClient, String localDirectory, DFtpFile file)
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new GetFileFromRemoteServerAction(ftpClient, localDirectory, test_Dir, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }
        internal bool SearchForFileOnServer(FtpClient ftpClient, String pattern)
        {
            DFtpAction action = new SearchFileRemoteAction(ftpClient, pattern, test_Dir);

            DFtpResult result = action.Run();

            return result.Type == DFtpResultType.Ok ? true : false;
        }

        [Fact]
        public void CreatePutRenameRemoveTest()
        {
            EstablishConnection();
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            // 1. Create and put file on server.
            DFtpFile newFile = CreateAndPutFileOnServer(client, "NewFile");

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForFileOnServer(client, newFile.GetName()));

            // 3. Rename the file
            RenameFileOnServer(client, newFile, "ChangedName");

            // 4. Search for the file by its new name
            Assert.True(SearchForFileOnServer(client, "ChangedName"));

            // 5. Old file should not still exist
            Assert.False(SearchForFileOnServer(client, "NewFile"));

            // 6. Delete it
            client.DeleteFile(test_Dir + "/ChangedName");

            // 7. We should NOT see the file on the server anymore
            Assert.False(SearchForFileOnServer(client, "ChangedName"));
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            return;
        }
    }
}
