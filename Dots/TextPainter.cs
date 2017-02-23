using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots
{
    class TextPainter : IGameFieldPainter
    {
        public void Paint(GameField gameField)
        {
            var output = new StringBuilder();

            for (int i = 0; i <= gameField.Field.Count; i++)
            {
                Console.Write($"{i,8}");
            }
            Console.WriteLine();
            Console.WriteLine();

            int count = 1;
            foreach (var row in gameField.Field)
            {
                Console.Write($"{count++,8}");
                row.ForEach(dot =>
                {
                    var dotInfo = $"{(dot.Active ? "" : "{")}{(dot.Chain ? "[" : "")}{dot.Value}{(dot.Chain ? "]" : "")}{(dot.Active ? "" : "}")}{(dot.Closed ? "*" : "")}";
                    Console.Write($"{dotInfo,8}");
                });
                Console.WriteLine();
            }
        }
    }
}
