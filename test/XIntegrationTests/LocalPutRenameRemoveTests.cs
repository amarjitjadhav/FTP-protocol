using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class LocalPutRenameRemoveTests
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

        internal void CreateAndPutFileOnLocal(String path, String originalName)
        {
            Directory.CreateDirectory(path);
            File.Create(path + "/" + originalName);
        }

        internal DFtpFile CreateAndPutFileOnLocal(FtpClient ftpClient, String filePath, String newFileName)
        {
            DFtpFile localSelection = new DFtpFile(filePath + "/" + newFileName, FtpFileSystemObjectType.File, newFileName);
            Directory.CreateDirectory(filePath);
            FileStream newStream = File.Create(filePath + "/" + newFileName);
            newStream.Close();

            return localSelection;
        }

        internal bool SearchForLocalFile(String path, String filename)
        {
            if (File.Exists(path + "/" + filename)) {
                return true;
            }
            else { return false; }
        }

        internal void RenameLocalFile(FtpClient ftpClient, DFtpFile file, String newName)
        {
            DFtpFile localSelection = file;
            DFtpAction action = new RenameFileLocalAction(ftpClient, test_Dir, localSelection, newName);
            DFtpResult result = action.Run();
            return;
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
            DFtpFile newFile = CreateAndPutFileOnLocal(client, test_Dir, "NewFile");

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForLocalFile(test_Dir, "NewFile"));

            // 3. Rename the file
            RenameLocalFile(client, newFile, "ChangedName");

            // 4. Ensure old name no longer exists
            Assert.False(SearchForLocalFile(test_Dir, "NewFile"));

            // 5. Search for the file by its new name
            Assert.True(SearchForLocalFile(test_Dir, "ChangedName"));

            // 6. Delete it
            File.Delete(test_Dir + "/ChangedName");

            // 7. We should NOT see the file on the server anymore
            Assert.False(SearchForLocalFile(test_Dir, "ChangedName"));
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            return;
        }
    }
}
