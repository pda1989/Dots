using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots
{
    class TextPainter : IGameFieldPainter
    {
        public void Paint(Field gameField)
        {
            var output = new StringBuilder();

            for (int i = 0; i <= gameField.Size; i++)
            {
                Console.Write($"{i,8}");
            }
            Console.WriteLine();
            Console.WriteLine();

            int count = 1;
            for (int i = 0; i< gameField.Size; i++)
            {
                Console.Write($"{count++,8}");
                for (int j = 0; j < gameField.Size; j++)
                {
                    var dot = gameField[i][j];
                    var dotInfo = $"{(dot.Active ? "" : "{")}{(dot.Chain ? "[" : "")}{dot.Value}{(dot.Chain ? "]" : "")}{(dot.Active ? "" : "}")}{(dot.Closed ? "*" : "")}";
                    Console.Write($"{dotInfo,8}");
                };
                Console.WriteLine();
            }
        }
    }
}
