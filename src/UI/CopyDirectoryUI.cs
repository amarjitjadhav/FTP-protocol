using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class CopyDirectoryUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.D;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => true;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Copy (D)irectory";


        public DFtpResult Go()
        {
            String newName = IOHelper.AskString("Enter name for the new Directory:");

            DFtpAction action = new CopyDirectoryRemoteAction(Client.ftpClient, Client.localDirectory, Client.remoteDirectory, Client.remoteSelection, newName);
            DFtpResult result = action.Run();

            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The directory was successfully copied to '" + newName + "'.");
                Client.remoteSelection = null;
            }

            return result;
        }
    }
}
