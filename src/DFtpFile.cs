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

    protected FtpFileSystemObjectType fileType;

    protected String permissions;
    protected bool remote; 

    public DFtpFile(String fullPath, FtpFileSystemObjectType type, String displayName = null, String modifiedDate = "", long size = 0)
    {
        this.fullPath = fullPath;
        this.displayName = displayName;
        this.modifiedDate = modifiedDate;
        this.size = size;
        this.fileType = type;
        this.permissions = null;
        this.remote = false;
    }



    public DFtpFile(FtpListItem file)
        : this(file.FullName,file.Type, file.Name, file.Modified.ToString(), file.Size)
    {
        fileType = file.Type;
        permissions = file.RawPermissions;
        remote = true;
    }

    /// <summary>
    /// Returns the full path of the parent directory of this file/directory.
    /// </summary>
    /// <returns>String representing the full path of the contining </returns>
    public String GetParentDirectory()
    {
        String dir = Path.GetDirectoryName(fullPath);
        if (remote)
            dir.Replace(@"\", "/");
        return dir;
    }

    public String GetFullPath() {
        if (remote)
            return fullPath.Replace(@"\", "/");
        return fullPath;
    }

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
        return this.fileType + " : " + GetName();
    }
}
