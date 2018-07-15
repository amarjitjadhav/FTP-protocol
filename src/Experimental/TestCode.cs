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
        /// <summary>
        /// A test usage of the PutFile action.
        /// </summary>
        /// <param name="client">A connected ftp client</param>
        public static void PutFile(FtpClient client)
        {
            // Create a temp file for upload and get its path
            String file = Path.GetTempFileName();

            // Local directory from which the file will be uploaded (maybe not needed)
            String localDirectory = Path.GetDirectoryName(file);

            // Local file selected for upload
            DFtpFile localSelection = new DFtpFile(file);

            // Upload destination
            String remoteDirectory = "/";

            // Remote file selection doesn't matter for upload
            DFtpFile remoteSelection = null;

            // Create the action
            DFtpAction action = new PutFile();

            // Initialize it with the info we've collected
            action.Init(client, localDirectory, localSelection, remoteDirectory, remoteSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();
            if (result.Type() == DFtpResult.Result.Ok)
                Console.WriteLine("Upload result: Ok");
        }

        /// <summary>
        /// Throwaway function for printing out the ftp root directory contents
        /// </summary>
        /// <param name="client">A connected ftp client</param>
        public static void Listing(FtpClient client)
        {
            FtpListItem[] list = client.GetListing("/");
            foreach (FtpListItem item in list)
                Console.WriteLine(item.FullName);
        }
    }
}
