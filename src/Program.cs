using System;
using System.Collections.Generic;
using System.Net;
using FluentFTP;

using DumbFTP;
using IO;
using Actions;
using Experimental;
using UI;


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
        double dt = 0;
        ConsoleUI.ClearBuffers();

        while (running)
        {
            dt += Time.deltaMs;
            ConsoleKeyInfo input = new ConsoleKeyInfo();

            String clientContextState = Client.state == ClientState.VIEWING_LOCAL ? "Viewing Local" : "Viewing Remote";

            ConsoleUI.WriteLine("DumpFTP - " + clientContextState, Color.Gold);
            ConsoleUI.WriteLine(" Version - " + version, Color.Olive);

            browser.DrawActionsMenu();
            ConsoleUI.Render();

            while (!ConsoleUI.AnyKey())
            {
                Time.Update();
                dt += Time.deltaMs;
                ConsoleUI.Write(0, 0, dt.ToString() + " milliseconds have passed", Color.Salmon);
                input = ConsoleUI.ReadKey();
                ConsoleUI.Render();
            }

            
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
                    ConsoleUI.WriteLine("Action completed successfully", Color.Gold);
                }
            }
            

            ConsoleUI.Render();

            ConsoleUI.ClearBuffers();
            ConsoleUI.ResetKeyPress();
            Time.Update();
          
        }
        
    }
}