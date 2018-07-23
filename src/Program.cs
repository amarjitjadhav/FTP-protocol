using System;
using System.Collections.Generic;
using System.Net;
using FluentFTP;

using DumbFTP;
using IO;
using Actions;
using Experimental;
using DumbFTP.UI;

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

        
        // my test change comment
        bool running = false;

        while (Client.ftpClient == null)
        {
            running = Login.TryConnect();
        }

        Console.WriteLine("Connected!");
        Client.remoteDirectory = "/";

        Browser browser = new Browser();

        while (running)
        {
            ConsoleUI.ClearBuffers();
            browser.ListActions();
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.Escape)
            {
                // Exit program.
                break;
            }
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
                if (action.RequiresFile && (Client.localSelection == null || Client.localSelection.Type() != FtpFileSystemObjectType.File))
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

                // Run the action that was selected.
                DFtpResult result = action.Go();

                // If by running the action returned a list of file objects.
                if (result is DFtpListResult)
                {
                    browser.Load((DFtpListResult)result);
                    browser.Show();
                }
                else if (result.Type == DFtpResultType.Ok)
                {
                    // Cool, we did the action.
                    Console.WriteLine("Action completed successfully");
                }
            }


          
        }
        
    }
}