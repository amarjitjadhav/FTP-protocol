using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFileAction : DFtpAction
    {
        public PutFileAction(FtpClient ftpClient, String localDirectory, DFtpFile localSelection, String remoteDirectory) : 
            base(ftpClient, localDirectory, localSelection, remoteDirectory, null)
        {
        }

        public override DFtpResult Run()
        {
            String source = localSelection.GetFullPath();
            String target = remoteDirectory + localSelection.GetName();
            try
            { 
                return ftpClient.UploadFile(source, target, FtpExists.Overwrite, true) == true ?
                    new DFtpResult(DFtpResultType.Ok) :   // Return ok if upload okay.
                    new DFtpResult(DFtpResultType.Error); // Return error if upload fail.
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message); // FluentFTP didn't like something.
            }
        }
    }
}
