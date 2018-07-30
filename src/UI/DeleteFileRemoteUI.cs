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
        public ConsoleKey Key => ConsoleKey.R;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "Delete (R)emote File";


        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new DeleteFileRemoteAction(Client.ftpClient, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            History.Log(result.ToString());
                
            return result;
        }
    }
}
