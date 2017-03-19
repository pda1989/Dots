using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 10;

            var game = new Game(size);
            Console.WriteLine($"Score {game.Result.FirstPlayer}:{game.Result.SecondPlayer}");
            Console.WriteLine("Field");
            game.Paint(new TextPainter());

            while (true)
            {
                Console.Write($"{(game.IsFirstMove ? Game.FirstPlayerDot : Game.SecondPlayerDot)} > ");
                string command = Console.ReadLine();

                if (command.ToLower() == "exit") break;

                if (command.ToLower() == "clear")
                    game.Initialyze(size);

                if (command.ToLower() == "debug")
                {
                    try
                    {
                        if (game.MakeMove(2, 3)) game.FinishMove();
                        if (game.MakeMove(3, 3)) game.FinishMove();
                        if (game.MakeMove(3, 2)) game.FinishMove();
                        if (game.MakeMove(1, 3)) game.FinishMove();
                        if (game.MakeMove(3, 4)) game.FinishMove();
                        if (game.MakeMove(2, 2)) game.FinishMove();
                        if (game.MakeMove(4, 3)) game.FinishMove();
                        if (game.MakeMove(2, 4)) game.FinishMove();
                        if (game.MakeMove(1, 9)) game.FinishMove();
                        if (game.MakeMove(3, 1)) game.FinishMove();
                        if (game.MakeMove(2, 9)) game.FinishMove();
                        if (game.MakeMove(3, 5)) game.FinishMove();
                        if (game.MakeMove(3, 9)) game.FinishMove();
                        if (game.MakeMove(4, 2)) game.FinishMove();
                        if (game.MakeMove(4, 9)) game.FinishMove();
                        if (game.MakeMove(4, 4)) game.FinishMove();
                        if (game.MakeMove(5, 9)) game.FinishMove();
                        if (game.MakeMove(5, 3)) game.FinishMove();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                }

                var words = command.Trim().Split(' ');
                if (words.Length == 2)
                {
                    int i, j;
                    if (int.TryParse(words[0], out i) && int.TryParse(words[1], out j))
                        try
                        {
                            if (game.MakeMove(i - 1, j - 1))
                                game.FinishMove();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }
                }

                Console.Clear();
                Console.WriteLine($"Score {game.Result.FirstPlayer}:{game.Result.SecondPlayer}");
                Console.WriteLine("Field");
                game.Paint(new TextPainter());
            }
        }
    }
}
