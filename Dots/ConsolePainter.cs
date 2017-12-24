using System;
using Dots.Core.Field;
using Dots.Core.Game;

namespace Dots
{
    internal class ConsolePainter : IGameFieldPainter
    {
        #region Methods

        public void Paint(Field gameField)
        {
            for (var i = 0; i <= gameField.Size; i++) Console.Write($"{i,8}");
            Console.WriteLine();
            Console.WriteLine();

            var count = 1;
            for (var i = 0; i < gameField.Size; i++)
            {
                Console.Write($"{count++,8}");
                for (var j = 0; j < gameField.Size; j++)
                {
                    var dot = gameField[i][j];
                    string dotInfo =
                        $"{(dot.Active ? "" : "{")}{(dot.Chain ? "[" : "")}{dot.Value}{(dot.Chain ? "]" : "")}{(dot.Active ? "" : "}")}{(dot.Closed ? "*" : "")}";
                    Console.Write($"{dotInfo,8}");
                }

                Console.WriteLine();
            }
        }

        #endregion
    }
}