using System;
using Xunit;

using Actions;
using Utilities;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XUnitTests
{
    public class DFtpActionTests
    {
        [Fact]
        public void PutFileTest()
        {
            FtpClient client = new FtpClient("hypersweet.com");
            client.Port = 21;
            client.Credentials = new NetworkCredential("cs410", "cs410");

            String file = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(file);
            List<DFtpFile> localSelectedFiles = new List<DFtpFile>();
            localSelectedFiles.Add(new DFtpFile(file));

            String remoteDirectory = "/";
            List<DFtpFile> remoteSelectedFiles = new List<DFtpFile>();

            DFtpAction action = new PutFile(client, localDirectory, localSelectedFiles, remoteDirectory, remoteSelectedFiles);
            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Ok);
        }
    }
}
