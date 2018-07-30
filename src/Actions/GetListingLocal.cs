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
            if (Directory.Exists(localDirectory))
            {
                List<DFtpFile> dFtpLocalListing = new List<DFtpFile>();
                PopulateLocalList(Directory.GetFiles(localDirectory),ref dFtpLocalListing);
                PopulateLocalList(Directory.GetDirectories(localDirectory), ref dFtpLocalListing);
                return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + localDirectory, dFtpLocalListing);
            }
            else
            {
                return new DFtpResult(DFtpResultType.Error, "Directory does not exist");
            }
        }

        private void PopulateLocalList(String[] result, ref List<DFtpFile> list)
        {
            foreach (String item in result)
            {
                list.Add(new DFtpFile((localDirectory + item)));
            }
        }
    }
}
