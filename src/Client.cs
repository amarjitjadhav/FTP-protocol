using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

public enum ClientState { VIEWING_LOCAL, VIEWING_REMOTE, COUNT };

public class Client
{
    public static ClientState state = ClientState.VIEWING_LOCAL;

    public static String serverName = null;
    public static String localDirectory = null;
    public static DFtpFile localSelection = null;
    public static String remoteDirectory = null;
    public static DFtpFile remoteSelection = null;
    public static FtpClient ftpClient = null;

    public static int windowWidth;
    public static int windowHeight;
       

}