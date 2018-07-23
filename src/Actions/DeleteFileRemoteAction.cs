using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that removes a file from the ftp server.
    /// </summary>
    public class DeleteFileRemoteAction : DFtpAction
    {
        /// <summary>
        /// Constructor to build an action that removes a file from the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="remoteDirectory">The remote directory path where the file to delete resides.</param>
        /// <param name="remoteSelection">The file to remove</param>
        public DeleteFileRemoteAction(FtpClient ftpClient, String remoteDirectory, DFtpFile remoteSelection)
            : base(ftpClient, null, null, remoteDirectory, remoteSelection)
        {
        }

        /// <summary>
        /// Attempt to remote the file from the FtpClient.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file was removed.</returns>
        public override DFtpResult Run()
        {
            String target = remoteDirectory + remoteSelection.GetName();
            
            try
            { 
                // FluentFTP -- Delete me file. pls.
                ftpClient.DeleteFile(target);

                return ftpClient.FileExists(target) == false ?
                    new DFtpResult(DFtpResultType.Ok, "File with path \"" + target + "\" removed from server.") :  
                    new DFtpResult(DFtpResultType.Error, "file with path \"" + target + "\" could not be removed from server.");
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "file with path \"" + target + "\" " +
                    "could not be removed from server." + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
    }
}
