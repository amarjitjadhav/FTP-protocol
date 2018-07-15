using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFile : DFtpAction
    {
        public PutFile(FtpClient ftpClient, String localDirectory, DFtpFile localSelection, String remoteDirectory, DFtpFile remoteSelection) : 
            base(ftpClient, localDirectory, localSelection, remoteDirectory, remoteSelection)
        {
        }

        public override DFtpResult Run()
        {
            String source = localSelection.GetFullPath();
            String target = remoteDirectory + localSelection.GetName();

            if (ftpClient.UploadFile(source, target))
                return new DFtpResult(DFtpResult.Result.Ok);
            return new DFtpResult(DFtpResult.Result.Error);
        }
    }
}
