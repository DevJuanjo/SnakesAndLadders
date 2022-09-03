using SnakesAndLadders.Contracts;
using SnakesAndLadders.Enums;
using SnakesAndLadders.Exceptions;
using SnakesAndLadders.Models;
using SnakesAndLaddersUI.Collections;
using SnakesAndLaddersUI.Models;
using System.Reflection;

namespace SnakesAndLaddersUI
{
    public class NewGameStarter
    {
        private readonly IGame game;
        private readonly ProgramCommands commands = new ProgramCommands();
        private readonly GameStatusConsoleCollection statusConsole = new GameStatusConsoleCollection();
        public static string? winner;

        public NewGameStarter(IGame game)
        {
            this.game = game;

            this.game.OnPlayerWins += Game_OnPlayerWins;

            commands.Add(new ProgramCommand(1, "Start a new game."), () => StartNewGame());
            commands.Add(new ProgramCommand(2, "Show list of players."), () => ShowListOfPlayers());
            commands.Add(new ProgramCommand(3, "Add a new player."), () => AddNewPlayer());
            commands.Add(new ProgramCommand(4, "Remove a player."), () => RemovePlayer());
            commands.Add(new ProgramCommand(5, "Remove all players."), () => RemoveAllPlayers());
            commands.Add(new ProgramCommand(6, "Exit the game."), () => Environment.Exit(0));
        }

        private static void Game_OnPlayerWins(object? sender, PlayerWonEventArgs e)
        {
            winner = e.Player.Name;
        }

        public void ShowHeader()
        {
            Console.WriteLine("SNAKES AND LADDERS");
            Console.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("For this first version we have no snakes and/or ladders but maybe you are the kind of person who enjoys walking around on an empty board...");
            Console.WriteLine("This game also has local multiplayer. So feel free to invite family and friends to enjoy this unique experience!");
            Console.WriteLine();
            Console.WriteLine("Multiplayer online just available on the premium version.");
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public void ShowListOfCommands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("LIST OF COMMAND");
            Console.ResetColor();
            Console.WriteLine();

            commands.WriteCommandToConsole();

            bool isValidCommand = false;
            Action? commandAction = null;

            do
            {
                Console.Write($"Which step do you want to take next ({commands.Keys.First().Command}-{commands.Keys.Last().Command})?: ");
                var userCommand = Console.ReadLine();

                isValidCommand = commands.TryToGetActionCommand(userCommand, out commandAction);
                if (isValidCommand == false)
                {
                    Console.WriteLine("Not valid Command");
                }

                Console.WriteLine();

            } while (isValidCommand == false);

            commandAction!.Invoke();
        }

        private void StartNewGame()
        {
            game.Reset();
            statusConsole.WriteStatusOnConsole(game.Status);
            Console.WriteLine();

            try
            {
                game.Start();
                statusConsole.WriteStatusOnConsole(game.Status);
            }
            catch(SnakesAndLaddersBaseException ex)
            {
                Console.WriteLine($"Ups, something wrong happened. Message = {ex.Message}");
                return;
            }
            finally
            {
                Console.WriteLine();
            }

            do
            {
                Play(game);
            } while (game.Status == GameStatus.InProgress);

            if (winner != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{winner} has won the game!! Congratulations!!");
                Console.ResetColor();
                Console.WriteLine();
                winner = null;
                FinishTheGame(game);
            }
        }

        private void ShowListOfPlayers()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("PLAYERS");
            Console.ResetColor();

            if(game.GetPlayers().Any() == false)
            {
                Console.WriteLine("No player has been registered for the game.");
                Console.WriteLine();
                return;
            }

            foreach (var player in game.GetPlayers())
            {
                Console.WriteLine($"- {player.Name}");
            }

            Console.WriteLine();
        }

        private void AddNewPlayer()
        {
            string? playerName;

            do
            {
                Console.Write($"Give me the name of the Player: ");
                playerName = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(playerName));

            try
            {
                game.AddPlayer(new DefaultPlayer(playerName));

                Console.WriteLine($"Player {playerName} successfully added");
            }
            catch (SnakesAndLaddersPlayerException ex)
            {
                Console.WriteLine($"Ups, something wrong happened. Message = {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }

        private void RemovePlayer()
        {
            string? playerName;

            do
            {
                Console.Write($"Give me the name of the Player to remove: ");
                playerName = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(playerName));

            try
            {
                if(game.GetPlayer(playerName) == null)
                {
                    Console.WriteLine("Player not found.");
                    return;
                }

                game.RemovePlayer(playerName);
                Console.WriteLine($"Player {playerName} successfully removed");
            }
            catch (SnakesAndLaddersPlayerException ex)
            {
                Console.WriteLine($"Ups, something wrong happened. Message = {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }

        private void RemoveAllPlayers()
        {
            string? userDesition;

            do
            {
                Console.Write($"Remove all the players? (y/n): ");
                userDesition = Console.ReadLine();

            } while (userDesition != "y" && userDesition != "n");

            try
            {
                if(userDesition == "n")
                {
                    return;
                }

                var playerNames = game.GetPlayers().Select(x => x.Name).ToList();

                foreach (var player in playerNames)
                {
                    game.RemovePlayer(player);
                }

                Console.WriteLine("Players succesfully removed.");
            }
            catch (SnakesAndLaddersPlayerException ex)
            {
                Console.WriteLine($"Ups, something wrong happened. Message: {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }

        private void Play(IGame game)
        {
            ConsoleKey readKey;

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"Please press ENTER to keep playing or ESC to finish the game.");
                Console.ResetColor();

                readKey = Console.ReadKey().Key;

            } while (readKey != ConsoleKey.Enter &&
                readKey != ConsoleKey.Escape);

            Console.WriteLine();
            Console.WriteLine();

            switch (readKey)
            {
                case ConsoleKey.Enter:
                    PlayNextTurn(game);
                    break;
                case ConsoleKey.Escape:
                    FinishTheGame(game);
                    break;
            }
        }

        private void PlayNextTurn(IGame game)
        {
            var playerWithTurn = game.GetPlayerWithTurn();
            var playerWithTurnPosition = playerWithTurn!.Position;

            Console.WriteLine($"Player {playerWithTurn.Name} has the turn.");
            Console.WriteLine($"Player is on position: {playerWithTurnPosition}");

            game.ExecuteNextTurn();

            Console.WriteLine($"Dice roll result is: {game.GetDice()!.RollResult}");

            if (playerWithTurnPosition != playerWithTurn.Position)
            {
                Console.WriteLine($"Player move from position {playerWithTurnPosition} to position {playerWithTurn.Position}.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine($"Player will not move. Final position out of the board's limits.");
            Console.WriteLine();
        }

        private void FinishTheGame(IGame game)
        {
            if (game.Status == GameStatus.InProgress)
            {
                game.Finish();
            }

            statusConsole.WriteStatusOnConsole(game.Status);
            Console.WriteLine();
        }
    }
}
