using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class History
    {
        private static StreamWriter writer = null;
        public static readonly String logFile = "History.txt";

        public static void Log(string msg)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToString(), " : ", msg));
            return;
        }

        private static void WriteToLog(string message)
        {
            if (writer == null) { CreateHistoryFile(); }

            writer?.WriteLine(message);
            return;
        }
        
        public static void Close()
        {
            writer.Close();
            writer = null;
            return;
        }

        private static void CreateHistoryFile()
        {
            File.WriteAllText(logFile, String.Empty);
            writer = new StreamWriter(File.Open(logFile, System.IO.FileMode.Create));
            if (writer != null)
            {
                writer.AutoFlush = true;
            }
            return;
        }

    }
}