using System;

namespace LinqConsole
{
    class ConsoleHelper
    {
        public static void With(ConsoleColor background, ConsoleColor foreground, Action action)
        {
            ConsoleColor oldBackground = Console.BackgroundColor;
            ConsoleColor oldForeground = Console.ForegroundColor;

            try
            {
                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;

                action();
            }
            finally
            {
                Console.BackgroundColor = oldBackground;
                Console.ForegroundColor = oldForeground;
            }
        }
    }
}
