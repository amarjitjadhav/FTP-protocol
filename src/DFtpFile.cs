using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public enum DFtpFileType { Local, Remote };

public class DFtpFile
{
    protected String fullPath;
    protected String displayName;
    protected String modifiedDate;
    protected long size;

    protected FtpFileSystemObjectType fileType;

    protected String permissions;
    protected DFtpFileType localOrRemote = DFtpFileType.Local; 

    public DFtpFile(String fullPath, String displayName = null, String modifiedDate = "", long size = 0)
    {
        this.fullPath = fullPath;
        this.displayName = displayName;
        this.modifiedDate = modifiedDate;
        this.size = size;
        this.permissions = null;
    }

    public DFtpFile(FtpListItem file)
        : this(file.FullName, file.Name, file.Modified.ToString(), file.Size)
    {
        fileType = file.Type;
        permissions = file.RawPermissions;
        localOrRemote = DFtpFileType.Remote;
    }

    public String GetDirectory()
    {
        if (localOrRemote == DFtpFileType.Remote)
            return Path.GetDirectoryName(fullPath);
        else
            return Path.GetDirectoryName(fullPath);
    }

    public String GetFullPath() => fullPath;

    public String GetName()
    {
        if (displayName != null)
        { 
            return displayName;
        }
        String[] separatedName = fullPath.Split(Path.DirectorySeparatorChar);
        return separatedName[separatedName.Length - 1];
    }

    public String GetDate() => modifiedDate;

    public long GetSize() => size;

    public FtpFileSystemObjectType Type() => fileType;

    public override String ToString()
    {
        return GetName();
    }
}
