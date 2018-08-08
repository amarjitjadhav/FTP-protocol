using System.IO;
using IO;
using System;

namespace UI
{
    public interface IDFtpUI
    {
        //protected bool isMocking;
        ConsoleKey Key { get; }
        bool RequiresLogin { get; }
        bool RequiresSelection { get; }
        bool HideForDirectory { get; }
        bool HideForFile { get; }
        bool HideForLocal { get; }
        bool HideForRemote { get; }
        String MenuText { get; }

        DFtpResult Go();
    }
}