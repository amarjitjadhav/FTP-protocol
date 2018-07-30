using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;
using System.Linq;

namespace UI
{
    public class ChangeDirectoryDownRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Enter;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "[Enter] = Change directory";

        public DFtpResult Go()
        {
            // Get listing for current directory
            DFtpAction getListingAction = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
            DFtpResult tempResult = getListingAction.Run();
            DFtpListResult listResult = null;
            if (tempResult is DFtpListResult)
            {
                listResult = (DFtpListResult)tempResult;
                List<DFtpFile> list = listResult.Files;
                // Filter out files
                list = list.Where(x => x.Type() == FtpFileSystemObjectType.Directory).ToList();

                // Choose from directories
                DFtpFile selection = IOHelper.Select<DFtpFile>("Choose a directory to enter.", list);

                // If something has been selected, update the remote selection
                if (selection != null)
                {
                    Client.remoteSelection = null;
                    Client.remoteDirectory = selection.GetFullPath();
                    return new DFtpResult(DFtpResultType.Ok, "Changed to directory '" + Client.remoteDirectory + "'.");
                }
            }
            return tempResult;
        }
    }
}
