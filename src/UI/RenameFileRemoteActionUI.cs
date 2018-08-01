using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class RenameFileRemoteActionUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.R;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(R)ename File";


        public DFtpResult Go()
        {
            String newName = IOHelper.AskString("Enter replacement name:");
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new RenameFileRemoteAction(Client.ftpClient, Client.remoteDirectory, Client.remoteSelection, newName);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file was successfully renamed to '" + newName + "'.");
                Client.remoteSelection = null;
            }

            return result;
        }
    }
}
