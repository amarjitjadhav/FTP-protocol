using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class RenameFileLocalActionUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.R;

        public bool RequiresLogin => true;

        public bool RequiresSelection => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(R)ename File";


        public DFtpResult Go()
        {
            String newName = IOHelper.AskString("Enter replacement name:");
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new RenameFileLocalAction(Client.ftpClient, Client.localDirectory, Client.localSelection, newName);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file was successfully renamed to '" + newName + "'.");
                Client.localSelection = null;
            }

            return result;
        }
    }
}
