using Actions;
using System;
using System.Collections.Generic;
using System.Text;
using IO;

namespace DumbFTP.UI
{
    public class SearchFileRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.S;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "(S)earch file remote";

        public DFtpResult Go()
        {
            String pattern = IOHelper.AskString("What pattern to search for?");
            //Console.Write("\nWhat pattern to search for? : ");
            //String pattern = Console.ReadLine();

            //Console.Write("\nInclude subdirectories y/n ? : ");
            //ConsoleKeyInfo includeSubirectories = Console.ReadKey();

            //Console.WriteLine();
            bool includeSubdirectories = IOHelper.AskBool("Include subdirectories?", "yes", "no");

            // Create the action, Initialize it with the info we've collected | includeSubirectories.Key == ConsoleKey.Y
            DFtpAction action = new SearchFileRemoteAction(Client.ftpClient, pattern, Client.remoteDirectory, includeSubdirectories);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            History.Log(result.ToString());
            
            return result;
        }
    }
}
