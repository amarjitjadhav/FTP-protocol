using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that copies a directory on the ftp server.
    /// </summary>
    public class CopyDirectoryRemoteAction : DFtpAction
    {
        protected String newName;

        /// <summary>
        /// Constructor to build an action that copies a directory on the server
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="remoteDirectory">The remote directory path where the file to be renamed resides.</param>
        /// <param name="remoteSelection">The file to rename</param>
        /// <param name="localDirectory">"Current local directory for temp stoage"</param>
        /// <param name="newName">"New directory name"</param>
        /// 
        public CopyDirectoryRemoteAction(FtpClient ftpClient, String localDirectory, String remoteDirectory, DFtpFile remoteSelection, String newName)
            : base(ftpClient, localDirectory, null, remoteDirectory, remoteSelection)
        {
            this.newName = newName;
        }

        /// <summary>
        /// Attempt to copy a directory on a remote ftp server
        ///
        /// </summary>
        /// <returns>DftpResultType.Ok, if the directory was copied.</returns>
        /// 

        public override DFtpResult Run()
        {
            String oldPath = remoteDirectory + "/" + remoteSelection.GetName();
            String newPath = remoteDirectory + "/" + newName;

            try
            {
                ftpClient.CreateDirectory(newPath);
                FtpListItem[] fluentListing = ftpClient.GetListing(oldPath, FtpListOption.AllFiles);
                Directory.CreateDirectory(localDirectory + "TEMPDOWNSLOADSDIR");
                String Localtemp_path = localDirectory + "TEMPDOWNSLOADSDIR";
                Copy_recursive(fluentListing, newPath, Localtemp_path);
                Directory.Delete(localDirectory + "TEMPDOWNSLOADSDIR");
                return new DFtpResult(DFtpResultType.Ok, "Created directory " + newPath);
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "File with path \"" + oldPath + "\" " +
                    "could not be moved to \"" + newPath + "\" on remote." + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
        private void Copy_recursive(FtpListItem[] source, String path, String localPath)
        {
           // Directory.CreateDirectory(localDirectory + @"\TEMPDOWNSLOADSDIR");
           
            foreach (FtpListItem _file in source)
            {
                if(_file.Type == FtpFileSystemObjectType.Directory)
                {
                    String newPath = path + "/" + _file.Name;
                    ftpClient.CreateDirectory(newPath);
                    FtpListItem[] fluentListing = ftpClient.GetListing(_file.FullName, FtpListOption.AllFiles);
                    Copy_recursive(fluentListing, newPath,localPath);
                }
                if(_file.Type == FtpFileSystemObjectType.File)
                {
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        ftpClient.DownloadFile(localPath + @"\" + _file.Name, _file.FullName);
                        ftpClient.UploadFile(localPath + @"\" + _file.Name, path + @"\" + _file.Name);
                        File.Delete(localPath + @"\" + _file.Name);
                    }
                    else
                    {
                        ftpClient.DownloadFile(localPath + "/" + _file.Name, _file.FullName);
                        ftpClient.UploadFile(localPath + "/" + _file.Name, path + "/" + _file.Name);
                        File.Delete(localPath + @"\" + _file.Name);
                    }
                }
            }
            //Directory.Delete(localDirectory + @"\TEMPDOWNSLOADSDIR");
        }
    }
}