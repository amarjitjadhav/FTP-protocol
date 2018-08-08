using System;
using System.Collections.Generic;
using System.Text;
using Actions;
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
            new ChangeDirectoryDownUI(),
            new ChangeDirectoryUpUI(),
            new CreateDirectoryRemoteUI(),
            new PutFileUI(),
            new SearchFileRemoteUI(),
            new DeleteFileRemoteUI(),
            new SelectRemoteUI(),
            new GetFileFromRemoteServerUI(),
            new LogOffRemoteServerUI(),
            new RenameFileRemoteActionUI(),
            new RenameFileLocalActionUI(),
            new SelectMultipleTestUI(),
            new CopyDirectoryUI(),
            new SelectLocalUI(),
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

            return;
        }

        public void DrawClientInfo()
        {
            String localSelectionText = "";
            if (Client.localSelection != null)
            { 
                localSelectionText = Client.localSelection.GetName();
            }
            String remoteSelectionText = "";
            if (Client.remoteSelection != null)
            { 
                remoteSelectionText = Client.remoteSelection.GetName();
            }

            ConsoleUI.WriteLine("Client info", Color.Gold);
            ConsoleUI.WriteLine("Local working directory:  " + Client.localDirectory, Color.White, 2);
            ConsoleUI.WriteLine("Local selected file/dir:  " + localSelectionText, Color.White, 2);
            ConsoleUI.WriteLine("Remote working directory: " + Client.remoteDirectory, Color.White, 2);
            ConsoleUI.WriteLine("Remote selected file/dir: " + remoteSelectionText, Color.White, 2);
            return;
        }

        public void DrawActionsMenu()
        {
            ConsoleUI.WriteLine("Actions: ", Color.Gold);
            foreach (IDFtpUI action in Actions)
            {
                if (action.RequiresLogin && Client.ftpClient == null)
                {
                    // DO LOGIN.???
                    continue;
                }
                if (Client.state == ClientState.VIEWING_LOCAL && action.RequiresSelection && ( Client.localSelection == null || Client.localSelection.Type() == FtpFileSystemObjectType.Link) )
                {
                    continue;
                }
                if (Client.state == ClientState.VIEWING_REMOTE && action.RequiresSelection && (Client.remoteSelection == null || Client.remoteSelection.Type() == FtpFileSystemObjectType.Link))
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
                if (action.HideForDirectory && Client.remoteSelection != null && Client.remoteSelection.Type() == FtpFileSystemObjectType.Directory)
                {
                    continue;
                }
                if (action.HideForFile && Client.remoteSelection != null && Client.remoteSelection.Type() == FtpFileSystemObjectType.File)
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
                if(action.RequiresSelection && Client.remoteSelection == null && Client.state == ClientState.VIEWING_REMOTE)
                {
                    continue;
                }
                ConsoleUI.WriteLine(action.MenuText, Color.Olive, 2);
            }
            return;
        }

        public void DrawListing()
        {
            DFtpResult result = null;
            DFtpAction action = null;
            if (Client.state == ClientState.VIEWING_LOCAL)
            {
                ConsoleUI.WriteLine("Listing for: " + Client.localDirectory, Color.Gold);
                action = new GetListingLocalAction(Client.localDirectory);
            }
            else if (Client.state == ClientState.VIEWING_REMOTE)
            {
                ConsoleUI.WriteLine("Listing for: " + Client.remoteDirectory, Color.Gold);
                action = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
            }
            result = action.Run();

            History.Log(result.ToString());

            if (result is DFtpListResult)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                foreach (DFtpFile file in listResult.Files)
                {
                    if(file.Type() == FtpFileSystemObjectType.File)
                    {
                        ConsoleUI.WriteLine((file.ToString()), Color.Green);
                    }
                    else if(file.Type() == FtpFileSystemObjectType.Directory)
                    {
                        ConsoleUI.WriteLine((file.ToString()), Color.Orange);
                    }
                
                }
            }
        }
    }
}
