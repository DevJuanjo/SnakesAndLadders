using SnakesAndLadders.Contracts;
using SnakesAndLadders.Enums;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;

namespace SnakesAndLadders
{
    /// <summary>
    /// Snake and Ladder game
    /// </summary>
    public class Game : IGame
    {
        private readonly IBoard board;
        private readonly IPlayerService playerManager;
        private readonly IDice dice;

        public Game(IBoard board, IPlayerService playerManager, IDice dice)
        {
            this.board = board;
            this.playerManager = playerManager;
            this.dice = dice;

            this.board.LoadBoard();
            Status = GameStatus.NotStarted;

            playerManager.OnPlayerWins += PlayerManager_OnPlayerWins;
        }

        /// <summary>
        /// Status of the game
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// Return a specific player by name.
        /// </summary>
        public IPlayer? GetPlayer(string name) => playerManager.GetPlayer(name);

        /// <summary>
        /// Return a list of the players in the games.
        /// </summary>
        public List<IPlayer> GetPlayers() => playerManager.Players ?? new List<IPlayer>();

        /// <summary>
        /// Return the dice being using in the game.
        /// </summary>
        public IDice GetDice() => dice;

        /// <summary>
        /// Return the player with the turn.
        /// </summary>
        /// <returns></returns>
        public IPlayer? GetPlayerWithTurn()
        {
            if (Status != GameStatus.InProgress)
            {
                throw SnakesAndLaddersGameStatusException.GameNotStartedException();
            }

            return playerManager.GetPlayerWithTurn();
        }

        /// <summary>
        /// Add a new player to the game.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void AddPlayer(IPlayer newPlayer)
        {
            if (Status == GameStatus.InProgress)
            {
                throw SnakesAndLaddersGameStatusException.GameAlreadyStartedException();
            }

            playerManager.AddPlayer(newPlayer);
        }

        /// <summary>
        /// Remove a specific player by name.
        /// </summary>
        /// <param name="playerName">Player name</param>
        public void RemovePlayer(string playerName)
        {
            if (Status == GameStatus.InProgress)
            {
                throw SnakesAndLaddersGameStatusException.GameAlreadyStartedException();
            }

            playerManager.RemovePlayer(playerName);
        }

        /// <summary>
        /// Start a new game.
        /// </summary>
        public void Start()
        {
            if (Status == GameStatus.InProgress)
            {
                throw SnakesAndLaddersGameStatusException.GameAlreadyStartedException();
            }

            if (GetPlayers().Count < Constants.MINIMUM_NUMBER_OF_PLAYERS_BY_GAME)
            {
                throw SnakesAndLaddersPlayerException.NotEnoughPlayersException();
            }

            Reset();

            playerManager.Start();
            playerManager.SetNextTurn();

            Status = GameStatus.InProgress;
        }

        /// <summary>
        /// Finish the game.
        /// </summary>
        public void Finish()
        {
            if (Status != GameStatus.InProgress)
            {
                throw SnakesAndLaddersGameStatusException.GameNotStartedException();
            }

            Status = GameStatus.Finished;
        }

        /// <summary>
        /// Reset all the elements of the game.
        /// </summary>
        public void Reset()
        {
            playerManager.Reset();
            dice.Reset();
            board.Reset();

            Status = GameStatus.NotStarted;
        }

        /// <summary>
        /// Clean all the properties of the game.
        /// </summary>
        public void Clean()
        {
            playerManager.Clean();
            dice.Clean();
            board.Clean();

            Status = GameStatus.NotStarted;
        }

        /// <summary>
        /// Trigger new turn.
        /// </summary>
        public void ExecuteNextTurn()
        {
            if (Status == GameStatus.Finished)
            {
                throw SnakesAndLaddersGameStatusException.GameIsFinishedException();
            }

            if (Status == GameStatus.NotStarted)
            {
                throw SnakesAndLaddersGameStatusException.GameNotStartedException();
            }

            playerManager.MovePlayerRelative(playerManager.GetPlayerWithTurn()!.Name, RollDice());

            playerManager.SetNextTurn();
        }

        /// <summary>
        /// Roll the dice.
        /// </summary>
        /// <returns>Result of the roll</returns>
        public int RollDice()
        {
            if (Status == GameStatus.Finished)
            {
                throw SnakesAndLaddersGameStatusException.GameIsFinishedException();
            }

            if (Status == GameStatus.NotStarted)
            {
                throw SnakesAndLaddersGameStatusException.GameNotStartedException();
            }

            return dice.Roll();
        }

        /// <summary>
        /// Event triggered when a player wins.
        /// </summary>
        public event EventHandler<PlayerWonEventArgs>? OnPlayerWins;

        private void PlayerManager_OnPlayerWins(object? sender, PlayerWonEventArgs e)
        {
            Status = GameStatus.Finished;

            OnPlayerWins?.Invoke(this, e);
        }
    }
}
