using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action for searching for a file on the remote server.
    /// </summary>
    public class SearchFileRemoteAction : DFtpAction
    {
        protected String pattern;
        protected String startPath;
        protected bool includeSubdirectories;

        public SearchFileRemoteAction(FtpClient ftpClient, String pattern, String startPath, bool includeSubdirectories = true)
            : base(ftpClient, null, null, null, null)
        {
            this.pattern = pattern;
            this.startPath = startPath;
            this.includeSubdirectories = includeSubdirectories;
        }

        /// <summary>
        /// Run this action.
        /// </summary>
        /// <returns>The DFtpResult casted as DFtpListResult that contains the files that were found.</returns>
        public override DFtpResult Run()
        {
            List<DFtpFile> found = new List<DFtpFile>();
            RecursiveSearchFile(pattern, startPath, ref found);

            String info = new String("Searched for pattern \"" + pattern + "\" on remote server in \"" + startPath + "\"");   

            return  found.Count > 0 ? 
                new DFtpListResult(DFtpResultType.Ok, info + " [Found: " + found.Count + " files]", found) : 
                new DFtpListResult(DFtpResultType.Error, info + " [No files found]", found);
        }

        /// <summary>
        /// Search the remote server for a particular pattern.
        /// </summary>
        /// <param name="pattern">The pattern to search for</param>
        /// <param name="root">The current directory</param>
        /// <param name="found">A list of patterns that match.</param>
        private void RecursiveSearchFile(String pattern, String root, ref List<DFtpFile> found)
        {
            if (found == null)
            {
                return;
            }

            // Search every item in this directory.
            foreach (FtpListItem item in ftpClient.GetListing(root))
            {
                // If its a directory and we are including subdirectories, then recursively search.
                if (item.Type == FtpFileSystemObjectType.Directory)
                {
                    if (includeSubdirectories)
                    { 
                        RecursiveSearchFile(pattern, item.FullName, ref found);
                    }
                }
                else if (item.Name.Contains(pattern))
                {
                    found.Add(new DFtpFile(item));
                }
            }
            return;
        }
    }
}
