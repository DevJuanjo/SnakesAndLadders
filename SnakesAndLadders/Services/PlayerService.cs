using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;

namespace SnakesAndLadders.Services
{
    /// <summary>
    /// Players manager service.
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IBoard board;

        public PlayerService(IBoard board)
        {
            this.board = board;
            this.Players = new List<IPlayer>();
        }

        /// <summary>
        /// Add a new player to the list of players.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void AddPlayer(IPlayer newPlayer)
        {
            if (this.GetPlayer(newPlayer.Name) != null)
            {
                throw SnakesAndLaddersPlayerException.PlayerExistsAlreadyException(newPlayer.Name);
            }

            newPlayer.OnPlayerWins += Player_OnPlayerWins;

            Players.Add(newPlayer);
        }

        /// <summary>
        /// Remove a specific player from the list of players by name.
        /// </summary>
        /// <param name="playerName"></param>
        public void RemovePlayer(string playerName)
        {
            var playerStoraged = this.GetPlayer(playerName);
            if (playerStoraged == null)
            {
                return;
            }

            Players.Remove(playerStoraged);
        }

        /// <summary>
        /// List of players.
        /// </summary>
        /// <returns></returns>
        public List<IPlayer> Players { get; }

        /// <summary>
        /// Return a specific player by name.
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>Matched Player. Null if not found</returns>
        public IPlayer? GetPlayer(string playerName)
        {
            return Players.FirstOrDefault(player => player.Name.Equals(playerName));
        }

        /// <summary>
        /// Returns the player that has the turn.
        /// </summary>
        /// <returns></returns>
        public IPlayer? GetPlayerWithTurn()
        {
            return Players.SingleOrDefault(player => player.HasTurn);
        }

        /// <summary>
        /// Move players in list to the start position.
        /// </summary>
        public void Start()
        {
            foreach (var players in Players)
            {
                players.MoveAbsolute(board, board.GetStart().SquareID);
            }
        }

        /// <summary>
        /// Reset every player in the list.
        /// </summary>
        public void Reset()
        {
            foreach(var player in Players)
            {
                player.Reset();
            }
        }

        /// <summary>
        /// Clean the list of players.
        /// </summary>
        public void Clean()
        {
            Players.Clear();
        }

        /// <summary>
        /// Move a player by name in a relative way.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="steps"></param>
        public void MovePlayerRelative(string name, int steps)
        {
            var playerToMove = this.GetPlayer(name);
            if (playerToMove == null)
            {
                throw SnakesAndLaddersPlayerException.PlayerNotFoundException(name);
            }

            playerToMove.MoveRelative(board, steps);
        }

        /// <summary>
        /// Move a player by name in a absolute way.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        public void MovePlayerAbsolute(string name, int position)
        {
            var playerToMove = this.GetPlayer(name);
            if (playerToMove == null)
            {
                throw SnakesAndLaddersPlayerException.PlayerNotFoundException(name);
            }

            playerToMove.MoveAbsolute(board, position);
        }

        /// <summary>
        /// Give the turn to the next player.
        /// </summary>
        public void SetNextTurn()
        {
            if(Players.Any() == false)
            {
                throw SnakesAndLaddersPlayerException.NoPlayersFound();
            }

            var currentPlayerWithTurn = Players.FirstOrDefault(x => x.HasTurn);
            if (currentPlayerWithTurn == null)
            {
                Players.First().GiveTurn();
                return;
            }

            Players.ForEach(x => x.RemoveTurn());

            var indexOfCurrentPlayerWithTurn = Players.IndexOf(currentPlayerWithTurn);
            if (indexOfCurrentPlayerWithTurn + 1 == Players.Count)
            {
                Players.First().GiveTurn();
                return;
            }

            Players[indexOfCurrentPlayerWithTurn + 1].GiveTurn();
        }

        /// <summary>
        /// Event triggered when a player wins.
        /// </summary>
        public event EventHandler<PlayerWonEventArgs>? OnPlayerWins;

        private void Player_OnPlayerWins(object? sender, EventArgs e)
        {
            OnPlayerWins?.Invoke(this, new PlayerWonEventArgs((IPlayer)sender));
        }
    }
}
