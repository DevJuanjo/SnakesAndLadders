using SnakesAndLaddersUI.Models;

namespace SnakesAndLaddersUI.Collections
{
    public class ProgramCommands : Dictionary<ProgramCommand, Action>
    {
        public ProgramCommands()
        {
        }

        public void WriteCommandToConsole()
        {
            foreach (var command in Keys)
            {
                Console.WriteLine($"{command.Command}) {command.Description}");
            }

            Console.WriteLine();
        }

        public bool TryToGetActionCommand(string? userCommand, out Action? action)
        {
            action = null;

            if (int.TryParse(userCommand, out var commandToExecute) == false ||
                    Keys.Any(x => x.Command == commandToExecute) == false)
            {
                return false;
            }

            action = this[Keys.Single(x => x.Command == commandToExecute)];
            return true;
        }
    }
}
