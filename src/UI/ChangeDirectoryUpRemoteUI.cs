using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;
using System.Linq;

namespace UI
{
    public class ChangeDirectoryUpRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Backspace;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "[Backspace] = Go to parent directory";

        public DFtpResult Go()
        {
            String[] separated = Client.remoteDirectory.Split("/");
            String parent = "";
            for (int i = 1; i < separated.Length - 1; ++i)
            {
                parent = parent + "/" + separated[i];
            }
            if (parent == "")
                parent = "/";

            Client.remoteSelection = null;
            Client.remoteDirectory = parent;
            return new DFtpResult(DFtpResultType.Ok, "Changed to directory '" + Client.remoteDirectory + "'.");
        }
    }
}
