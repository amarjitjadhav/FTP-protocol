using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class LogOffRemoteServerUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.L;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "(L)og off";

        public bool HideForDirectory => false;

        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            //DFtpAction action = new LogOffRemoteServer(Client.ftpClient, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            // DFtpResult result = action.Run();

            Client.ftpClient = null;
            IOHelper.Message("You have logged of from '" + Client.serverName + "'.");
           
            return new DFtpResult(DFtpResultType.Ok, "User logged off");
        }
    }
}
