using FluentAssertions;
using Moq;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;

namespace SnakesAndLadders.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly PlayerService playerService;
        private readonly Mock<IBoard> mockBoard;

        public PlayerServiceTests()
        {
            mockBoard = new Mock<IBoard>();

            playerService = new PlayerService(mockBoard.Object);
        }

        [Fact]
        public void AddPlayer_should_add_new_player()
        {
            playerService.Players.Should().HaveCount(0);

            playerService.AddPlayer(new DefaultPlayer("TestPlayer"));

            playerService.Players.Should().HaveCount(1);

            playerService.Players[0].Name.Should().Be("TestPlayer");
        }

        [Fact]
        public void AddPlayer_already_exists_must_throw_SnakeAndLaddersPlayerException()
        {
            StartPlayersSource();
            
            Assert.Throws<SnakesAndLaddersPlayerException>(() => playerService.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1)));
        }

        [Fact]
        public void RemovePlayer_must_remove_player()
        {
            StartPlayersSource();

            playerService.Players.Should().HaveCount(2);

            playerService.RemovePlayer(Constants.DefaulNamePlayer1);

            playerService.Players.Should().HaveCount(1);

            playerService.Players[0].Name.Should().NotBe(Constants.DefaulNamePlayer1);
        }

        [Fact]
        public void GetPlayer_must_return_player_By_name()
        {
            StartPlayersSource();

            playerService.GetPlayer(Constants.DefaulNamePlayer1).Should().NotBeNull();
        }

        [Fact]
        public void GetPlayerWithTurn_must_return_player_with_HasTurn_true()
        {
            StartPlayersSource();
            
            playerService.GetPlayer(Constants.DefaulNamePlayer1)!.GiveTurn();

            playerService.GetPlayerWithTurn()!.Name.Should().Be(Constants.DefaulNamePlayer1);
        }

        [Fact]
        public void GetPlayers_must_return_all_players()
        {
            StartPlayersSource();

            playerService.Players.Should().HaveCount(2);

            playerService.Players[0].Name.Should().Be(Constants.DefaulNamePlayer1);
            playerService.Players[1].Name.Should().Be(Constants.DefaulNamePlayer2);
        }

        [Fact]
        public void Start_must_move_all_players_to_start()
        {
            StartPlayersSource();

            mockBoard.Setup(x => x.IsPositionOnBoard(It.IsAny<int>())).Returns(true);
            mockBoard.Setup(x => x.GetStart()).Returns(new BoardSquare(1));

            playerService.Players.All(x => x.Position == 0).Should().Be(true);

            playerService.Start();

            playerService.Players.All(x => x.Position == 1).Should().Be(true);
        }


        [Fact]
        public void Reset_must_reset_all_players()
        {
            var mockPlayer1 = new Mock<IPlayer>();
            var mockPlayer2 = new Mock<IPlayer>();

            mockPlayer1.Setup((x) => x.Name).Returns(() => Constants.DefaulNamePlayer1);
            mockPlayer2.Setup((x) => x.Name).Returns(() => Constants.DefaulNamePlayer2);

            playerService.AddPlayer(mockPlayer1.Object);
            playerService.AddPlayer(mockPlayer2.Object);

            playerService.Reset();

            mockPlayer1.Verify(x => x.Reset(), times: Times.Once);
            mockPlayer2.Verify(x => x.Reset(), times: Times.Once);
        }

        [Fact]
        public void Clean_must_remove_all_players()
        {
            StartPlayersSource();

            playerService.Clean();

            playerService.Players.Should().HaveCount(0);
        }

        [Fact]
        public void MovePlayerRelative_should_call_MovePlayer()
        {
            var mockPlayer = new Mock<IPlayer>();

            mockPlayer.Setup((x) => x.Name).Returns(() => Constants.DefaulNamePlayer1); 

            playerService.AddPlayer(mockPlayer.Object);

            playerService.MovePlayerRelative(Constants.DefaulNamePlayer1, It.IsAny<int>());

            mockPlayer.Verify(x => x.MoveRelative(mockBoard.Object, It.IsAny<int>()), times: Times.Once);
        }

        [Fact]
        public void MovePlayerRelative_Player_not_found_should_throw_SnakeAndLaddersPlayerException()
        {
            Assert.Throws<SnakesAndLaddersPlayerException>(() => playerService.MovePlayerRelative(Constants.DefaulNamePlayer1, It.IsAny<int>()));
        }

        [Fact]
        public void MovePlayerAbsolute_should_call_MovePlayer()
        {
            var mockPlayer = new Mock<IPlayer>();

            mockPlayer.Setup((x) => x.Name).Returns(() => Constants.DefaulNamePlayer1);

            playerService.AddPlayer(mockPlayer.Object);

            playerService.MovePlayerAbsolute(Constants.DefaulNamePlayer1, It.IsAny<int>());

            mockPlayer.Verify(x => x.MoveAbsolute(mockBoard.Object, It.IsAny<int>()), times: Times.Once);
        }

        [Fact]
        public void MovePlayerAbsolute_Player_not_found_should_throw_SnakeAndLaddersPlayerException()
        {
            Assert.Throws<SnakesAndLaddersPlayerException>(() => playerService.MovePlayerAbsolute(Constants.DefaulNamePlayer1, It.IsAny<int>()));
        }

        [Fact]
        public void SetNextTurn_must_set_turn_to_the_next_in_List()
        {
            StartPlayersSource();

            playerService.Players.First()!.GiveTurn();

            playerService.Players[0].HasTurn.Should().Be(true);
            playerService.Players[1].HasTurn.Should().Be(false);

            playerService.SetNextTurn();

            playerService.Players[0].HasTurn.Should().Be(false);
            playerService.Players[1].HasTurn.Should().Be(true);
        }

        [Fact]
        public void SetNextTurn_last_one_in_list_has_turn_must_set_turn_to_the_first()
        {
            StartPlayersSource();

            playerService.Players.Last()!.GiveTurn();

            playerService.Players[0].HasTurn.Should().Be(false);
            playerService.Players[1].HasTurn.Should().Be(true);

            playerService.SetNextTurn();

            playerService.Players[0].HasTurn.Should().Be(true);
            playerService.Players[1].HasTurn.Should().Be(false);
        }

        [Fact]
        public void SetNextTurn_if_no_player_has_Turn_must_set_turn_to_the_first_one()
        {
            StartPlayersSource();

            playerService.Players.Any(x => x.HasTurn).Should().Be(false);

            playerService.SetNextTurn();

            playerService.Players[0].HasTurn.Should().Be(true); 
        }

        [Fact]
        public void SetNextTurn_No_Players_in_list_must_throw_SnakeAndLaddersPlayerException()
        {
            Assert.Throws<SnakesAndLaddersPlayerException>(() => playerService.SetNextTurn());
        }

        /// <summary>
        /// US1 - UAT2
        /// Given the token is on square 1
        /// When the token is moved 3 spaces
        /// Then the token is on square 4
        /// </summary>
        [Fact]
        public void US1_UAT2_MovePlayerRelativ_Player_pos_1_move_3_positions_to_4()
        {
            var player = new DefaultPlayer(Constants.DefaulNamePlayer1);
            var board = new DefaultBoard(100);

            player.GiveTurn();
            board.LoadBoard();

            var newPlayerService = new PlayerService(board);

            newPlayerService.AddPlayer(player);

            newPlayerService.Start();

            newPlayerService.MovePlayerRelative(Constants.DefaulNamePlayer1, 3);

            player.Position.Should().Be(4);
        }

        /// <summary>
        /// US1 - UAT3
        /// Given the token is on square 1
        /// When the token is moved 3 spaces
        /// And then it is moved 4 spaces
        /// Then the token is on square 8
        /// </summary>
        [Fact]
        public void US1_UAT3_MovePlayerRelativ_Player_pos_1_move_3_and_then_4_positions_to_8()
        {
            var player = new DefaultPlayer(Constants.DefaulNamePlayer1);
            var board = new DefaultBoard(100);

            player.GiveTurn();
            board.LoadBoard();

            var newPlayerService = new PlayerService(board);

            newPlayerService.AddPlayer(player);

            newPlayerService.Start();

            newPlayerService.MovePlayerRelative(Constants.DefaulNamePlayer1, 3);
            newPlayerService.MovePlayerRelative(Constants.DefaulNamePlayer1, 4);

            player.Position.Should().Be(8);
        }


        /// <summary>
        /// US2 - UAT1
        /// Given the token is on square 97
        /// When the token is moved 3 spaces
        /// Then the token is on square 100
        /// And the player has won the game
        /// </summary>
        [Fact]
        public void US2_UAT1_MovePlayerRelativ_move_3_positions_to_100_and_player_win_game()
        {
            var player = new DefaultPlayer(Constants.DefaulNamePlayer1);
            var board = new DefaultBoard(100);

            player.GiveTurn();
            board.LoadBoard();

            var newPlayerService = new PlayerService(board);

            newPlayerService.AddPlayer(player);

            newPlayerService.MovePlayerAbsolute(Constants.DefaulNamePlayer1, 97);

            newPlayerService.MovePlayerRelative(Constants.DefaulNamePlayer1, 3);

            player.IsWinner.Should().BeTrue();
            player.Position.Should().Be(100);
        }

        /// <summary>
        /// US2 - UAT2
        /// Given the token is on square 97
        /// When the token is moved 4 spaces
        /// Then the token is on square 100
        /// And the player has not won the game
        /// </summary>
        [Fact]
        public void US2_UAT2_MovePlayerRelative_move_4_positions_to_101_and_player_not_win_game()
        {
            var player = new DefaultPlayer(Constants.DefaulNamePlayer1);
            var board = new DefaultBoard(100);

            player.GiveTurn();
            board.LoadBoard();

            var newPlayerService = new PlayerService(board);

            newPlayerService.AddPlayer(player);

            newPlayerService.MovePlayerAbsolute(Constants.DefaulNamePlayer1, 97);

            newPlayerService.MovePlayerRelative(Constants.DefaulNamePlayer1, 4);

            player.IsWinner.Should().BeFalse();
            player.Position.Should().Be(97);
        }

        private void StartPlayersSource()
        {
            playerService.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer1));
            playerService.AddPlayer(new DefaultPlayer(Constants.DefaulNamePlayer2));
        }
    }
}
