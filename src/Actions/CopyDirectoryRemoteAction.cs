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
            //set paths for the directory we are copying and its new name
            if(remoteSelection == null)
            {
                return new DFtpResult(DFtpResultType.Error, "Please select a directory");
            }
            String oldPath = remoteDirectory + "/" + remoteSelection.GetName();
            String newPath = "";
            //If the entered name is an empty string append Copy- to the start of the directory name
            if (newName == "" || newName == remoteSelection.GetName())
            {
                newPath = remoteDirectory + "/" + remoteSelection.GetName() + "-copy";
                while (ftpClient.DirectoryExists(newPath))
                {
                    newPath = newPath + "-copy";
                }
            }
            else
            {
                //otherwise the name is valid and just set it as the new path
                newPath = remoteDirectory + "/" + newName;
            }
            try
            {
                //create the directory to copy into
                ftpClient.CreateDirectory(newPath);
                //get the listing for the original directory
                FtpListItem[] fluentListing = ftpClient.GetListing(oldPath, FtpListOption.AllFiles);
                //Create temporary directory to store the downloaded file
                Directory.CreateDirectory(localDirectory + "TEMPDOWNSLOADSDIR");
                String Localtemp_path = localDirectory + "TEMPDOWNSLOADSDIR";
                //Copy the original directory into the newly created directory
                Copy_recursive(fluentListing, newPath, Localtemp_path);
                //Delete the temp directory
                Directory.Delete(localDirectory + "TEMPDOWNSLOADSDIR");
                //if everything worked return Ok
                return new DFtpResult(DFtpResultType.Ok, "Created directory " + newPath);
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
        private void Copy_recursive(FtpListItem[] source, String path, String localPath)
        {  
            //Loop through the list of files in the original directory
            foreach (FtpListItem _file in source)
            {
                //If the file is a directory call this function recursively on said directory
                if(_file.Type == FtpFileSystemObjectType.Directory)
                {
                    String newPath = path + "/" + _file.Name;
                    ftpClient.CreateDirectory(newPath);
                    FtpListItem[] fluentListing = ftpClient.GetListing(_file.FullName, FtpListOption.AllFiles);
                    Copy_recursive(fluentListing, newPath,localPath);
                }
                //if the file type is file then download the file then reupload it in the new directory
                if(_file.Type == FtpFileSystemObjectType.File)
                {
                    //Check if the OS is windows
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        //Download the file from the original location
                        ftpClient.DownloadFile(localPath + @"\" + _file.Name, _file.FullName);
                        //Upload the file in the newly copied directory
                        ftpClient.UploadFile(localPath + @"\" + _file.Name, path + @"\" + _file.Name);
                        //Clean up the locally downloaded file
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
        }
    }
}