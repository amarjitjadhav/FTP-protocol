using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XUnitTests
{
    public class PutFileTests
    {
        private FtpClient client;

        internal void EstablishConnection()
        {
            client = new FtpClient("hypersweet.com")
            {
                Port = 21,
                Credentials = new NetworkCredential("cs410", "cs410")
            };
        }


        [Fact]
        public void PutFileTestValidFile()
        {
            EstablishConnection();
            // This is a mock file that doesnt really exist.
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath);
            
            String remoteDirectory = "/";
            DFtpFile remoteSelection = null;

            DFtpAction action = new PutFile(client, localDirectory, localSelection, remoteDirectory, remoteSelection);
            
            DFtpResult result = action.Run();

            Assert.True(result.Type() == DFtpResult.Result.Ok);
        }
    }
}
