using FluentAssertions;
using SnakesAndLadders.Models;

namespace SnakesAndLadders.Tests.Contracts
{
    public class DefaultDiceTests
    {
        private readonly DefaultDice defaultDice;

        public DefaultDiceTests()
        {
            defaultDice = new DefaultDice();
        }

        [Fact]
        public void Verify_Roll_Must_Give_A_Number_Between_1_and_6()
        {
            var rollResult = new List<int>();

            for(int i = 0; i <= 10; i++)
            {
                rollResult.Add(defaultDice.Roll());
            }

            rollResult.Should().AllSatisfy(result => result.Should().BeInRange(1, 6));
        }
    }
}
