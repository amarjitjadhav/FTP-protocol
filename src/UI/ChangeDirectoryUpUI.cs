using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Actions;

using FluentFTP;
using IO;
using System.Linq;

namespace UI
{
    public class ChangeDirectoryUpUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Backspace;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "[Backspace] = Go to parent directory";

        public DFtpResult Go()
        {
            String parent = "";
            if(Client.state == ClientState.VIEWING_LOCAL)
            {
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    String[] separated = Client.localDirectory.Split(@"\");
                    parent = parent + separated[0];
                    if(separated.Length == 1)
                    {
                        parent = parent + @"\";
                    }
                    for (int i = 1; i < separated.Length - 1; ++i)
                    {
                        parent = parent + @"\" + separated[i];
                    }
                    Client.localSelection = null;
                    Client.localDirectory = parent;
                    return new DFtpResult(DFtpResultType.Ok, "Changed local directory to: '" + Client.localDirectory + "'.");
                }
                else
                {
                    String[] separated = Client.localDirectory.Split("/");
                    for (int i = 1; i < separated.Length - 1; ++i)
                    {
                        parent = parent + "/" + separated[i];
                    }
                    if (parent == "")
                    {
                        parent = "/";
                    }
                    Client.localSelection = null;
                    Client.localDirectory = parent;
                    return new DFtpResult(DFtpResultType.Ok, "Changed local directory to: '" + Client.localDirectory + "'.");
                }
            }
            else if(Client.state == ClientState.VIEWING_REMOTE)
            {
                String[] separated = Client.remoteDirectory.Split("/");
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
            else
            {
                return new DFtpResult(DFtpResultType.Error, "Error on changing directory.");
            }
        }
    }
}
