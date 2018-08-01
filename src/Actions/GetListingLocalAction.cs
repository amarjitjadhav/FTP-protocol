    using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using FluentFTP;

namespace Actions
{
    public class GetListingLocalAction : DFtpAction
    {
        public GetListingLocalAction(String targetDirectory) :
            base(null , targetDirectory, null, null , null)
        {
        }

        public override DFtpResult Run()
        {
            try
            {
                if (Directory.Exists(localDirectory))
                {
                    //Create an empty list of DftpFiles to store our file list
                    List<DFtpFile> dFtpLocalListing = new List<DFtpFile>();
                    //Grab all directories in the provided directory and store them in the list
                    PopulateLocalList(Directory.GetDirectories(localDirectory), ref dFtpLocalListing, false);
                    //Grab all files in the provided directory and store them in the list
                    PopulateLocalList(Directory.GetFiles(localDirectory), ref dFtpLocalListing, true);
                    //return the completed list
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + localDirectory, dFtpLocalListing);
                }
                else
                {
                    //if directory does not exist return a failure
                    return new DFtpResult(DFtpResultType.Error, "Directory does not exist");
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                return new DFtpResult(DFtpResultType.Error, "You do not have permission to access this directory. "+ ex.Message);
            }
        }

        private void PopulateLocalList(String[] result, ref List<DFtpFile> list, bool isfile)
        {
            if(isfile == true)
            {
                foreach (String item in result)
                {
                    list.Add(new DFtpFile((item), FtpFileSystemObjectType.File));
                }
            }
            else
            {
                foreach (String item in result)
                {
                    list.Add(new DFtpFile((localDirectory + item), FtpFileSystemObjectType.Directory));
                }
            }
        }
    }
}
