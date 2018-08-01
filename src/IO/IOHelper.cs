using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace IO
{
    /// <summary>
    /// This class contains helpful functions for prompting the user for various types of values.
    /// </summary>
    public static class IOHelper
    {
        private static int pageKeyStepSize = 10;
        
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
        /// Displays a message to the user and prompts them to press enter to continue.
        /// </summary>
        /// <param name="display">Message to display.</param>
        public static void Message(String display)
        {
            ConsoleUI.Initialize();
            ConsoleUI.ClearBuffers();
            int displayLength = display.Length;
            int x = Console.WindowWidth / 2 - displayLength / 2;
            int y = Console.WindowHeight / 2 + 1;
            ConsoleUI.Write(x, y, display, Color.White);
            ConsoleUI.Write(x, y - 2, "Press [enter] to continue.", Color.White);
            ConsoleUI.Render();
            Console.ReadLine();
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
                ConsoleUI.Write(x, y - 2, trueText, selected ? Color.Green.Invert() : Color.Green);
                ConsoleUI.Write(x + trueText.Length + 2, y - 2, falseText, selected ? Color.Green : Color.Green.Invert());
                ConsoleUI.Render();
                while (Console.KeyAvailable == false)
                { }
                while (Console.KeyAvailable == true)
                {
                    input = Console.ReadKey(true);
                }
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

        /// <summary>
        /// Loops through the list to find the longest item (string representation).
        /// </summary>
        /// <typeparam name="T">Type of item in list</typeparam>
        /// <param name="list">List of items</param>
        /// <returns>Returns an integer length</returns>
        private static int GetLengthOfLongestItem<T>(List<T> list)
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

        /// <summary>
        /// Loops through the array to find the longest item (string representation).
        /// </summary>
        /// <typeparam name="T">Type of item in list</typeparam>
        /// <param name="list">List of items</param>
        /// <returns>Returns an integer length</returns>
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

        public static T SelectSorted<T>(String question, List<T> list) where T : IComparable
        {
            list.Sort();
            return Select<T>(question, list);
        }

        /// <summary>
        /// Shows the user a scrolling selection screen to choose from among items in a list.
        /// </summary>
        /// <typeparam name="T">Type of item in the list.</typeparam>
        /// <param name="question">Text to display.</param>
        /// <param name="list">List of items.</param>
        /// <returns>Returns the item selected.</returns>
        public static T Select<T>(String question, List<T> list) where T : IComparable
        {
            T[] array = list.ToArray();
            return Select<T>(question, array);
        }

        /// <summary>
        /// Shows the user a scrilling selection screen to choose from among items in an array.
        /// </summary>
        /// <typeparam name="T">Type of item in the array.</typeparam>
        /// <param name="question">Text to display.</param>
        /// <param name="list">Array of items.</param>
        /// <returns>Returns the item selected.</returns>
        public static T Select<T>(String question, T[] list) where T : IComparable
        {
            int numberOfItems = list.Length;
            

            // Set up width/height info for centering
            int displayLength = question.Length;
            int displayWidth = GetLengthOfLongestItem<T>(list);
            if (displayLength > displayWidth)
                displayWidth = displayLength;
            int displayHeight = 1 + numberOfItems;
            if (displayHeight > Console.WindowHeight - 1)
                displayHeight = Console.WindowHeight - 1;
            int headerAndSpacingHeight = 2;

            int viewHeight = displayHeight - headerAndSpacingHeight + 1;

            if (viewHeight < 1)
                viewHeight = 1;
            

            int x = Console.WindowWidth / 2 - displayWidth / 2;
            int y = Console.WindowHeight / 2 + displayHeight / 2;

            int selected = 0;
            int firstItemInView = 0;

            void SelectUp()
            {
                if (selected > 0)
                {
                    --selected;
                    if (selected == firstItemInView - 1)
                        --firstItemInView;
                }
            }

            void SelectDown()
            {
                if (selected < numberOfItems - 1)
                {
                    ++selected;
                    if (selected == viewHeight + firstItemInView)
                        ++firstItemInView;
                }
            }

            //ConsoleUI.Initialize();
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
                        ConsoleUI.Write(x, itemY, list[i].ToString(), selected == i ? Color.Green.Invert() : Color.Green);
                    }
                }
                // IF there's nothing to select from print a message and return null
                if (numberOfItems == 0)
                {
                    ConsoleUI.Write(x, y - 2, "Nothing to select.", Color.Red);
                    ConsoleUI.Render();
                    Console.ReadLine();
                    return default(T);
                }

                ConsoleUI.Render();

                //Handle input
                while (Console.KeyAvailable == false)
                { }
                while (Console.KeyAvailable == true)
                {
                    input = Console.ReadKey(true);
                }

                if (input.Key == ConsoleKey.DownArrow)
                {
                    SelectDown();
                }
                else if (input.Key == ConsoleKey.PageDown)
                {
                    for (int i = 0; i < pageKeyStepSize; ++i)
                    {
                        SelectDown();
                    }
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                   SelectUp();
                }
                else if (input.Key == ConsoleKey.PageUp)
                {
                    for (int i = 0; i < pageKeyStepSize; ++i)
                    {
                        SelectUp();
                    }
                }
                
                
            } while (input.Key != ConsoleKey.Enter);

            return list[selected];
        }

        /// <summary>
        /// Shows the user a scrolling selection screen to choose multiple items from among
        /// those in the list. Arrow keys navigate, spacebar selects/deselects, and enter confirms
        /// the current selection.
        /// </summary>
        /// <typeparam name="T">Type of item in the array.</typeparam>
        /// <param name="question">Text to display.</param>
        /// <param name="list">Array of items.</param>
        /// <returns>Returns a list of the items selected.</returns>
        public static List<T> SelectMultiple<T>(String question, List<T> list, bool sortList = false) where T : IComparable
        {
            bool firstLoop = true;
            ConsoleUI.Initialize();
            List<T> results = new List<T>();
            int numberOfItems = list.Count;

            // Break up the question into lines if need be
            List<String> questionLines = WordWrap(question, Console.WindowWidth);

            // Sort if the user wants to
            if (sortList)
            {
                list.Sort();
            }

            int roomForSelectionMarker = 2;
            int screenBorder = 0;

            // Set up display width for the longest line or full screen, whichever is shorter.
            // Used for centering small selections.
            int displayWidth = GetLengthOfLongestItem<String>(questionLines);
            displayWidth = Math.Max(displayWidth, GetLengthOfLongestItem<T>(list) + roomForSelectionMarker);
            displayWidth = Math.Min(displayWidth, Console.WindowWidth - 2 * screenBorder);

            // Set up height info for centering display area
            int headerAndSpacingHeight = questionLines.Count + 1;
            int displayHeight = numberOfItems + headerAndSpacingHeight;
            displayHeight = Math.Min(displayHeight, Console.WindowHeight - 2 * screenBorder);
            
            // Set up height info for scrolling view if 
            int viewHeight = displayHeight - headerAndSpacingHeight;
            if (viewHeight < 1)
                viewHeight = 1;

            int displayCornerX = Console.WindowWidth / 2 - displayWidth / 2;
            int displayCornerY = Console.WindowHeight / 2 + displayHeight / 2 - 1;

            // Initialize list for tracking what is selected
            List<bool> selectedItems = new List<bool>();
            for (int i = 0; i < numberOfItems; ++i)
            {
                selectedItems.Add(false);
            }

            // Initial highlighted item and view window position
            int selected = 0;
            int firstItemInView = 0;

            void MoveUp()
            {
                if (selected > 0)
                {
                    --selected;
                    if (selected == firstItemInView - 1)
                        --firstItemInView;
                }
            }

            void MoveDown()
            {
                if (selected < numberOfItems - 1)
                {
                    ++selected;
                    if (selected == viewHeight + firstItemInView)
                        ++firstItemInView;
                }
            }

            //ConsoleUI.Initialize();
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            do
            {
                ConsoleUI.ClearBuffers();
                ConsoleUI.Write(displayCornerX, displayCornerY, questionLines, Color.White);
                for (int i = 0; i < numberOfItems; ++i)
                {
                    if (i >= firstItemInView && i < firstItemInView + viewHeight)
                    {
                        int itemY = displayCornerY - headerAndSpacingHeight - i + firstItemInView;

                        // Draw '*' for selected items
                        if (selectedItems[i])
                        {
                            ConsoleUI.Write(displayCornerX, itemY, "*", selected == i ? Color.Green.Invert() : Color.Green);
                        }

                        // Draw item
                        ConsoleUI.Write(displayCornerX + roomForSelectionMarker, itemY, list[i].ToString(), selected == i ? Color.Green.Invert() : Color.Green);
                    }
                }

                // IF there's nothing to select from print a message and return null
                if (numberOfItems == 0)
                {
                    ConsoleUI.Write(displayCornerX, displayCornerY - headerAndSpacingHeight, "Nothing to select.", Color.Red);
                    ConsoleUI.Render();
                    Console.ReadLine();
                    return results;
                }

                ConsoleUI.Render();

                //Handle input
                while (Console.KeyAvailable == false)
                { }
                while (Console.KeyAvailable == true)
                {
                    input = Console.ReadKey(true);
                }

                if (input.Key == ConsoleKey.DownArrow)
                {
                    MoveDown();
                }
                else if (input.Key == ConsoleKey.PageDown)
                {
                    for (int i = 0; i < pageKeyStepSize; ++i)
                    {
                        MoveDown();
                    }
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    MoveUp();
                }
                else if (input.Key == ConsoleKey.PageUp)
                {
                    for (int i = 0; i < pageKeyStepSize; ++i)
                    {
                        MoveUp();
                    }
                }
                else if (input.Key == ConsoleKey.Spacebar)
                {
                    selectedItems[selected] = !selectedItems[selected];
                }
            } while (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape);

            // Populate results with items selected
            for (int i = 0; i < numberOfItems; ++i)
            {
                if (selectedItems[i])
                {
                    results.Add(list[i]);
                }
            }

            return results;
        }


        /// <summary>
        /// Wraps a string into a list of strings of the specified width.
        /// Code based on post found in this thread:
        /// https://social.msdn.microsoft.com/Forums/en-US/e549e7a7-bcd9-4f18-b797-4590180855c2/wrap-the-text-with-fixed-size-length-of-30-using-c?forum=csharpgeneral
        /// </summary>
        /// <param name="line">String to wrap.</param>
        /// <param name="width">Width at which to wrap.</param>
        /// <returns></returns>
        public static List<String> WordWrap(String line, int width)
        {
            List<String> sentence = new List<String>();
            int index = 0;
            String result = "";
            foreach (char c in line)
            {
                //if smaller than width, add the the result
                if (index <= width)
                {
                    //increase char index
                    index++;
                    result += c;
                }
                if (index == width - 4)
                {
                    //if index hits the chars within width, add to list and clear result and index
                    sentence.Add(result);
                    result = "";
                    index = 0;
                }
            }
            //add the last remaing characters 
            sentence.Add(result);
            return sentence;
        }
    }
}
