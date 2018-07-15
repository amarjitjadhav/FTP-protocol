using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

/// <summary>
/// A DFtpAction is any ftp command which can be carried out with a live ftp connection,
/// a local directory and file selection, and a remote directory and file selection.
/// This turns out to be basically everything you need to do on an ftp.
/// </summary>
public abstract class DFtpAction
{
    protected FtpClient ftpClient;  // A reference to the current remote server connection
    protected String localDirectory;    // Where the user was on the client side
    protected DFtpFile localSelection;  // The local file/directory selected when the action began
    protected String remoteDirectory;   // Where the user was on the server side
    protected DFtpFile remoteSelection; // The remote file/directory selected when the action began

    /// <summary>
    /// This must be called before running the action to provide it with the client and
    /// current selection information.
    /// </summary>
    /// <param name="ftpClient">An already connected FtpClient object</param>
    /// <param name="localDirectory">The directory the user was viewing client-side</param>
    /// <param name="localSelection">The file/directory the user had selected client-side</param>
    /// <param name="remoteDirectory">The directory the user was viewing server-side</param>
    /// <param name="remoteSelection">The file/directory the user had selected server-side</param>
    public void Init(
        FtpClient ftpClient,
        String localDirectory,
        DFtpFile localSelection,
        String remoteDirectory,
        DFtpFile remoteSelection)
    {
        this.ftpClient = ftpClient;
        this.localDirectory = localDirectory;
        this.localSelection = localSelection;
        this.remoteDirectory = remoteDirectory;
        this.remoteSelection = remoteSelection;
    }

    /// <summary>
    /// This abstract function must be implemented by classes inheriting from DFtpAction.
    /// It should complete the desired action and return a DFtpResult indicating success/failure.
    /// </summary>
    /// <returns>A DFtpResult with type indicating success/failure and an optional message</returns>
    public abstract DFtpResult Run();
}
