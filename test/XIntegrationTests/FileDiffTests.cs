using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;
using DumbFTP;

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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient, String remoteDirectory = "/")
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);


            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        [Fact]
        public void FileDiffFilesAreSame_FileDifFReturnsFalse()
        {
            EstablishConnection();
            System.Random random = new System.Random((int)Time.MillisecondsPastEquinox());

            // Create a file, put it on the server. Use FluentFTP to write this 
            // byte sequence to the file.
            byte[] sequence = new byte[1024];
            random.NextBytes(sequence);

            DFtpFile file = CreateAndPutFileOnServer(client, "/");

            using (Stream ostream = client.OpenWrite(file.GetName()))
            {
                try
                {
                    ostream.Write(sequence, 0, sequence.Length);
                }
                finally
                {
                    ostream.Close();
                }
            }

            FtpReply reply = client.GetReply();
            
            // Download the file locally to the current working directory.
            client.DownloadFile(file.GetName(), file.GetName());

            // Compare the local selection with the remote selection.
            Client.localSelection = new DFtpFile(file.GetName(), FtpFileSystemObjectType.File);
            Client.remoteSelection = new DFtpFile(file.GetName(), FtpFileSystemObjectType.File);

            
            // Same files should not be different.
            Assert.True(Client.AreFileSelectionsDifferent() == false);

            // Clean up
            File.Delete(file.GetName());
            client.DeleteFile(file.GetName());

            return;
        }
    }
}
