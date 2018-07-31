using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DFtpFile : IComparable
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

    public void SetName(String name)
    {
        this.displayName = name;
    }

    public String GetDate() => modifiedDate;

    public long GetSize() => size;

    public FtpFileSystemObjectType Type() => fileType;

    public string Get_Type()
    {
        return fileType.ToString();
    }
    public override String ToString()
    {
        return Get_Type() + " : " + GetName();
        //return GetName();
    }

    public int CompareTo(DFtpFile other)
    {
        if (this.Type() == FtpFileSystemObjectType.Directory && other.Type() == FtpFileSystemObjectType.File)
        {
            return -1;
        }
        else if (this.Type() == FtpFileSystemObjectType.File && other.Type() == FtpFileSystemObjectType.Directory)
        {
            return 1;
        }
        else
        {
            return String.Compare(this.GetName(), other.GetName());
        }
    }

    public int CompareTo(object obj)
    {
        if (obj is DFtpFile)
            return this.CompareTo((DFtpFile)obj);
        else
            throw new Exception();
    }

}
