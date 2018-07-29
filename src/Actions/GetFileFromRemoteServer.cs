using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class GetFileFromRemoteServer: DFtpAction {

        public GetFileFromRemoteServer(FtpClient ftpClient, String localDirectory, String remoteDirectory, DFtpFile remoteSelection)
            : base(ftpClient, localDirectory, null,  remoteDirectory, remoteSelection)
        {
        }
         public override DFtpResult Run()
         {
            String remoteSource = remoteDirectory + remoteSelection.GetName();
            String localTarget = localDirectory + remoteSelection.GetName();
            
            try
            { 
                if(ftpClient.FileExists(remoteSource) == false){
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


