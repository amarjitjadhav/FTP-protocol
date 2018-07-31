using System;
using System.Collections.Generic;
using System.Text;


public enum DFtpResultType { Ok, Error };

public class DFtpResult
{
    public DFtpResultType Type { get; protected set; }
    public String Message { get; protected set; } = "";
    public static object Result { get; internal set; }

    public DFtpResult(DFtpResultType type, String message = "")
    {
        this.Type = type;
        this.Message = message;
    }

    public override string ToString() => Message + " => " + Type.ToString();

}
