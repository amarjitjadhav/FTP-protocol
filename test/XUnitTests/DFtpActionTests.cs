using System;
using Xunit;

using Actions;
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
            DFtpFile localSelection = new DFtpFile(file);

            String remoteDirectory = "/";
            DFtpFile remoteSelection = null;

            DFtpAction action = new PutFile();
            action.Init(client, localDirectory, localSelection, remoteDirectory, remoteSelection);
            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Ok);
        }
    }
}
