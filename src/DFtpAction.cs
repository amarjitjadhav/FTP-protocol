using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

public abstract class DFtpAction
{
    protected FtpClient ftpClient;  // handle to the current remote server connection
    protected String localDirectory;    // Where the user was on the client side
    protected List<DFtpFile> localFiles;    // list of one or more selected local files/directories
    protected String remoteDirectory;   // Where the user was on the server side
    protected List<DFtpFile> remoteFiles;   // list of one or more selected remote files/directories

    public DFtpAction(
        FtpClient ftpClient,
        String localDirectory,
        List<DFtpFile> localFiles,
        String remoteDirectory,
        List<DFtpFile> remoteFiles)
    {
        this.ftpClient = ftpClient;
        this.localDirectory = localDirectory;
        this.localFiles = localFiles;
        this.remoteDirectory = remoteDirectory;
        this.remoteFiles = remoteFiles;
    }

    public abstract DFtpResult Run();
}
