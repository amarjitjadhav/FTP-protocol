using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;

namespace UI
{
    public class SelectMultipleTestUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Z;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "[Z] = Select Multple Test";

        public DFtpResult Go()
        {
            {
                List<int> numbers = new List<int>();
                for (int i = 0; i < 10; ++i)
                    numbers.Add(i);
                List<int> selection = IOHelper.SelectMultiple<int>("This is a short message. Use arrow keys and space bar to select, enter to confirm.", numbers);

                StringBuilder sb = new StringBuilder();
                sb.Append("You selected: ");
                foreach (int i in selection)
                    sb.Append(" " + i);
                IOHelper.Message(sb.ToString());
            }
            {
                List<int> numbers = new List<int>();
                for (int i = 0; i < 50; ++i)
                    numbers.Add(i);
                List<int> selection = IOHelper.SelectMultiple<int>("This is a short message. Use arrow keys and space bar to select, enter to confirm.", numbers);

                StringBuilder sb = new StringBuilder();
                sb.Append("You selected: ");
                foreach (int i in selection)
                    sb.Append(" " + i);
                IOHelper.Message(sb.ToString());
            }
            {
                List<int> numbers = new List<int>();
                for (int i = 0; i < 50; ++i)
                    numbers.Add(i);
                List<int> selection = IOHelper.SelectMultiple<int>("This is going to be a really long line of text that needs to be broken up. This is going to be a really long line of text that needs to be broken up. This is going to be a really long line of text that needs to be broken up. Use arrow keys and spacebar to select, enter to confirm.", numbers);

                StringBuilder sb = new StringBuilder();
                sb.Append("You selected: ");
                foreach (int i in selection)
                    sb.Append(" " + i);
                IOHelper.Message(sb.ToString());
            }

            return new DFtpResult(DFtpResultType.Ok, "Selected file/dir '" + Client.remoteSelection + "'.");
        }
    }
}
