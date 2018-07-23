using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;
using DumbFTP.UI;
using IO;

namespace DumbFTP
{
    /// <summary>
    /// This class 
    /// </summary>
    public class Browser
    {
        public List<IDFtpUI> Actions { get; private set; } = new List<IDFtpUI>
        {
            new PutFileUI(),
            new SearchFileRemoteUI(),
            new GetRemoteListingUI(),
            new DeleteFileRemoteUI(),
        };

        public Browser()
        {
        }
    
        public void DrawResultList(DFtpListResult list)
        {
            DrawActionsMenu();
            foreach (DFtpFile file in list.Files)
            {
                Console.WriteLine(file.GetName() + " " + file.GetSize() + " " + (file.Type() == FluentFTP.FtpFileSystemObjectType.Directory? "dir" : "file"));
            }
            
            ConsoleUI.WaitForAnyKey();
            return;
        }

        public void DrawActionsMenu()
        {
            Console.Write("Actions: ");
            foreach (IDFtpUI action in Actions)
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
            return;
        }
    }
}
