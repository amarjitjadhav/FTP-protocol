using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;
using System.Linq;

namespace XIntegrationTests
{
    public class DeleteDirectoryTest
    {
        private static FtpClient client = null;
        private const String testDirectory = "/remote_listing_test_directory/";
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

        internal DFtpFile CreateAndPutDirectoryOnServer(FtpClient ftpClient, String remoteDirectory = testDirectory)
        {
            String dirpath = Path.GetDirectoryName(remoteDirectory);
            //String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(dirpath, FtpFileSystemObjectType.Directory);

            DFtpAction action = new CreateDirectoryRemoteAction(client, dirpath);

            DFtpResult result = action.Run();

            return localSelection;
        }

        internal void RemoveDirectoryOnServer(FtpClient ftpClient, String remoteDirectory = testDirectory)
        {
            //DFtpFile remoteSelection = remoteDirectory;

            DFtpAction action = new DeleteRemoteDirectoryAction(ftpClient, remoteDirectory);

            DFtpResult result = action.Run();
            return;
        }

        internal void GetDirectoryFromRemoteServer(FtpClient ftpClient, String remoteDirectory = testDirectory)
        {
            //DFtpFile remoteSelection = file;

            DFtpAction action = new GetListingRemoteAction(ftpClient, remoteDirectory);

            DFtpResult result = action.Run();
            return;
        }
        ///
        /// <summary> action to write for SearchForDirectoryOnServer <summary>
        /// 
    
        // internal bool SearchForDirectoryOnServer(FtpClient ftpClient, String pattern)
        // {
        //     DFtpAction action = new SearchDirectoryRemoteAction(ftpClient, pattern, "/");

        //     DFtpResult result = action.Run();

        //     return result.Type == DFtpResultType.Ok ? true : false;
        // }

        [Fact]
        public void GetListingRemoteTest()
        {
            EstablishConnection();

            List<DFtpFile> directory = new List<DFtpFile>();
            // Create and put 3 files on server.
            for (int i = 0; i < 3; ++i)
            {
                //files.Add(CreateAndPutFileInDirectoryOnServer(client));
                directory.Add(CreateAndPutDirectoryOnServer(client));
            }

            // Get listing of the directory
            DFtpAction action = new GetListingRemoteAction(client, testDirectory);
            DFtpResult result = action.Run();
            DFtpListResult listResult = null;
            if (result is DFtpListResult)
            {
                listResult = (DFtpListResult)result;
                
                var directories =  listResult.Files.Where(x => x.Type() == FtpFileSystemObjectType.Directory); //.Type() == FtpFileSystemObjectType.Directory);
                var cnt = directories.ToList().Count;
                //Assert.True(cnt == 3);
            }
            
            else
            {
                return;
            }
            

            // Check that there are three files
            //Assert.True(listResult.Equals(directory));

            // foreach (DFtpFile file in files)
            // {
            //     // Delete each file
            //     //RemoveFileOnServer(client, file);
            //     RemoveDirectoryOnServer(client, )
            //     {
                    
            //     }

            //     // Make sure it's gone
            //     Assert.False(SearchForFileOnServer(client, file.GetName()));
            // }

            return;
        }
    
    }
}
