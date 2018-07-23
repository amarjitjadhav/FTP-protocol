using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;
using DumbFTP.UI;

namespace DumbFTP
{
    /// <summary>
    /// This class 
    /// </summary>
    public class Browser
    {
        private DFtpListResult list = null;
        private List<IDFtpUI> actions = null;

        public Browser()
        {
            actions = new List<IDFtpUI>
            {
                new PutFileUI(),
                new SearchFileRemoteUI(),
                new GetRemoteListingUI(),
            };
        }

        public void Load(DFtpListResult list)
        {
            this.list = list;
        }

        public void Show()
        {
            ListActions();
            foreach (DFtpFile file in list.Files)
            {
                Console.WriteLine(file.GetName() + " " + file.GetSize() + " " + (file.Type() == FluentFTP.FtpFileSystemObjectType.Directory? "dir" : "file"));
            }
        }

        public void ListActions()
        {
            Console.Write("Actions: ");
            foreach (IDFtpUI action in actions)
            {
                if (action.RequiresLogin && Client.ftpClient == null)
                {
                    // DO LOGIN.???
                    continue;
                }
                if (action.RequiresFile && ( Client.localSelection == null || Client.localSelection.Type() != FtpFileSystemObjectType.File) )
                {
                    continue;
                }
                if (action.HideForDirectory && Client.localSelection != null && Client.localSelection.Type() == FtpFileSystemObjectType.Directory)
                {
                    continue;
                }
                if (action.HideForFile && Client.localSelection != null && Client.localSelection.Type() == FtpFileSystemObjectType.File)
                {
                    continue;
                }
                if (action.HideForLocal && Client.state == ClientState.VIEWING_LOCAL)
                {
                    continue;
                }
                if (action.HideForRemote && Client.state == ClientState.VIEWING_REMOTE)
                {
                    continue;
                }
                Console.Write(action.MenuText + "  ");
            }
            Console.WriteLine();
        }
    }
}
