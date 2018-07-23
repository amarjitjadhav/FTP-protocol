using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IO
{
    /// <summary>
    /// This class contains helpful functions for prompting the user for various types of values.
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// Prompts the user to enter a string. An empty response is valid.
        /// </summary>
        /// <param name="display">This text will be displayed to the user, usually a question.</param>
        /// <returns>Returns the string the user typed.</returns>
        public static String AskString(String display)
        {
            ConsoleUI.Initialize();
            ConsoleUI.ClearBuffers();
            int displayLength = display.Length;
            int x = Console.WindowWidth / 2 - displayLength / 2;
            int y = Console.WindowHeight / 2 + 1;
            ConsoleUI.Write(x, y, display, Color.White);
            ConsoleUI.Render();
            ConsoleUI.CursorTo(x, y - 2);
            String result = Console.ReadLine();
            ConsoleUI.CursorOff();
            return result;
        }

        /// <summary>
        /// This prompts the user to choose between a yes/no or ok/cancel type resopnse (modal).
        /// </summary>
        /// <param name="question">The text to display to the user. Usualy a yes/no question.</param>
        /// <param name="trueText">The text to display for the true response (usually yes/ok)</param>
        /// <param name="falseText">The texto display for the false response (usually no/cancel)</param>
        /// <returns></returns>
        public static bool AskBool(String question, String trueText, String falseText)
        {
            bool selected = true;
            int displayLength = question.Length;
            int x = Console.WindowWidth / 2 - displayLength / 2;
            int y = Console.WindowHeight / 2 + 1;

            ConsoleUI.Initialize();
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            do
            {
                ConsoleUI.ClearBuffers();
                ConsoleUI.Write(x, y, question, Color.White);
                ConsoleUI.Write(x, y - 2, trueText, selected ? Color.Gold.Invert() : Color.Gold);
                ConsoleUI.Write(x + trueText.Length + 2, y - 2, falseText, selected ? Color.Gold : Color.Gold.Invert());
                ConsoleUI.Render();
                input = Console.ReadKey();
                if (selected)
                {
                    if (input.Key == ConsoleKey.RightArrow)
                        selected = false;
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                    selected = true;
            } while (input.Key != ConsoleKey.Enter);
            
            return selected;
        }

        /// <summary>
        /// This prompts the user to enter a valid integer. It will continue to prompt until 
        /// a valid integer is entered.
        /// </summary>
        /// <param name="display">The text to be displayed to the user.</param>
        /// <returns>Returns a valid integer value.</returns>
        public static int AskInt(String display)
        {
            int displayLength = display.Length;
            int x = Console.WindowWidth / 2 - displayLength / 2;
            int y = Console.WindowHeight / 2 + 1;
            int result = 0;
            bool valid = true;

            ConsoleUI.Initialize();
            do
            {
                ConsoleUI.ClearBuffers();
                if (valid)
                {
                    ConsoleUI.Write(x, y, display, Color.White);
                    ConsoleUI.Render();
                    ConsoleUI.CursorTo(x, y - 2);
                }
                else
                {
                    ConsoleUI.Write(x, y, display, Color.White);
                    ConsoleUI.Write(x, y - 1, "Must be a valid integer.", Color.Red);
                    ConsoleUI.Render();
                    ConsoleUI.CursorTo(x, y - 3);
                }
                try
                {
                    result = Convert.ToInt32(Console.ReadLine());
                    valid = true;
                }
                catch (System.FormatException e)
                {
                    History.Log(e.ToString());
                    valid = false;
                }
                ConsoleUI.CursorOff();
            } while (!valid);

            return result;
        }

        private static int GetLengthOfLongestItem<T>(T[] list)
        {
            int longest = 0;
            foreach (T item in list)
            {
                int length = item.ToString().Length;
                if (length > longest)
                    longest = length;
            }
            return longest;
        }

        public static T Select<T>(String question, T[] list)
        {
            int numberOfItems = list.Length;
            if (numberOfItems == 0)
                return default(T);

            // Set up width/height info for centering
            int displayLength = question.Length;
            int displayWidth = GetLengthOfLongestItem<T>(list);
            if (displayLength > displayWidth)
                displayWidth = displayLength;
            int displayHeight = 1 + numberOfItems;
            if (displayHeight > Console.WindowHeight - 1)
                displayHeight = Console.WindowHeight - 1;
            int headerAndSpacingHeight = 2;
            int viewHeight = displayHeight - headerAndSpacingHeight;

            int x = Console.WindowWidth / 2 - displayWidth / 2;
            int y = Console.WindowHeight / 2 + displayHeight / 2;

            int selected = 0;
            int firstItemInView = 0;
            ConsoleUI.Initialize();
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            do
            {
                ConsoleUI.ClearBuffers();
                ConsoleUI.Write(x, y, question, Color.White);
                for (int i = 0; i < numberOfItems; ++i)
                {
                    if (i >= firstItemInView && i < firstItemInView + viewHeight)
                    {
                        int itemY = y - (i - firstItemInView) - headerAndSpacingHeight;
                        ConsoleUI.Write(x, itemY, list[i].ToString(), selected == i ? Color.Gold.Invert() : Color.Gold);
                    }
                }

                // Debug
                //ConsoleUI.Write(0, 1, "firstItemInView: " + firstItemInView, Color.White);
                //ConsoleUI.Write(0, 1, "displayHeight: " + displayHeight, Color.White);
                //ConsoleUI.Write(0, 1, "firstItemInView: " + firstItemInView, Color.White);

                ConsoleUI.Render();

                //Handle input
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.DownArrow && selected < numberOfItems - 1)
                {
                    ++selected;
                    if (selected == viewHeight + firstItemInView)
                        ++firstItemInView;
                }
                else if (input.Key == ConsoleKey.UpArrow && selected > 0)
                {
                    --selected;
                    if (selected == firstItemInView - 1)
                        --firstItemInView;
                }
            } while (input.Key != ConsoleKey.Enter);

            return list[selected];
        }

    }
}
