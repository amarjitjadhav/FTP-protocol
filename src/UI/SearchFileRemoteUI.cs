using Actions;
using System;
using System.Collections.Generic;
using System.Text;
using IO;

namespace UI
{
    public class SearchFileRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.S;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(S)earch file remote";

        public DFtpResult Go()
        {
            String pattern = IOHelper.AskString("What pattern to search for?");
            bool includeSubdirectories = IOHelper.AskBool("Include subdirectories?", "yes", "no");

            // Create the action, Initialize it with the info we've collected
            DFtpAction action = new SearchFileRemoteAction(Client.ftpClient, pattern, Client.remoteDirectory, includeSubdirectories);

            // Carry out the action and get the result
            DFtpResult result = action.Run();
            
            return result;
        }
    }
}
