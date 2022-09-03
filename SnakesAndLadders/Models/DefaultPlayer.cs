using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Services;

namespace SnakesAndLadders.Models
{
    /// <summary>
    /// Represent a default player of the game.
    /// </summary>
    public class DefaultPlayer : IPlayer
    {
        public DefaultPlayer(string name)
        {
            this.Name = name;
            this.HasTurn = false;
            this.Position = default;
        }

        /// <summary>
        /// Name of the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Return true if the player has the turn to roll the dices.
        /// </summary>
        public bool HasTurn { get; private set; }

        /// <summary>
        /// Return true if the player has won.
        /// </summary>
        public bool IsWinner { get; private set; }

        /// <summary>
        /// Position of the player on the board.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Move the player around the <paramref name="board"/> in a relative way respect its current position.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="steps">Number of steps to move forward or backwards</param>
        public void MoveRelative(IBoard board, int steps)
        {
            if(HasTurn == false)
            {
                throw SnakesAndLaddersPlayerException.PlayerHasNoTurnToMove(Name);
            }

            var finalPosition = Position + steps;

            if (board.IsPositionOnBoard(finalPosition))
            {
                Position = finalPosition;
            }

            if (board.GetGoal().SquareID == finalPosition)
            {
                IsWinner = true;
                OnPlayerWins?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Move the player around the <paramref name="board"/> in a absolute way.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="position"></param>
        public void MoveAbsolute(IBoard board, int position)
        {
            if (board.IsPositionOnBoard(position) == false)
            {
                throw SnakesAndLaddersBoardException.BoardPositionNotExists();
            }

            Position = position;

            if (board.GetGoal().SquareID == position)
            {
                IsWinner = true;
                OnPlayerWins?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Give the Turn to the player.
        /// </summary>
        public void GiveTurn()
        {
            HasTurn = true;
        }

        /// <summary>
        /// Remove the Turn to the player.
        /// </summary>
        public void RemoveTurn()
        {
            HasTurn = false;
        }

        /// <summary>
        /// Reset the player.
        /// </summary>
        public void Reset()
        {
            HasTurn = false;
            Position = default;
        }

        /// <summary>
        /// Event triggered when a player wins
        /// </summary>
        public event EventHandler? OnPlayerWins;
    }
}
