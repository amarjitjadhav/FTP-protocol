using System;
using System.Collections.Generic;
using System.Text;

namespace IO
{
    public static class IOHelper
    {
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
    }
}
