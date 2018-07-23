using Xunit;
using Actions;
using System.Collections.Generic;
using System.IO;
using System;
using IO;

namespace XUnitTests
{
    public class HistoryTest
    {
        [Fact]
        public void LogHistoryFileContainsMessage()
        {
            String message = new String("my message");

            History.Log(message);
            History.Close();

            String readBack = File.ReadAllText(History.logFile);

            Assert.Contains(message, readBack);
            return;
        }
    }
}
