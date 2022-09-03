using SnakesAndLadders.Contracts;
using SnakesAndLadders.Enums;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Extensions;
using SnakesAndLadders.Models;
using SnakesAndLaddersUI;
using System.Reflection;
using Unity;

namespace SnakesAndLadders
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            container.RegisterDefaultServices();

            var program = new NewGameStarter(container.Resolve<IGame>());

            program.ShowHeader();

            do
            {
                program.ShowListOfCommands();
            } while (true);
        }
    }
}