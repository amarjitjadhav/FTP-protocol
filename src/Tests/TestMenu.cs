using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;
using IO;

namespace Tests
{
    public class TestMenu
    {
        public TestMenu()
        {
            ConsoleUI.Initialize();
        }

        public void ListFiles(FtpClient client)
        {
            Color yellowPurple = new Color(255, 255, 0, 255, 0, 255); // Yellow on purple
            Color blackWhite = new Color(255, 255, 255); // White on black
            int position = Console.WindowHeight - 1;

            ConsoleUI.Clear();
            ConsoleUI.Write(0, position--, "  Dumb FTP v0.1  ", yellowPurple);
            ConsoleUI.Write(0, position--, "  Now in Goldenrod & Prince purple color scheme!", yellowPurple.Invert());
            ConsoleUI.Write(0, position--, "  .", blackWhite);
            ConsoleUI.Write(0, position--, "  ..", blackWhite);

            foreach (FtpListItem item in client.GetListing("/"))
            {
                long size = 0;
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    size = client.GetFileSize(item.FullName);
                }
                DateTime time = client.GetModifiedTime(item.Name);
                String name = item.FullName;

                // Draw name inverted if it is 'selected'
                if (position == Console.WindowHeight - 6)
                    ConsoleUI.Write(2, position, name, blackWhite.Invert());
                else
                    ConsoleUI.Write(2, position, name, blackWhite);

                // File size
                ConsoleUI.Write(16, position, size.ToString(), blackWhite);

                // File last modified
                ConsoleUI.Write(22, position, time.ToString(), blackWhite);

                --position;
            }
            List<String> text = new List<string>();
            text.Add("This is a mock-up window to test color and drawing functions.");
            text.Add("Press 'Enter' to exit.");
            ConsoleUI.Write(Console.WindowWidth / 2 - text[0].Length / 2, Console.WindowHeight / 2, text, blackWhite);
            ConsoleUI.Write(0, 0, "  [Space] = Select file/directory    [Enter] = Show commands, ", yellowPurple);
            ConsoleUI.Render();
        }

    }
}
