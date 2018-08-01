
using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that removes a directory from the ftp server.
    /// </summary>
    public class DeletRemoteDirectory:DFtpAction
    {
        /// <summary>
        /// Constructor to build an action that removes a directory from the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="remoteDirectory">The remote directory path where the file to delete resides.</param>
        public DeletRemoteDirectory(FtpClient ftpClient, String remoteDirectory)
            : base(ftpClient, null, null, remoteDirectory,null)
        {
        }

        public override DFtpResult Run()
        {
            String target = remoteDirectory.GetFtpDirectoryName();
            
            try
            { 
                // FluentFTP -- Delete me file. pls.
                ftpClient.DeleteDirectory(target);

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