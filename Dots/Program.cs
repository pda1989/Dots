using System;
using Dots.Core.Game;

namespace Dots
{
    internal static class Program
    {
        #region Methods

        private static void Main()
        {
            const int size = 10;

            var game = new Game(new ConsolePainter());
            game.Initialyze(size);
            Console.WriteLine($"Score {game.Result.FirstPlayerScore}:{game.Result.SecondPlayerScore}");
            Console.WriteLine("Field");
            game.Paint();

            while (true)
            {
                Console.Write($"{(game.FirstPlayerMove ? 1 : 2)} > ");
                string command = Console.ReadLine();

                if (command != null && command.ToLower() == "exit") break;

                if (command != null && command.ToLower() == "clear")
                    game.Initialyze(size);

                if (command != null && command.ToLower() == "debug")
                    try
                    {
                        game.MakeMove(2, 3);
                        game.MakeMove(3, 3);
                        game.MakeMove(3, 2);
                        game.MakeMove(1, 3);
                        game.MakeMove(3, 4);
                        game.MakeMove(2, 2);
                        game.MakeMove(4, 3);
                        game.MakeMove(2, 4);
                        game.MakeMove(1, 9);
                        game.MakeMove(3, 1);
                        game.MakeMove(2, 9);
                        game.MakeMove(3, 5);
                        game.MakeMove(3, 9);
                        game.MakeMove(4, 2);
                        game.MakeMove(4, 9);
                        game.MakeMove(4, 4);
                        game.MakeMove(5, 9);
                        game.MakeMove(5, 3);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }

                var words = command?.Trim().Split(' ');
                if (words?.Length == 2)
                {
                    if (int.TryParse(words[0], out int i) && int.TryParse(words[1], out int j))
                        try
                        {
                            game.MakeMove(i - 1, j - 1);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }
                }

                Console.Clear();
                Console.WriteLine($"Score {game.Result.FirstPlayerScore}:{game.Result.SecondPlayerScore}");
                Console.WriteLine("Field");
                game.Paint();
            }
        }

        #endregion
    }
}