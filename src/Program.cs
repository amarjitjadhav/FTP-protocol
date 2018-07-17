using System;
using System.Collections.Generic;
using System.Net;
using FluentFTP;

using DumbFTP;
using IO;
using Actions;
using Experimental;

class Program
{
    static void Main(string[] args)
    {
        List<IDFtpUI> actions = new List<IDFtpUI>
        {
            new PutFileUI(),
            new SearchFileRemoteUI(),
            new GetRemoteListingUI(),
        };
        
        bool running = false;

        while (Client.ftpClient == null)
        {
            running = Login.TryConnect();
        }

        Console.WriteLine("Connected!");
        //state = ClientState.PUTFILE;
        //running = delegates[(int)state].Invoke();

        while (running)
        {
            ConsoleKeyInfo input = Console.ReadKey();
  
            foreach (IDFtpUI action in actions)
            {
                if (action.Key != input.Key)
                {
                    continue;
                }
                if (action.RequiresLogin && Client.ftpClient == null)
                {
                    // DO LOGIN.???
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

                DFtpResult result = action.Go();
                if (result is DFtpListResult)
                {
                    // Run Browser on list result.
                }

            }
          
        }
        
    }
}