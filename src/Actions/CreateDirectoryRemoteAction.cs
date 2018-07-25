using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    /// <summary>
    /// Create a directory on the remote server, also creating other parent directories on the way.
    /// </summary>
    public class CreateDirectoryRemoteAction : DFtpAction
    {
        private String newDirectoryPath = null;

        /// <summary>
        /// Constructor for action.
        /// </summary>
        /// <param name="ftpClient">Currently connected client.</param>
        /// <param name="newDirectoryPath">Path of the directory to create.</param>
        public CreateDirectoryRemoteAction(FtpClient ftpClient, String newDirectoryPath) : 
            base(ftpClient, null, null, null, null)
        {
            this.newDirectoryPath = newDirectoryPath;
        }

        public override DFtpResult Run()
        {
            try
            {
                ftpClient.CreateDirectory(newDirectoryPath, true);
                return new DFtpResult(DFtpResultType.Ok, "Created directory " + newDirectoryPath);
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}
