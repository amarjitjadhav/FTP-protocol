using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that renames a file on the ftp server.
    /// FluentFTP doc suggests using MoveFile() rather than low level RenameFile().
    /// </summary>
    public class RenameFileRemoteAction : DFtpAction
    {
        protected String newName;

        /// <summary>
        /// Constructor to build an action that renames a file on the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="remoteDirectory">The remote directory path where the file to be renamed resides.</param>
        /// <param name="remoteSelection">The file to rename</param>

        public RenameFileRemoteAction(FtpClient ftpClient, String remoteDirectory, DFtpFile remoteSelection, String newName)
            : base(ftpClient, null , null, remoteDirectory, remoteSelection)
        {
            this.newName = newName;
        }

        /// <summary>
        /// Try to rename file from the FtpClient. 
        /// Checks if dest exists and skips if it already exists.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file was renamed.</returns>
        /// 

        public override DFtpResult Run()
        {
            String oldPath = remoteDirectory + "/" + remoteSelection.GetName();
            String newPath = remoteDirectory + "/" + newName;
            bool result = false;

            try
            {
                ftpClient.Rename(oldPath, newPath);
                
                return result == false ?
                    new DFtpResult(DFtpResultType.Ok, "File with path \"" + oldPath + "\" moved to \"" + newPath + "\" on remote server.") :
                    new DFtpResult(DFtpResultType.Error, "File with path \"" + oldPath + "\" could not be moved to \"" + newPath + "\" on remote server.");
            }
            catch(Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "File with path \"" + oldPath + "\" " +
                    "could not be moved to \"" + newPath + "\" on remote." + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
    }
}