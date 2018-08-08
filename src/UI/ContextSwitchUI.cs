using System;
using System.Collections.Generic;
using System.Text;

namespace UI
{
    public class ContextSwitchUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Tab;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "[Tab] Switch between local/remote";

        public DFtpResult Go()
        {
            Client.state = Client.state == ClientState.VIEWING_LOCAL ? 
                ClientState.VIEWING_REMOTE :   // Now viewing remote directory.
                ClientState.VIEWING_LOCAL;     // Now viewing local directory.

            // If the client isnt connect to a remote, dont let them change states.
            Client.state = Client.ftpClient == null ? ClientState.VIEWING_LOCAL : Client.state;
            

            if (Client.state == ClientState.VIEWING_LOCAL)
            {
                // Viewing local. Do stuff.
            }
            else
            {
                // Viewing remote. Do stuff.
            }

            return new DFtpResult(DFtpResultType.Ok, "State changed to " + Client.state.ToString());
        }
    }
}
