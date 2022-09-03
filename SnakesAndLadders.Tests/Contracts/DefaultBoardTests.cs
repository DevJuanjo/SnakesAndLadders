using FluentAssertions;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;

namespace SnakesAndLadders.Tests.Contracts
{
    public class DefaultBoardTests
    {
        private readonly DefaultBoard board;

        public DefaultBoardTests()
        {
            this.board = new DefaultBoard(100);
        }

        [Fact]
        public void Initialization_with_minimum_size_under_2_must_Throw_ArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DefaultBoard(1));
        }

        [Fact]
        public void LoadBoard_must_fill_array_of_squares_in_sequenz_order_by_id()
        {
            board.Squares.All(x => x.SquareID == default).Should().BeTrue();

            board.LoadBoard();

            board.Squares.All(x => x.SquareID == default).Should().BeFalse();
            board.Squares.Should().BeInAscendingOrder(x => x.SquareID);
        }

        [Fact]
        public void GetStart_must_give_first_square()
        {
            board.LoadBoard();

            board.GetStart().Should().Be(board.Squares[0]);
            board.GetStart().SquareID.Should().Be(1);
        }


        [Fact]
        public void GetStart_must_throw_SnakesAndLaddersBoardException()
        {
            Assert.Throws<SnakesAndLaddersBoardException>(() => board.GetStart());
        }

        [Fact]
        public void GetGoal_must_give_Last_square()
        {
            board.LoadBoard();

            board.GetGoal().Should().Be(board.Squares[99]);
            board.GetGoal().SquareID.Should().Be(100);
        }


        [Fact]
        public void GetGoal_must_throw_SnakesAndLaddersBoardException()
        {
            Assert.Throws<SnakesAndLaddersBoardException>(() => board.GetGoal());
        }
    }
}
