using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

/// <summary>
/// A result that is typically returned from an action that returns files.
/// </summary>
public class DFtpListResult : DFtpResult
{
    public List<DFtpFile> Files { get; private set; } = new List<DFtpFile>();

    public DFtpListResult(DFtpResultType type, String message, List<DFtpFile> files) : base(type, message)
    {
        this.Files = files;
    }

    public DFtpListResult(DFtpResultType type, String message, FtpListItem[] files) : base(type, message)
    {
        List<DFtpFile> list = new List<DFtpFile>();
        foreach (FtpListItem item in files)
        {
            DFtpFile file = new DFtpFile(item);
            list.Add(file);
        }
        this.Files = list;
    }
}
