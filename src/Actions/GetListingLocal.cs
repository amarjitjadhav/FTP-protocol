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
                    List<DFtpFile> dFtpLocalListing = new List<DFtpFile>();
                    PopulateLocalList(Directory.GetFiles(localDirectory), ref dFtpLocalListing, true);
                    PopulateLocalList(Directory.GetDirectories(localDirectory), ref dFtpLocalListing, false);
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + localDirectory, dFtpLocalListing);
                }
                else
                {
                    return new DFtpResult(DFtpResultType.Error, "Directory does not exist");
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                return new DFtpResult(DFtpResultType.Error, "You do not have permission to access this directory. "+ ex.Message);
            }
        }

        private void PopulateLocalList(String[] result, ref List<DFtpFile> list, bool flag)
        {
            if(flag == true)
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
