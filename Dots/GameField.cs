using System;
using System.Collections.Generic;
using System.Text;

namespace Dots
{
    public class GameField : IGameField
    {
        static private readonly byte Dot1 = 1;
        static private readonly byte Dot2 = 2;

        public List<List<Dot>> Field { get; set; }
        public bool FirstMove { get; set; }

        public GameField(int size)
        {
            Field = InitialyzeField(size);
            FirstMove = true;
        }

        public List<List<Dot>> InitialyzeField(int size)
        {
            List<List<Dot>> field = new List<List<Dot>>();

            for (int i = 0; i < size; i++)
            {
                var row = new List<Dot>();
                for (int j = 0; j < size; j++)
                    row.Add(new Dot { Value = 0, Chain = false, Active = true });
                field.Add(row);
            }

            return field;
        }

        public bool MakeMove(int i, int j)
        {
            if (i < 0 || i >= Field.Count || j < 0 || j >= Field[i].Count)
                throw new ArgumentOutOfRangeException();

            if (Field[i][j].Value == 0)
            {
                Field[i][j].Value = FirstMove ? Dot1 : Dot2;
                FirstMove = !FirstMove;
                return true;
            }
            else
                return false;
        }

        public void CheckChains()
        {

        }

        public override string ToString()
        {
            var output = new StringBuilder();
            foreach (var row in Field)
            {
                row.ForEach(dot => output.Append($"{dot.Value, 3}{(dot.Chain ? "-" : "")}{(dot.Active ? "" : "*")} "));
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}
