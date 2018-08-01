using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class DeleteFileRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.D;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(D)elete Remote File";


        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new DeleteFileRemoteAction(Client.ftpClient, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            // Nullify the selection if successful.
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file '" + Client.remoteSelection.GetName() + "' was deleted successfully.");
                Client.remoteSelection = null;
            }
            
            return result;
        }
    }
}
