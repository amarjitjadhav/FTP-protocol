using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DFtpFile
{
    protected String fullPath;
    protected String displayName;
    protected String modifiedDate;
    protected long size;
    protected String permissions;

    public DFtpFile(String fullPath, String displayName = null, String modifiedDate = "", long size = 0)
    {
        this.fullPath = fullPath;
        this.displayName = displayName;
        this.modifiedDate = modifiedDate;
        this.size = size;
        this.permissions = null;
    }

    public DFtpFile(FtpListItem item)
    {
        this.fullPath = item.FullName;
        this.displayName = item.Name;
        this.modifiedDate = item.Modified.ToString();
        this.size = item.Size;
        this.permissions = item.RawPermissions;
    }

    public String GetFullPath()
    {
        return fullPath;
    }

    public String GetName()
    {
        if (displayName != null)
            return displayName;
        String[] separatedName = fullPath.Split(Path.DirectorySeparatorChar);
        return separatedName[separatedName.Length - 1];
    }

    public String GetDate()
    {
        return modifiedDate;
    }

    public long GetSize()
    {
        return size;
    }

}
