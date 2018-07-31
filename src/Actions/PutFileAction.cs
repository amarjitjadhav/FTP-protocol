using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFileAction : DFtpAction
    {
        private bool overwrite = true;

        public PutFileAction(FtpClient ftpClient, String localDirectory, DFtpFile localSelection, String remoteDirectory, bool overwrite = true) : 
            base(ftpClient, localDirectory, localSelection, remoteDirectory, null)
        {
            this.overwrite = overwrite;
        }

        public override DFtpResult Run()
        {
            String source = localSelection.GetFullPath();
            String target = remoteDirectory + "/" + localSelection.GetName();
            FtpExists existsMode = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            bool createDirectoryStructure = true;
            FtpVerify verifyMode = FtpVerify.Retry;
            ftpClient.RetryAttempts = 3;

            try
            { 
                return ftpClient.UploadFile(source, target, existsMode, createDirectoryStructure, verifyMode) == true ?
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
