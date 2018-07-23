﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IO
{
    public class ConsoleUI
    {
        private const String defaultColor = "\u001b[37m";
        static private int width;
        static private int height;
        static private List<List<char>> buffer;
        private static List<List<String>> colorBuffer;
        private static bool isANSISupported = true;
        static private bool colorEnabled = true;

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static void Initialize()
        {
            SetBackgroundColor(ConsoleColor.Black);

            width = Console.WindowWidth;
            height = Console.WindowHeight;
            Console.CursorVisible = false;

            buffer = new List<List<char>>();
            colorBuffer = new List<List<String>>();

            for (int x = 0; x < width; ++x)
            {
                List<char> bufferRow = new List<char>();
                List<String> colorBufferRow = new List<String>();

                for (int y = 0; y < height; ++y)
                {
                    bufferRow.Add(' ');
                    // Add some default color
                    colorBufferRow.Add(defaultColor);
                }
                buffer.Add(bufferRow);
                colorBuffer.Add(colorBufferRow);
            }

            bool isWindows = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                // This is code to enable ANSI Character support on windows taken from
                // https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e
                IntPtr iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                {
                    Console.WriteLine("WARNING: Failed to get output console mode");
                    isANSISupported = false;
                    Console.ReadKey();
                    return;
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(iStdOut, outConsoleMode))
                {
                    Console.WriteLine($"WARNING: Failed to set output console mode, error code: {GetLastError()}");
                    isANSISupported = false;
                    Console.ReadKey();
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// Clear the contents of the buffer string table. Reset the 
        /// screen contents with default draw box.
        /// </summary>
        public static void ClearBuffers()
        {
            ClearBuffers(Color.White);
            Console.Clear();
            return;
        }

        /// <summary>
        /// Clear the contents of the buffer string table. Reset the 
        /// screen contents with default draw box.
        /// </summary>
        public static void ClearBuffers(Color color)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    buffer[x][y] = ' ';
                    colorBuffer[x][y] = color.ToCode();
                }
            }
            Console.Clear();
            return;
        }

        /// <summary>
        /// Turns on the cursor and moves it to the x/y locations specified.
        /// </summary>
        /// <param name="x">Distance from left</param>
        /// <param name="y">Distance from bottom</param>
        public static void CursorTo(int x, int y)
        {
            if (x < 0)
                x = 0;
            if (x >= width)
                x = width - 1;
            if (y < 0)
                y = 0;
            if (y >= height)
                y = height - 1;
            Console.SetCursorPosition(x, height - 1 - y);
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Turns off the cursor.
        /// </summary>
        public static void CursorOff()
        {
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Draw everything from the buffer to the console.
        /// </summary>
        public static void Render()
        {
            for (int y = height - 1; y >= 0; --y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < width; ++x)
                {
                    if (colorEnabled && isANSISupported)
                    {
                        sb.Append(colorBuffer[x][y]);
                    }
                    sb.Append(buffer[x][y]);
                }
                Console.Write(sb.ToString());
            }
            Console.SetCursorPosition(0, 0);
        }

        public static void ToggleColor()
        {
            colorEnabled = colorEnabled ? false : true;
            return;
        }

        public static void Write(int x, int y, char output, Color color)
        {
            //don't do anything if we're off the screen
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            buffer[x][y] = output;
            colorBuffer[x][y] = color.ToCode();
            return;
        }

        public static void Write(int x, int y, String output, Color color)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                Write(x + i, y, output[i], color);
            }
        }

        public static void Write(int x, int y, List<String> lines, Color color)
        {
            for (int i = 0; i < lines.Count; ++i)
            {
                Write(x, y - i, lines[i], color);
            }
        }


        public static int MaxWidth() => width;

        public static int MaxHeight() => height;

        public static void WaitForAnyKey()
        {
            Console.WriteLine("Press [Any Key] to continue..");
            if (Console.KeyAvailable == true)
            {
                Console.ReadKey();
            }
            Console.ReadKey();
            return;
        }

        public static void SetBackgroundColor(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            return;
        }

        public static void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            return;
        }
    }
}