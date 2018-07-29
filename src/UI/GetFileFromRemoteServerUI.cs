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
        public ConsoleKey Key => ConsoleKey.R;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Get File From (R)emote Server";


        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new GetFileFromRemoteServer(Client.ftpClient, Client.localDirectory, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            History.Log(result.ToString());

            return result;
        }
    }
}
