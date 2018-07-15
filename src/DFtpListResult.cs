using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A result that is typically returned from an action that returns files.
/// </summary>
public class DFtpListResult : DFtpResult
{
    public List<DFtpFile> Files { get; private set; } = new List<DFtpFile>();

    public DFtpListResult(Result type, String message, List<DFtpFile> files) : base(type, message)
    {
        this.type = type;
        this.message = message;
        this.Files = files;
    }

}
