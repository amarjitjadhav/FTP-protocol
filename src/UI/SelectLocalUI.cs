using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;

namespace UI
{
    public class SelectLocalUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.S;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(S)elect local file/directory";

        public DFtpResult Go()
        {
            // Get listing for remote directory
            DFtpAction getListingAction = new GetListingLocalAction(Client.localDirectory);
            DFtpResult tempResult = getListingAction.Run();
            DFtpListResult listResult = null;
            if (tempResult is DFtpListResult)
            {
                listResult = (DFtpListResult)tempResult;
                DFtpFile selected = IOHelper.Select<DFtpFile>("Choose a local file to select.", listResult.Files, true);
                // If something has been selected, update the remote selection
                if (selected != null)
                {
                    Client.localSelection = selected;
                    return new DFtpResult(DFtpResultType.Ok, "Selected file/dir '" + Client.remoteSelection + "'.");
                }
            }
            return tempResult;
        }
    }
}
