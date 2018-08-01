using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that renames a file on the local server.
    /// FluentFTP doc suggests using MoveFile() rather than low level RenameFile().
    /// </summary>
    public class RenameFileLocalAction : DFtpAction
    {
        protected String newName;

        /// <summary>
        /// Constructor to build an action that renames a file on local server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="localDirectory">The local directory path where the file to rename resides.</param>
        /// <param name="localDirectory">The file to rename</param>

        public RenameFileLocalAction(FtpClient ftpClient, String localDirectory, DFtpFile localSelection, String newName)
            : base(null, localDirectory, localSelection, null, null)
        {
            this.newName = newName;
        }

        /// <summary>
        /// Try to rename file on local server. 
        /// Checks if dest exists and skips if it already exists.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file was renamed.</returns>
        /// 

        public override DFtpResult Run()
        {
            String oldPath = localDirectory + Path.DirectorySeparatorChar + localSelection.GetName();
            String newPath = localDirectory + Path.DirectorySeparatorChar + newName;
            try
            {
                try
                {
                    File.Move(oldPath, newPath);
                    return new DFtpResult(DFtpResultType.Ok, "File with path \"" + oldPath + "\" moved to \"" + newPath + "\" on local server.");
                }
                catch(Exception x)
                {
                    return new DFtpResult(DFtpResultType.Error, "File with path \"" + oldPath + "\" could not be moved to \"" + newPath + "\" on local server. Error:" + x);
                }
            }
            catch(Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "File with path \"" + oldPath + "\" " +
                    "could not be moved to \"" + newPath + "\" on local." + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
    }
}