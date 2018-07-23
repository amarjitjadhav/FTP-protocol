using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

namespace UI
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
            //String file = Path.GetTempFileName();

            // Local directory from which the file will be uploaded (maybe not needed)
            //String localDirectory = Path.GetDirectoryName(file);

            // Local file selected for upload
            //DFtpFile localSelection = new DFtpFile(file);

            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new PutFileAction(Client.ftpClient, Client.localDirectory, Client.localSelection, Client.remoteDirectory);

            // Carry out the action and get the result
            //DFtpResult result = ;
            //if (result.Type() == DFtpResult.Result.Ok)
            //    Console.WriteLine("Upload result: Ok");
            return action.Run();
        }
    }
}
