using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFile : DFtpAction
    {
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
