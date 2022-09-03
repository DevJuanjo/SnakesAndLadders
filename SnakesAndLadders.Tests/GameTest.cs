using FluentAssertions;
using Moq;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;

namespace SnakesAndLadders.Tests
{
    public class GameTests
    {
        private readonly Game game;
        private readonly Mock<IBoard> mockBoard;
        private readonly Mock<IPlayerService> mockPlayerService;
        private readonly Mock<IDice> mockDice;

        public GameTests()
        {
            mockBoard = new Mock<IBoard>();
            mockPlayerService = new Mock<IPlayerService>();
            mockDice = new Mock<IDice>();

            game = new Game(mockBoard.Object, mockPlayerService.Object, mockDice.Object);
        }

        [Fact]
        public void AddPlayer_must_call_PlayerService_AddPlayer()
        {
            var mockPlayer = new Mock<IPlayer>();

            game.AddPlayer(mockPlayer.Object);

            mockPlayerService.Verify(x => x.AddPlayer(mockPlayer.Object), times: Times.Once);
        }

        [Fact]
        public void AddPlayer_after_starting_must_throw_SnakesAndLaddersGameStatusException()
        {
            StartGame();

            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.AddPlayer(new Mock<IPlayer>().Object));
        }

        [Fact]
        public void RemovePlayer_must_call_PlayerService_RemovePlayer()
        {
            game.RemovePlayer("PlayerTest");

            mockPlayerService.Verify(x => x.RemovePlayer("PlayerTest"), times: Times.Once);
        }

        [Fact]
        public void RemovePlayer_after_starting_must_throw_SnakesAndLaddersGameStatusException()
        {
            StartGame();

            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.RemovePlayer(It.IsAny<string>()));
        }

        [Fact]
        public void Start_must_reset_all_services_and_set_status_to_inProgress()
        {
            StartGame();

            mockBoard.Verify(x => x.Reset(), times: Times.Once);
            mockPlayerService.Verify(x => x.Reset(), times: Times.Once);
            mockDice.Verify(x => x.Reset(), times: Times.Once);

            game.Status.Should().Be(Enums.GameStatus.InProgress);
        }

        [Fact]
        public void Start_must_call_PlayerService_Start()
        {
            mockPlayerService.Setup(x => x.Players).Returns(() => new List<IPlayer>()
            {
                It.IsAny<IPlayer>(),
                It.IsAny<IPlayer>()
            });

            game.Start();

            mockPlayerService.Verify(x => x.Start(), times: Times.Once);
        }

        [Fact]
        public void Start_must_give_turn_to_first_player()
        {
            mockBoard.Setup(x => x.IsPositionOnBoard(It.IsAny<int>())).Returns(true);
            mockBoard.Setup(x => x.GetStart()).Returns(new BoardSquare(1));

            var playerService = new PlayerService(mockBoard.Object);

            var newGame = new Game(mockBoard.Object, playerService, mockDice.Object);

            newGame.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1));
            newGame.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer2));

            playerService.GetPlayerWithTurn().Should().BeNull();

            newGame.Start();

            playerService.GetPlayerWithTurn()!.Name.Should().Be(Constants.DefaulNamePlayer1);
        }

        [Fact]
        public void Start_after_starting_must_throw_SnakesAndLaddersGameStatusException()
        {
            StartGame();

            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.Start());
        }

        [Fact]
        public void Start_with_wrong_number_of_Player_must_throw_SnakesAndLaddersPlayerException()
        {
            mockPlayerService.Setup(x => x.Players).Returns(() => new List<IPlayer>());

            Assert.Throws<SnakesAndLaddersPlayerException>(() => game.Start());
        }

        [Fact]
        public void Clear_must_clean_all_services_and_set_status_to_NotStarted()
        {
            game.Clean();

            mockBoard.Verify(x => x.Clean(), times: Times.Once);
            mockPlayerService.Verify(x => x.Clean(), times: Times.Once);
            mockDice.Verify(x => x.Clean(), times: Times.Once);

            game.Status.Should().Be(Enums.GameStatus.NotStarted);
        }

        [Fact]
        public void Reset_must_resetn_all_services_and_set_status_to_NotStarted()
        {
            game.Reset();

            mockBoard.Verify(x => x.Reset(), times: Times.Once);
            mockPlayerService.Verify(x => x.Reset(), times: Times.Once);
            mockDice.Verify(x => x.Reset(), times: Times.Once);

            game.Status.Should().Be(Enums.GameStatus.NotStarted);
        }

        [Fact]
        public void Finish_must_set_Status_to_Finished()
        {
            StartGame();

            game.Finish();

            game.Status.Should().Be(Enums.GameStatus.Finished);
        }

        [Fact]
        public void Finish_if_not_started_yet_must_throw_SnakesAndLaddersGameStatusException()
        {
            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.Finish());
        }

        [Fact]
        public void RollDices_must_return_DiceResult()
        {
            StartGame();

            mockDice.Setup(x => x.Roll()).Returns(15);

            var rollResult = game.RollDice();

            rollResult.Should().Be(15);
            mockDice.Verify(x => x.Roll(), times: Times.Once);
        }

        [Fact]
        public void GetPlayers_must_return_all_players()
        {
            mockPlayerService.Setup(x => x.Players).Returns(() => new List<IPlayer>()
            {
                new DefaultPlayer(Constants.DefaulNamePlayer1),
                new DefaultPlayer(Constants.DefaulNamePlayer2)
            });

            var players = game.GetPlayers();

            players.Should().HaveCount(2);

            players[0].Name.Should().Be(Constants.DefaulNamePlayer1);
            players[1].Name.Should().Be(Constants.DefaulNamePlayer2);
        }

        [Fact]
        public void GetPlayerWithTurn_must_return_the_player_with_HasTurn_true()
        {
            StartGame();

            mockPlayerService.Setup(x => x.GetPlayerWithTurn()).Returns(() => new DefaultPlayer(Constants.DefaulNamePlayer2));

            var player = game.GetPlayerWithTurn();

            player.Should().NotBeNull();
            player!.Name.Should().Be(Constants.DefaulNamePlayer2);
        }

        [Fact]
        public void GetPlayerWithTurn_if_not_started_yet_must_throw_SnakesAndLaddersGameStatusException()
        {
            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.GetPlayerWithTurn());
        }

        [Fact]
        public void ExecuteNextTurn_must_move_Player1_to_position_4()
        {
            mockDice.Setup(x => x.Roll()).Returns(() => 3);

            mockBoard.Setup(x => x.IsPositionOnBoard(It.IsAny<int>())).Returns(true);
            mockBoard.Setup(x => x.GetStart()).Returns(new BoardSquare(1));

            var playerService = new PlayerService(mockBoard.Object);

            var newGame = new Game(mockBoard.Object, playerService, mockDice.Object);

            newGame.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1));

            newGame.Start();

            playerService.Players.All(x => x.Position == 1).Should().Be(true);

            newGame.ExecuteNextTurn();

            playerService.Players.All(x => x.Position == 4).Should().Be(true);
        }

        [Fact]
        public void ExecuteNextTurn_finished_must_throw_SnakesAndLaddersGameStatusException()
        {
            StartGame();

            game.Finish();

            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.ExecuteNextTurn());
        }

        [Fact]
        public void ExecuteNextTurn_NotStarted_must_throw_SnakesAndLaddersGameStatusException()
        {
            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.ExecuteNextTurn());
        }

        [Fact]
        public void RollDices_finished_must_throw_SnakesAndLaddersGameStatusException()
        {
            StartGame();

            game.Finish();

            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.RollDice());
        }

        [Fact]
        public void RollDices_NotStarted_must_throw_SnakesAndLaddersGameStatusException()
        {
            Assert.Throws<SnakesAndLaddersGameStatusException>(() => game.RollDice());
        }

        [Fact]
        public void ExecuteNextTurn_move_3_positions_to_100_and_game_is_finished()
        {
            mockDice.Setup(x => x.Roll()).Returns(3);

            var board = new DefaultBoard(100);

            var newGame = new Game(board, new PlayerService(board), mockDice.Object);

            var player = new DefaultPlayer(Constants.DefaulNamePlayer1);

            newGame.AddPlayer(player);

            newGame.Start();

            player.MoveAbsolute(board, 97);

            newGame.ExecuteNextTurn();

            newGame.Status.Should().Be(Enums.GameStatus.Finished);
        }

        /// <summary>
        /// US1 - UAT1
        /// Given the game is started
        /// When the token is placed on the board
        /// Then the token is on square 1
        /// </summary>
        [Fact]
        public void US1_UAT1_Start_must_place_all_Players_On_Position_1()
        {
            mockBoard.Setup(x => x.IsPositionOnBoard(It.IsAny<int>())).Returns(true);
            mockBoard.Setup(x => x.GetStart()).Returns(new BoardSquare(1));

            var playerService = new PlayerService(mockBoard.Object);

            var newGame = new Game(mockBoard.Object, playerService, mockDice.Object);

            newGame.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1));
            newGame.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer2));

            playerService.Players.All(x => x.Position == 0).Should().Be(true);

            newGame.Start();

            playerService.Players.All(x => x.Position == 1).Should().Be(true);
        }

        /// <summary>
        /// US3 - UAT1
        /// Given the game is started
        /// When the player rolls a die
        /// Then the result should be between 1-6 inclusive
        /// </summary>
        [Fact]
        public void US3_UAT1_RollDice_game_started_must_give_result_between_1_and_6()
        {
            mockPlayerService.Setup(x => x.Players).Returns(() => new List<IPlayer>()
            {
                It.IsAny<IPlayer>()
            });

            var newGame = new Game(mockBoard.Object, mockPlayerService.Object, new DefaultDice());

            newGame.Start();

            var rollResult = new List<int>();

            for (int i = 0; i <= 10; i++)
            {
                rollResult.Add(newGame.RollDice());
            }

            rollResult.Should().AllSatisfy(result => result.Should().BeInRange(1, 6));
        }

        /// <summary>
        /// US3 - UAT2
        /// Given the player rolls a 4
        /// When they move their token
        /// Then the token should move 4 spaces
        /// </summary>
        [Fact]
        public void US3_UAT2_ExecuteNextTurn_roll_4_must_move_player_4_postions()
        {
            mockBoard.Setup(x => x.IsPositionOnBoard(It.IsAny<int>())).Returns(true);
            mockBoard.Setup(x => x.GetStart()).Returns(new BoardSquare(1));
            mockDice.Setup(x => x.Roll()).Returns(4);

            var playerService = new PlayerService(mockBoard.Object);

            playerService.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1));

            var newGame = new Game(mockBoard.Object, playerService, mockDice.Object);

            newGame.Start();

            var player = newGame.GetPlayer(Constants.DefaulNamePlayer1);

            player!.Position.Should().Be(1);

            newGame.ExecuteNextTurn();

            player!.Position.Should().Be(5);
        }

        private void StartGame()
        {
            mockPlayerService.Setup(x => x.Players).Returns(() => new List<IPlayer>()
            {
                It.IsAny<IPlayer>()
            });

            game.Start();
        }
    }
}
