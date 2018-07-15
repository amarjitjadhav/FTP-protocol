using System;
using System.Collections.Generic;
using System.Text;

public class DFtpResult
{
    public enum Result { Ok, Error };
    public Result type;
    public String message;
    
    public DFtpResult(Result type, String message = "")
    {
        this.type = type;
        this.message = message;
    }

    public Result Type()
    {
        return type;
    }
}
