using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using FluentFTP;
using DumbFTP;

public enum ClientState { VIEWING_LOCAL, VIEWING_REMOTE, COUNT };

public class Client
{
    public static ClientState state = ClientState.VIEWING_LOCAL;
    public static long idleTime = 0;

    public static String serverName = null;
    public static String localDirectory = null;
    public static DFtpFile localSelection = null;
    public static String remoteDirectory = null;
    public static DFtpFile remoteSelection = null;

    public static FtpClient ftpClient = null;

    public static int windowWidth;
    public static int windowHeight;
       
    /// <summary>
    /// Compare the remote selection with the local selection within the client state.
    /// </summary>
    /// <returns>True, if the remote selection and the local selection are different.</returns>
    public static bool AreFileSelectionsDifferent() 
    {
        if (ftpClient == null) // || !ftpClient.IsConnected)
        {
            throw new FtpException("FTP Client not found or is not connected");
        }
    
        // Open the remote file.
        Stream remoteStream = ftpClient.OpenRead(remoteSelection.GetFullPath());
        ftpClient.GetReply(); // to read the success/failure response from the server

        // Open the local file.
        using (Stream localStream = new FileStream(localSelection.GetFullPath(),
            FileMode.Open, FileAccess.Read))
        {

            // Convert the local file and remote file streams to bytes.
            byte[] remoteBytes = remoteStream.ReadBytes();
            byte[] localBytes = localStream.ReadBytes();

            remoteStream.Close();
            localStream.Close();
            
            
            return (remoteBytes != null && localBytes != null) ? !remoteBytes.SequenceEqual(localBytes) : false;
        }
    }
    
}