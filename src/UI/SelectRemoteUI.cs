using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;

namespace UI
{
    public class SelectRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.S;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(S)elect file/directory";

        public DFtpResult Go()
        {
            // Get listing for remote directory
            DFtpAction getListingAction = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
            DFtpResult tempResult = getListingAction.Run();
            DFtpListResult listResult = null;
            if (tempResult is DFtpListResult)
            {
                listResult = (DFtpListResult)tempResult;

                // Choose from files
                DFtpFile selected = IOHelper.Select<DFtpFile>("Choose a remote file to select.", listResult.Files, true);
            
                // If something has been selected, update the remote selection
                if (selected != null)
                {
                    Client.remoteSelection = selected;
                    return new DFtpResult(DFtpResultType.Ok, "Selected file/dir '" + Client.remoteSelection + "'.");
                }
            }
            return tempResult;
        }
    }
}
