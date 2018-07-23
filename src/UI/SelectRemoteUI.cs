using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;

namespace UI
{
    public class SelectRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.E;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "S(e)lect file/directory";

        public DFtpResult Go()
        {
            // Get listing for remote directory
            FtpListItem[] list = Client.ftpClient.GetListing(Client.remoteDirectory);

            // Choose from files
            FtpListItem selected = IOHelper.Select<FtpListItem>("Choose a remote file to select.", list);

            // Set the client's remote selection
            Client.remoteSelection = new DFtpFile(selected);

            return new DFtpResult(DFtpResultType.Ok, "Listed files in remote directory: " + Client.remoteDirectory);
        }
    }
}
