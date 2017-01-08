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
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                    if (Field[i][j].Value != 0 && Field[i][j].Active && !Field[i][j].Chain)
                        Field[i][j].Chain = FindChain(Field[i][j], i, j, 1);
        }

        private bool FindChain(Dot initDot, int iCurrent, int jCurrent, int count)
        {
            if (Field[iCurrent][jCurrent] == initDot && count >= 4)
                return true;

            var currentValue = Field[iCurrent][jCurrent].Value;

            if (iCurrent - 1 > 0 && jCurrent - 1 > 0 && currentValue == Field[iCurrent - 1][jCurrent - 1].Value)
            {
                Field[iCurrent - 1][jCurrent - 1].Chain = FindChain(initDot, iCurrent - 1, jCurrent - 1, count + 1);
                return Field[iCurrent - 1][jCurrent - 1].Chain;
            }
            if (iCurrent - 1 > 0 && currentValue == Field[iCurrent - 1][jCurrent].Value)
            {
                Field[iCurrent - 1][jCurrent].Chain = FindChain(initDot, iCurrent - 1, jCurrent, count + 1);
                return Field[iCurrent - 1][jCurrent].Chain;
            }
            if (jCurrent - 1 > 0 && currentValue == Field[iCurrent][jCurrent - 1].Value)
            {
                Field[iCurrent][jCurrent - 1].Chain = FindChain(initDot, iCurrent, jCurrent - 1, count + 1);
                return Field[iCurrent][jCurrent - 1].Chain;
            }

            if (iCurrent + 1 < Field.Count && jCurrent + 1 < Field.Count && currentValue == Field[iCurrent + 1][jCurrent + 1].Value)
            {
                Field[iCurrent + 1][jCurrent + 1].Chain = FindChain(initDot, iCurrent + 1, jCurrent + 1, count + 1);
                return Field[iCurrent + 1][jCurrent + 1].Chain;
            }
            if (iCurrent + 1 < Field.Count && currentValue == Field[iCurrent + 1][jCurrent].Value)
            {
                Field[iCurrent + 1][jCurrent].Chain = FindChain(initDot, iCurrent + 1, jCurrent, count + 1);
                return Field[iCurrent + 1][jCurrent].Chain;
            }
            if (jCurrent + 1 > 0 && currentValue == Field[iCurrent][jCurrent + 1].Value)
            {
                Field[iCurrent][jCurrent + 1].Chain = FindChain(initDot, iCurrent, jCurrent + 1, count + 1);
                return Field[iCurrent][jCurrent + 1].Chain;
            }

            return false;
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
