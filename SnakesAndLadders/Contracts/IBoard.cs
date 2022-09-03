using SnakesAndLadders.Models;

namespace SnakesAndLadders.Services
{
    public interface IBoard
    {
        /// <summary>
        /// Initialize the boar.
        /// </summary>
        void LoadBoard();

        /// <summary>
        /// Return the square considers as the start square on the board.
        /// </summary>
        /// <returns></returns>
        BoardSquare GetStart();

        /// <summary>
        /// Return the square considers as the goal square on the board.
        /// </summary>
        /// <returns></returns>
        BoardSquare GetGoal();

        /// <summary>
        /// Array from tiles on the board.
        /// </summary>
        BoardSquare[] Squares { get; }

        /// <summary>
        /// Reset the board.
        /// </summary>
        void Reset();

        /// <summary>
        /// Clean the board.
        /// </summary>
        void Clean();

        /// <summary>
        /// Check if a specific squareID exist on the board.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        bool IsPositionOnBoard(int squareID);
    }
}
