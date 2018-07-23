using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;
using IO;

namespace UI
{
    /// <summary>
    /// This class 
    /// </summary>
    public class Browser
    {
        public List<IDFtpUI> Actions { get; private set; } = new List<IDFtpUI>
        {
            new ContextSwitchUI(),
            new PutFileUI(),
            new SearchFileRemoteUI(),
            //new GetRemoteListingUI(),
            new DeleteFileRemoteUI(),
            new SelectRemoteUI(),
        };

        public Browser()
        {
        }
    
        public void DrawResultList(DFtpListResult list)
        {
            Console.WriteLine();
            foreach (DFtpFile file in list.Files)
            {
                Console.WriteLine(file.GetName() + " " + file.GetSize() + " " + (file.Type() == FluentFTP.FtpFileSystemObjectType.Directory? "dir" : "file"));
            }
            Console.WriteLine();

            ConsoleUI.WaitForAnyKey();
            return;
        }

        public void DrawActionsMenu()
        {
            StringBuilder actionText = new StringBuilder();
            actionText.Append("Actions ");
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
                actionText.Append(" | " + action.MenuText);
            }
            ConsoleUI.Write(1, 0, actionText.ToString(), Color.Green);
            ConsoleUI.Render();
            return;
        }
    }
}
