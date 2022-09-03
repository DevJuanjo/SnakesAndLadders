using FluentAssertions;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;

namespace SnakesAndLadders.Tests.Contracts
{
    public class DefaultPlayerTests
    {
        private readonly DefaultPlayer player;

        public DefaultPlayerTests()
        {
            player = new DefaultPlayer(Constants.DefaulNamePlayer1);
        }

        [Fact]
        public void MoveRelativ_must_move_with_step_3_from_1_to_4()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            player.GiveTurn();
            player.MoveAbsolute(board, 1);
            player.MoveRelative(board, 3);

            player.Position.Should().Be(4);
        }

        [Fact]
        public void MoveRelativ_no_turn_must_throw_SnakesAndLaddersPlayerException()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            Assert.Throws<SnakesAndLaddersPlayerException>(() => player.MoveRelative(board, 3));
        }

        [Fact]
        public void MoveRelativ_new_position_out_of_board_boundaries_must_keep_position()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            player.GiveTurn();
            player.MoveAbsolute(board, 99);
            player.MoveRelative(board, 2);

            player.Position.Should().Be(99);
        }


        [Fact]
        public void MoveRelativ_new_position_is_goal_of_the_board_must_set_iswinner()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            player.GiveTurn();
            player.MoveAbsolute(board, 99);
            player.MoveRelative(board, 1);

            player.Position.Should().Be(100);
            player.IsWinner.Should().BeTrue();
        }

        [Fact]
        public void MoveAbsolute_must_move_from_1_to_4()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            player.MoveAbsolute(board, 1);
            player.MoveAbsolute(board, 4);

            player.Position.Should().Be(4);
        }

        [Fact]
        public void MoveAbsolute_new_position_out_of_board_boundaries_must_throw_SnakesAndLaddersBoardException()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            Assert.Throws<SnakesAndLaddersBoardException>(() => player.MoveAbsolute(board, 101));
        }


        [Fact]
        public void MoveAbsolute_new_position_is_goal_of_the_board_must_set_iswinner()
        {
            var board = new DefaultBoard(100);
            board.LoadBoard();

            player.MoveAbsolute(board, 100);

            player.Position.Should().Be(100);
            player.IsWinner.Should().BeTrue();
        }
    }
}
