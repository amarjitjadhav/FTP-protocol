using System;
using Xunit;
using Actions;
using System.Collections.Generic;
using System.IO;

namespace XUnitTests
{
    public class DFTPResultTest
    {
        [Theory]
        [InlineData(DFtpResultType.Ok, "Okay")]
        [InlineData(DFtpResultType.Error, "Error")]
        public void NewDFtpResultContructorPublicMethodsReturnsMatchingParameters(DFtpResultType resultType, String msg)
        {
            DFtpResult result = new DFtpResult(resultType, msg);

            Assert.True(result.Type == resultType);
            Assert.True(result.Message == msg);
            return;
        }


    }
}
