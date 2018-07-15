using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class PutFile : DFtpAction
    {
        public PutFile(FtpClient client, String localDirectory, List<DFtpFile> localFiles, String remoteDirectory, List<DFtpFile> remoteFiles)
            : base(client, localDirectory, localFiles, remoteDirectory, remoteFiles)
        {
        }

        public override DFtpResult Run()
        {
            String source = localFiles[0].GetFullPath();
            String target = remoteDirectory + localFiles[0].GetName();

            if (ftpClient.UploadFile(source, target))
                return new DFtpResult(DFtpResult.Result.Ok);
            return new DFtpResult(DFtpResult.Result.Error);
        }
    }
}
