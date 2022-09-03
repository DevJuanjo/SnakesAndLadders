using SnakesAndLadders.Enums;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;

namespace SnakesAndLadders.Contracts
{
    public interface IGame
    {
        /// <summary>
        /// Status of the game
        /// </summary>
        public GameStatus Status { get; }

        /// <summary>
        /// Return a specific Player by name.
        /// </summary>
        public IPlayer? GetPlayer(string name);

        /// <summary>
        /// Return a list of the players in the games.
        /// </summary>
        public List<IPlayer> GetPlayers();

        /// <summary>
        /// Return the dice being using in the game.
        /// </summary>
        IDice GetDice();

        /// <summary>
        /// Return the player with the turn.
        /// </summary>
        /// <returns></returns>
        IPlayer? GetPlayerWithTurn();

        /// <summary>
        /// Add a new player to the game.
        /// </summary>
        /// <param name="newPlayer"></param>
        void AddPlayer(IPlayer newPlayer);

        /// <summary>
        /// Remove a specific player by name.
        /// </summary>
        /// <param name="playerName">Player name</param>
        void RemovePlayer(string playerName);

        /// <summary>
        /// Start a new game
        /// </summary>
        void Start();

        /// <summary>
        /// Finish the game
        /// </summary>
        void Finish();

        /// <summary>
        /// Reset all the elements of the game.
        /// </summary>
        void Reset();

        /// <summary>
        /// Clean all the properties of the game.
        /// </summary>
        void Clean();

        /// <summary>
        /// Trigger new turn
        /// </summary>
        void ExecuteNextTurn();

        /// <summary>
        /// Roll the dice.
        /// </summary>
        /// <returns>Result of the roll</returns>
        int RollDice();

        /// <summary>
        /// Event triggered when a player wins.
        /// </summary>
        event EventHandler<PlayerWonEventArgs>? OnPlayerWins;
    }
}
