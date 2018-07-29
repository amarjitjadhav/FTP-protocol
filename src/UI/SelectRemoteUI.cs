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
        public ConsoleKey Key => ConsoleKey.E;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "S(e)lect file/directory";

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
                DFtpFile selected = IOHelper.Select<DFtpFile>("Choose a remote file to select.", listResult.Files);

                if (selected != null)
                {
                    // if it's a directory, set the client's remote directory, otherwise set selected file.
                    if (selected.Type() == FtpFileSystemObjectType.Directory)
                    {
                        Client.remoteDirectory = selected.GetFullPath();
                        Client.remoteSelection = null;
                    }
                    // Otherwise set the client's remote selection
                    else
                    {
                        Client.remoteSelection = selected;
                    }
                }

                return new DFtpResult(DFtpResultType.Ok, "Listed files in remote directory: " + Client.remoteDirectory);
            }
            else
                return tempResult;
        }
    }
}
