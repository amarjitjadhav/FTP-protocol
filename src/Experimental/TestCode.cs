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
            // Initialize it with the info we've collected
            DFtpAction action = new PutFile(client, localDirectory, localSelection, remoteDirectory, remoteSelection);

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

        /// <summary>
        /// A test usage of the SearchFileRemote action.
        /// </summary>
        /// <param name="client">The ftp client connection.</param>
        /// <param name="pattern">The pattern to search for on the remote server.</param>
        /// <param name="startPath">The starting path to search.</param>
        /// <returns></returns>
        public static DFtpResult SearchFileRemote(FtpClient client, String pattern, String startPath, bool includeSubdirectories = true)
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new SearchFileRemote(client, pattern, startPath, includeSubdirectories);

            
            // Carry out the action and get the result
            DFtpListResult result = (DFtpListResult)action.Run();

            if (result.Type() == DFtpResult.Result.Ok)
            {
                foreach (DFtpFile file in result.Files)
                { 
                    Console.WriteLine("File found: " + file.GetFullPath());
                }
            }
            return result;
        }
    }
}
