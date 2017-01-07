﻿using System;
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
            var game = new GameField(10);
            Console.WriteLine("Field");
            Console.WriteLine(game.ToString());

            while (true)
            {
                Console.Write(" > ");
                string command = Console.ReadLine();

                if (command.ToLower() == "exit") break;

                var words = command.Trim().Split(' ');
                if (words.Length == 2)
                {
                    int i, j;
                    if (int.TryParse(words[0], out i) && int.TryParse(words[1], out j))
                        try
                        {
                            if (game.MakeMove(i, j))
                                game.CheckChains();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                }

                Console.Clear();
                Console.WriteLine("Field");
                Console.WriteLine(game.ToString());
            }
        }
    }
}