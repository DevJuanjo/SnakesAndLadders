namespace SnakesAndLadders.Services
{
    public interface IPlayer 
    { 
        /// <summary>
        /// Name of the player.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return true if the player has the turn to roll the dices.
        /// </summary>
        bool HasTurn { get; }

        /// <summary>
        /// Return true if the player has won.
        /// </summary>
        bool IsWinner { get; }

        /// <summary>
        /// Current Position of the player on the board.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Move the player around the <paramref name="board"/> in a relative way respect its current position.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="steps">Number of steps to move forward or backwards</param>
        void MoveRelative(IBoard board, int steps);

        /// <summary>
        /// Move the player around the <paramref name="board"/> in a absolute way.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="position">SquareID to move</param>
        void MoveAbsolute(IBoard board, int position);

        /// <summary>
        /// Give the turn to the player.
        /// </summary>
        void GiveTurn();

        /// <summary>
        /// Remove the turn to the player.
        /// </summary>
        void RemoveTurn();

        /// <summary>
        /// Reset the player.
        /// </summary>
        void Reset();

        /// <summary>
        /// Event triggered when a player wins.
        /// </summary>
        public event EventHandler? OnPlayerWins;
    }
}
