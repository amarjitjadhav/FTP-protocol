using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DumbFTP
{
    /// <summary>
    /// Extends the methods of the C# Stream class. The source code for this file is an example from
    /// the MSDN. https://msdn.microsoft.com/en-us/library/system.io.filestream.read(v=vs.110).aspx
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// Read bytes from a Stream source and return the array of bytes.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The array of bytes found in the stream.</returns>
        public static byte[] ReadBytes(this Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            // Read the source file into a byte array.
            byte[] bytes = new byte[stream.Length];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;

            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                { 
                    break;
                }

                numBytesRead += n;
                numBytesToRead -= n;
            }

            return bytes;
        }

    }
}
