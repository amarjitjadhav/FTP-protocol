using System;
using System.Collections.Generic;
using System.Net;
using FluentFTP;

using DumbFTP;
using IO;
using Actions;
using UI;


class Program
{
    public static String version = "1.0alpha";
    public const double allowedIdleTime = 120.0f;
    private static int width;
    private static int height;

    public static void LoginLoop(bool fromIdle)
    {
        Console.Clear();

        if (fromIdle)
        {
            Console.WriteLine("You were timed out for being idle too long");
        }

        Client.ftpClient = null;

        while (Client.ftpClient == null)
        {
            bool success = Login.TryConnect();
            if (success == false)
            {
                Console.WriteLine("Try again.");
            }
        }

        Client.localDirectory = null;
        Client.remoteDirectory = "/";
        Client.remoteSelection = null;
        Client.localSelection = null;

        Console.WriteLine("Connected!");

        return;
    }

    static void Main(string[] args)
    {
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        ConsoleUI.Initialize();
        

        LoginLoop(false);


        Browser browser = new Browser();
        long idleTime = 0;
        ConsoleUI.ClearBuffers();

        bool running = true;
        while (running)
        {
            Program.CheckForResize();
            idleTime += Time.deltaMs;
            ConsoleKeyInfo input = new ConsoleKeyInfo();

            String clientContextState = Client.state == ClientState.VIEWING_LOCAL ? "Viewing Local" : "Viewing Remote";

            ConsoleUI.WriteLine("DumbFTP v" + version, Color.Gold);
            ConsoleUI.WriteLineWrapped("You are currently connected to '" + Client.serverName + "'. Below are the current working directories and any selected files on both the client (local) and the server (remote). The view can be switched by pressing [tab]. Actions which can be performe will appear below. Please note that some actions may only be carried out after a selection has been made.", Color.White, 2);
            browser.DrawClientInfo();
            browser.DrawActionsMenu();
            browser.DrawListing();
            ConsoleUI.Render();

            while (!ConsoleUI.AnyKey())
            {
                Time.Update();
                idleTime += Time.deltaMs;
                ConsoleUI.Write(0, 0, "                                ", Color.White);
                ConsoleUI.Write(0, 0, "Idle for " + Time.MillisecondsToSeconds(idleTime).ToString() + " seconds", Color.Salmon);
                input = ConsoleUI.ReadKey();
                ConsoleUI.Render();
                if (Time.MillisecondsToSeconds(idleTime) >= allowedIdleTime)
                {
                    // Exit program.
                    LoginLoop(true);
                    break;
                }
            }
            

            idleTime = 0;

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

                if (action.RequiresFile && Client.state == ClientState.VIEWING_LOCAL && (Client.localSelection == null || Client.localSelection.Type() != FtpFileSystemObjectType.File))
                {
                    continue;
                }
                if (action.RequiresFile && Client.state == ClientState.VIEWING_REMOTE &&  (Client.remoteSelection == null || Client.remoteSelection.Type() != FtpFileSystemObjectType.File))
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
                    // ConsoleUI.WriteLine("Action completed successfully", Color.Gold); //comment out for now
                }
            }
            


            ConsoleUI.Render();

            ConsoleUI.ClearBuffers();
            ConsoleUI.ResetKeyPress();
            //Time.Update();
          
        } 
    }

    private static void CheckForResize()
    {
        if (Program.width != Console.WindowWidth || Program.height != Console.WindowHeight)
        {
            Program.width = Console.WindowWidth;
            Program.height = Console.WindowHeight;
            ConsoleUI.Initialize();
        }
    }
}