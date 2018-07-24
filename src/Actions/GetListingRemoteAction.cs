using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class GetListingRemoteAction : DFtpAction
    {
        public GetListingRemoteAction(FtpClient ftpClient, String targetDirectory) : 
            base(ftpClient, null, null, targetDirectory, null)
        {
        }

        public override DFtpResult Run()
        {
            try
            {
                FtpListItem[] files = ftpClient.GetListing(remoteDirectory);
                return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + remoteDirectory, files);
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message); // FluentFTP didn't like something.
            }
        }
    }
}
