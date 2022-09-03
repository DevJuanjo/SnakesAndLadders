namespace SnakesAndLadders.Models
{
    public struct BoardSquare
    {
        public BoardSquare(int squareID)
        {
            this.SquareID = squareID;
        }

        /// <summary>
        /// Square ID
        /// </summary>
        public int SquareID { get; }
    }
}
