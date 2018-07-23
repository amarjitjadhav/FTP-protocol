using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFileAction : DFtpAction
    {
        public PutFileAction(FtpClient ftpClient, String localDirectory, DFtpFile localSelection, String remoteDirectory, DFtpFile remoteSelection) : 
            base(ftpClient, localDirectory, localSelection, remoteDirectory, remoteSelection)
        {
        }

        public override DFtpResult Run()
        {
            String source = localSelection.GetFullPath();
            String target = remoteDirectory + localSelection.GetName();

            return ftpClient.UploadFile(source, target) == true ?
                new DFtpResult(DFtpResultType.Ok) :   // Return ok if upload okay.
                new DFtpResult(DFtpResultType.Error); // Return error if upload fail.
        }
    }
}
