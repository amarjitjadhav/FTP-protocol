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
          
        }

        /// <summary>
        /// Throwaway function for printing out the ftp root directory contents
        /// </summary>
        /// <param name="client">A connected ftp client</param>
        public static void Listing(FtpClient client)
        {
            FtpListItem[] list = client.GetListing("/");
            foreach (FtpListItem item in list)
            { 
                Console.WriteLine(item.FullName);
            }
        }

        /// <summary>
        /// A test usage of the SearchFileRemote action.
        /// </summary>
        /// <param name="client">The ftp client connection.</param>
        /// <param name="pattern">The pattern to search for on the remote server.</param>
        /// <param name="startPath">The starting path to search.</param>
        /// <returns></returns>
        //public static DFtpResult SearchFileRemote(FtpClient client, String pattern, String startPath, bool includeSubdirectories = true)
        //{

        //    return null;
        //}
    }
}
