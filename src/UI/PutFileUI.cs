using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

namespace DumbFTP.UI
{
    public class PutFileUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.U;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(U)pload File";


        public DFtpResult Go()
        {
            // Create a temp file for upload and get its path
            String file = Path.GetTempFileName();

            // Local directory from which the file will be uploaded (maybe not needed)
            String localDirectory = Path.GetDirectoryName(file);

            // Local file selected for upload
            DFtpFile localSelection = new DFtpFile(file);

            // Upload destination
            String remoteDirectory = "/";

            // Remote file selection doesn't matter for upload
            DFtpFile remoteSelection = null;

            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new PutFile(Client.ftpClient, Client.localDirectory, Client.localSelection, Client.remoteDirectory, Client.remoteSelection);

            // Carry out the action and get the result
            //DFtpResult result = ;
            //if (result.Type() == DFtpResult.Result.Ok)
            //    Console.WriteLine("Upload result: Ok");
            return action.Run();
        }
    }
}
