using FluentFTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Actions
{
    public class DeletFile : DFtpAction
    {
        public DeletFile(FluentFTP.FtpClient ftpClient, DFtpFile remoteSelection)
            : base(ftpClient, null, null, null, remoteSelection)
        {
            
        }
        public override DFtpResult Run(){
           bool isFileExists =  ftpClient.FileExists(remoteSelection.GetFullPath());
           if(isFileExists){
               ftpClient.DeleteFile(remoteSelection.GetFullPath());
               return new DFtpResult(DFtpResultType.Ok);
           }
           return new DFtpResult(DFtpResultType.Error);
        }
    }
}