using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FluentFTP;
using Actions;

namespace Experimental
{
    public class TestCode
    {
        public static void PutFile(FtpClient client)
        {
            String file = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(file);
            List<DFtpFile> localSelectedFiles = new List<DFtpFile>();
            localSelectedFiles.Add(new DFtpFile(file));

            String remoteDirectory = "/";
            List<DFtpFile> remoteSelectedFiles = new List<DFtpFile>();

            DFtpAction action = new PutFile(client, localDirectory, localSelectedFiles, remoteDirectory, remoteSelectedFiles);
            DFtpResult result = action.Run();
        }

        public static void Listing(FtpClient client)
        {
            FtpListItem[] list = client.GetListing("/");
            foreach (FtpListItem item in list)
                Console.WriteLine(item.FullName);
        }
    }
}
