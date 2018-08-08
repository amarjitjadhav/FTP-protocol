using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    /// <summary>
    /// The user interaction class for attempting to upload a file to the remote server.
    /// </summary>
    public class PutFileUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.U;

        public bool RequiresLogin => true;

        public bool RequiresSelection => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(U)pload File";


        /// <summary>
        /// Attemps to upload a file from the client context to the remote server.
        /// </summary>
        /// <returns>A DftpResult object containing an OK or ERROR with a message of the 
        /// resulting operation.</returns>
        public DFtpResult Go()
        {
            // Create the action. Initialize it with the info we've collected
            DFtpAction action = new PutFileAction(Client.ftpClient, Client.localDirectory, Client.localSelection, Client.remoteDirectory);

            DFtpResult result = action.Run();

            // Give some feedback if successful
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file '" + Client.localSelection.GetName() + "' uploaded successfully.");
            }
            // Return the result after running.
            return result;
        }
    }
}
