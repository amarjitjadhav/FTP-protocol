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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient, String newFileName, String remoteDirectory = "/")
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File, newFileName);


            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        internal void RenameFileOnServer(FtpClient ftpClient, DFtpFile file, String newName, String remoteDirectory = "/")
        {
            DFtpFile remoteSelection = file;
            DFtpAction action = new RenameFileRemoteAction(ftpClient, remoteDirectory, remoteSelection, newName);
            DFtpResult result = action.Run();
            return;
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

            DFtpAction action = new GetFileFromRemoteServer(ftpClient, localDirectory, remoteDirectory, remoteSelection);

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
        public void CreatePutRenameRemoveTest()
        {
            EstablishConnection();

            // 1. Create and put file on server.
            DFtpFile newFile = CreateAndPutFileOnServer(client, "NewFile");

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForFileOnServer(client, "NewFile"));

            // 3. Rename the file
            RenameFileOnServer(client, newFile, "ChangedName");

            // 4. Search for the file by its new name
            Assert.True(SearchForFileOnServer(client, "ChangedName"));

            // 5. Delete it
            RemoveFileOnServer(client, newFile);

            // 6. We should NOT see the file on the server anymore
            Assert.False(SearchForFileOnServer(client, newFile.GetName()));
            return;
        }
    }
}
