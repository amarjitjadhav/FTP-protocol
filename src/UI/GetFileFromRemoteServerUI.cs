using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class GetFileFromRemoteServerUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.G;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(G)et File From Remote Server";


        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new GetFileFromRemoteServerAction(Client.ftpClient, Client.localDirectory, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();
            
            // Give some feedback if successful
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file '" + Client.remoteSelection.GetName() + "' downloaded successfully.");
            }
            // Return the result after running.
            return result;
        }
    }
}
