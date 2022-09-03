using SnakesAndLadders.Services;

namespace SnakesAndLadders.Models
{
    public class PlayerWonEventArgs : EventArgs
    {
        public PlayerWonEventArgs(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; }
    }
}
