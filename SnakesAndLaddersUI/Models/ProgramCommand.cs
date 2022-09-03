namespace SnakesAndLaddersUI.Models
{
    public struct ProgramCommand
    {
        public ProgramCommand(int command, string description)
        {
            Command = command;
            Description = description;
        }

        public int Command { get; }
        public string Description { get; }
    }
}
