namespace SnakesAndLadders.Services
{
    public interface IDice
    {
        /// <summary>
        /// Return the last result of the roll of the dice.
        /// </summary>
        /// <returns></returns>
        int RollResult { get; }

        /// <summary>
        /// It rolls the dice.
        /// </summary>
        /// <returns>Number get from the roll</returns>
        int Roll();

        /// <summary>
        /// Reset the dice.
        /// </summary>
        void Reset();

        /// <summary>
        /// Clean the dice.
        /// </summary>
        void Clean();
    }
}
