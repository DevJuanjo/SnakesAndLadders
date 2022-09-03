using SnakesAndLadders.Contracts;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;
using Unity;

namespace SnakesAndLadders.Extensions
{
    public static class IUnityContainerExtension
    {
        /// <summary>
        /// Extension to Register the necessary dafault services in a UnityContainer
        /// </summary>
        /// <param name="container"></param>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        public static IUnityContainer RegisterDefaultServices(this IUnityContainer container, int boardSize = 100)
        {
            container.RegisterInstance<IDice>(new DefaultDice());
            container.RegisterInstance<IBoard>(new DefaultBoard(boardSize));
            container.RegisterType<IPlayerService, PlayerService>();
            container.RegisterSingleton<IGame, Game>();

            return container;
        }
    }
}
