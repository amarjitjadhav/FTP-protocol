using System.IO;
using IO;
using System;

namespace DumbFTP
{
    interface IDFtpUI
    {
        //protected bool isMocking;
        ConsoleKey Key { get; }
        bool RequiresLogin { get; }
        bool HideForDirectory { get; }
        bool HideForFile { get; }
        bool HideForLocal { get; }
        bool HideForRemote { get; }
        String MenuText { get; }

        DFtpResult Go();
    }
}