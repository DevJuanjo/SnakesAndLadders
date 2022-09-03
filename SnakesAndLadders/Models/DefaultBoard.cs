using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Services;

namespace SnakesAndLadders.Models
{
    /// <summary>
    /// Represent the DefaultBoard for a SnakeAndLadders game.
    /// </summary>
    public class DefaultBoard : IBoard
    {
        private readonly int numberOfSquares;
        private bool loaded;

        public DefaultBoard(int numberOfSquares)
        {
            if (numberOfSquares <= Constants.MINIMUM_SIZE_BOARD)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfSquares),
                    $"Size of the Board must be at least {Constants.MINIMUM_SIZE_BOARD}");
            }

            this.numberOfSquares = numberOfSquares;
            this.loaded = false;
            this.Squares = new BoardSquare[numberOfSquares];
        }

        /// <summary>
        /// Initialize the board.
        /// </summary>
        public void LoadBoard()
        {
            for(var i = 0; i < numberOfSquares; i++)
            {
                this.Squares[i] = new BoardSquare(i + 1);
            }

            loaded = true;
        }

        /// <summary>
        /// Return the square considers as the start square on the board.
        /// </summary>
        /// <returns></returns>
        public BoardSquare GetStart()
        {
            if(loaded == false)
            {
                throw SnakesAndLaddersBoardException.BoardNotLoadedException();
            }

            return this.Squares[0];
        }

        /// <summary>
        /// Return the square considers as the goal square on the board.
        /// </summary>
        /// <returns></returns>
        public BoardSquare GetGoal()
        {
            if (loaded == false)
            {
                throw SnakesAndLaddersBoardException.BoardNotLoadedException();
            }

            return this.Squares[numberOfSquares - 1];
        }

        /// <summary>
        /// Reset the board.
        /// </summary>
        public void Reset()
        {
            LoadBoard();
        }

        /// <summary>
        /// Check if a specific Square ID exists on the Board.
        /// </summary>
        /// <param name="squareID"></param>
        /// <returns></returns>
        public bool IsPositionOnBoard(int squareID)
        {
            return Squares.Any(x => x.SquareID == squareID);
        }

        public void Clean()
        {
            return;
        }

        /// <summary>
        /// Squares that form the layout for the game.
        /// </summary>
        public BoardSquare[] Squares { get; private set; }
    }
}
