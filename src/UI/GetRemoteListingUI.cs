using FluentFTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace DumbFTP.UI
{
    /*
    public class GetRemoteListingUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Tab;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "[Tab] View remote listing";

        public DFtpResult Go()
        {

            List<DFtpFile> files = new List<DFtpFile>();

            
            String directoryToView = "/";
            if (Client.remoteSelection != null)
            {
                directoryToView = Client.remoteDirectory;
            }

            FtpListItem[] list = Client.ftpClient.GetListing(directoryToView);

            foreach (FtpListItem item in list)
            {
                files.Add(new DFtpFile(item));
            }

            return new DFtpListResult(DFtpResultType.Ok, "Directory listing for: " + directoryToView, files);
        }
    }
    */
}
