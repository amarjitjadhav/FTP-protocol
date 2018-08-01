using Actions;
using System;
using System.Collections.Generic;
using System.Text;
using IO;

namespace UI
{
    public class CreateDirectoryRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.C;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(C)reate directory";

        public DFtpResult Go()
        {
            String name = IOHelper.AskString("Enter new directory name.");

            // Create the action, Initialize it with the info we've collected
            DFtpAction action = new CreateDirectoryRemoteAction(Client.ftpClient, Client.remoteDirectory + "/" + name);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            // Give some feedback if successful
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The directory '" + name + "' was created successfully.");
            }
            // Return the result after running.

            return result;
        }
    }
}
