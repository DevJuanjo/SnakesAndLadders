using SnakesAndLadders.Enums;
using System.Drawing;

namespace SnakesAndLaddersUI.Collections
{
    public class GameStatusConsoleCollection : Dictionary<GameStatus, ConsoleColor>
    {
        public GameStatusConsoleCollection()
        {
            Add(GameStatus.NotStarted, ConsoleColor.DarkYellow);
            Add(GameStatus.InProgress, ConsoleColor.Green);
            Add(GameStatus.Finished, ConsoleColor.Red);

        }

        public void WriteStatusOnConsole(GameStatus status)
        {
            Console.ForegroundColor = GetColor(status);
            Console.WriteLine($"Game Status: {status}");
            Console.ResetColor();
        }

        private ConsoleColor GetColor(GameStatus status)
        {
            if (TryGetValue(status, out var color) == false)
            {
                return ConsoleColor.White;
            }

            return color;
        }
    }
}
