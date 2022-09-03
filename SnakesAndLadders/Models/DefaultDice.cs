using SnakesAndLadders.Services;

namespace SnakesAndLadders.Models
{
    /// <summary>
    /// Represent a 6 sides dice
    /// </summary>
    public class DefaultDice : IDice
    {
        Random random;

        public DefaultDice()
        {
            this.random = new Random();
        }

        /// <summary>
        /// Roll result.
        /// </summary>
        public int RollResult { get; private set; }

        /// <summary>
        /// Reset the dice.
        /// </summary>
        public void Reset()
        {
            return;
        }

        /// <summary>
        /// Clean the dice.
        /// </summary>
        public void Clean()
        {
            return;
        }

        /// <summary>
        /// Roll the dice.
        /// </summary>
        /// <returns>Value of the dice after rolling it</returns>
        public int Roll()
        {
            RollResult = random.Next(1, 7);

            return RollResult;
        }
    }
}
