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
    public static String version = "1.0alpha";

    static void Main(string[] args)
    {
        ConsoleUI.Initialize();
        bool running = false;

        while (Client.ftpClient == null)
        {
            running = Login.TryConnect();
            if (running == false)
            {
                Console.WriteLine("Try again.");
            }
        }

        Console.WriteLine("Connected!");
        Client.remoteDirectory = "/";

        Browser browser = new Browser();

        while (running)
        {

            ConsoleUI.ClearBuffers();
            String clientContextState = Client.state == ClientState.VIEWING_LOCAL ? "Viewing Local" : "Viewing Remote";

            ConsoleUI.Write(0, ConsoleUI.MaxHeight() - 1, "DumpFTP - " + clientContextState, Color.Gold);
            ConsoleUI.Write(0, ConsoleUI.MaxHeight() - 2, " Version - " + version, Color.Olive);

            ConsoleUI.Render();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            browser.DrawActionsMenu();
            ConsoleKeyInfo input = Console.ReadKey();

            if (input.Key == ConsoleKey.Escape)
            {
                // Exit program.
                break;
            }
            foreach (IDFtpUI action in browser.Actions)
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
                    browser.DrawResultList((DFtpListResult)result);
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