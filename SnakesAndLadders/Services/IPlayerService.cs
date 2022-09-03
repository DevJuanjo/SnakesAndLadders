using SnakesAndLadders.Models;

namespace SnakesAndLadders.Services
{
    public interface IPlayerService
    {
        /// <summary>
        /// Add a new player.
        /// </summary>
        /// <param name="newPlayer">New player to be added</param>
        void AddPlayer(IPlayer newPlayer);

        /// <summary>
        /// Removes a player by name.
        /// </summary>
        /// <param name="playerName">Player's name</param>
        void RemovePlayer(string playerName);

        /// <summary>
        /// Returns a player by name.
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <returns></returns>
        IPlayer? GetPlayer(string playerName);

        /// <summary>
        /// Returns the player that has the turn.
        /// </summary>
        /// <returns></returns>
        IPlayer? GetPlayerWithTurn();

        /// <summary>
        /// List of players.
        /// </summary>
        /// <returns></returns>
        List<IPlayer> Players { get; }

        /// <summary>
        /// Give the turn to the next player.
        /// </summary>
        void SetNextTurn();

        /// <summary>
        /// Move a specific player in a relative way.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="steps"></param>
        void MovePlayerRelative(string name, int steps);

        /// <summary>
        /// Move a specific player in a absolute way.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position">Square ID from Board</param>
        void MovePlayerAbsolute(string name, int position);

        /// <summary>
        /// Start all the players.
        /// </summary>
        void Start();

        /// <summary>
        /// Reset the players.
        /// </summary>
        void Reset();

        /// <summary>
        /// Remove all the existing players.
        /// </summary>
        void Clean();

        /// <summary>
        /// Event triggered when a player wins.
        /// </summary>
        event EventHandler<PlayerWonEventArgs>? OnPlayerWins;
    }
}
