using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;
using IO;

namespace Actions
{
    /// <summary>
    /// An action that Downloads a file from the ftp server.
    /// </summary>
    public class GetFileFromRemoteServer: DFtpAction {
        /// <summary>
        /// Constructor to build an action that downloads a file from the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="localDirectory">The local directory path</param>
        /// <param name="remoteDirectory">The remote directory path where the file to delete resides.</param>
        /// <param name="remoteSelection">The file to remove</param>
        public GetFileFromRemoteServer(FtpClient ftpClient, String localDirectory, String remoteDirectory, DFtpFile remoteSelection)
            : base(ftpClient, localDirectory, null,  remoteDirectory, remoteSelection)
        {
        }
        /// <summary>
        /// Attempt to download the file from the FtpClient.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file is downloaded.</returns>
        public override DFtpResult Run()
         {
            //if remote source exists.
            if (remoteDirectory == null || remoteSelection == null)
            {
                return new DFtpResult(DFtpResultType.Error, "please select a file before fetching from remote server");
            }

            String fileName = remoteSelection.GetName();
            String remoteSource = remoteDirectory + "/" + fileName;
            String localTarget = localDirectory + (this.isWindows() ? "\\" : "/") + fileName;
            
            try
            { 
                if(ftpClient.FileExists(remoteSource) == false)
                {
                    return  new DFtpResult(DFtpResultType.Error, "file with path \"" + remoteSource + "\" doesn't exists.");
                }
      
                ftpClient.DownloadFile(localTarget, remoteSource);

                return new DFtpResult(DFtpResultType.Ok, "File with path \"" + remoteSource + "\" copied to local directory."); 
                   
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "file with path \"" + remoteSource + "\" " +
                    "could not be copied from server." + ex.Message);
            }
        }
    }
}


