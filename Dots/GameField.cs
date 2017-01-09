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
                    if ((i == 0 || i == Field.Count - 1 || j == 0 || j == Field.Count - 1) && Field[i][j].Value != 0)
                    {
                        // Check corners
                        if (i == 0 && j == 0)
                            Field[i][j].Closed = (Field[0][1].Value != 0 && 
                                                  Field[0][1].Value != Field[i][j].Value && 
                                                  Field[1][0].Value != 0 &&
                                                  Field[1][0].Value != Field[i][j].Value);
                        if (i == 0 && j == Field.Count - 1)
                            Field[i][j].Closed = (Field[0][Field.Count - 2].Value != 0 &&
                                                  Field[0][Field.Count - 2].Value != Field[i][j].Value &&
                                                  Field[1][Field.Count - 1].Value != 0 &&
                                                  Field[1][Field.Count - 1].Value != Field[i][j].Value);
                        if (i == Field.Count - 1 && j == 0)
                            Field[i][j].Closed = (Field[Field.Count - 1][1].Value != 0 &&
                                                  Field[Field.Count - 1][1].Value != Field[i][j].Value &&
                                                  Field[Field.Count - 2][0].Value != 0 &&
                                                  Field[Field.Count - 2][0].Value != Field[i][j].Value);
                        if (i == Field.Count - 1 && j == Field.Count - 1)
                            Field[i][j].Closed = (Field[Field.Count - 1][Field.Count - 2].Value != 0 &&
                                                  Field[Field.Count - 1][Field.Count - 2].Value != Field[i][j].Value &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value != 0 &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value != Field[i][j].Value);

                        // Check sides
                        if ((i == 0 || i == Field.Count - 1) && (j != 0 && j != Field.Count - 1))
                        {
                            bool closedSide = false;
                            if (i == 0)
                                for (int k = 1; k < Field.Count; k++)
                                {
                                    if (Field[k][j].Value != 0 && Field[k][j].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                    if (k == Field.Count - 1 &&
                                        Field[k][j - 1].Value != 0 &&
                                        Field[k][j - 1].Value != Field[i][j].Value &&
                                        Field[k][j + 1].Value != 0 &&
                                        Field[k][j + 1].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                }
                            else
                                for (int k = Field.Count - 2; k >= 0; k--)
                                {
                                    if (Field[k][j].Value != 0 && Field[k][j].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                    if (k == 0 &&
                                        Field[k][j - 1].Value != 0 &&
                                        Field[k][j - 1].Value != Field[i][j].Value &&
                                        Field[k][j + 1].Value != 0 &&
                                        Field[k][j + 1].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                }
                            Field[i][j].Closed = (Field[i][j - 1].Value != 0 &&
                                                  Field[i][j - 1].Value != Field[i][j].Value &&
                                                  Field[i][j + 1].Value != 0 &&
                                                  Field[i][j + 1].Value != Field[i][j].Value &&
                                                  closedSide);
                        }
                        if ((j == 0 || j == Field.Count - 1) && (i != 0 && i != Field.Count - 1))
                        {
                            bool closedSide = false;
                            if (i == 0)
                                for (int k = 1; k < Field.Count; k++)
                                {
                                    if (Field[i][k].Value != 0 && Field[i][k].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                    if (k == Field.Count - 1 &&
                                        Field[i - 1][k].Value != 0 &&
                                        Field[i - 1][k].Value != Field[i][j].Value &&
                                        Field[i + 1][k].Value != 0 &&
                                        Field[i + 1][k].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                }
                            else
                                for (int k = Field.Count - 2; k >= 0; k--)
                                {
                                    if (Field[i][k].Value != 0 && Field[i][k].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                    if (k == 0 &&
                                        Field[i - 1][k].Value != 0 &&
                                        Field[i - 1][k].Value != Field[i][j].Value &&
                                        Field[i + 1][k].Value != 0 &&
                                        Field[i + 1][k].Value != Field[i][j].Value)
                                    {
                                        closedSide = true;
                                        break;
                                    }
                                }
                            Field[i][j].Closed = (Field[i - 1][j].Value != 0 &&
                                                  Field[i - 1][j].Value != Field[i][j].Value &&
                                                  Field[i + 1][j].Value != 0 &&
                                                  Field[i + 1][j].Value != Field[i][j].Value &&
                                                  closedSide);
                        }
                    }
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
                row.ForEach(dot =>
                {
                    string value = $"{dot.Value}{(dot.Chain ? "-" : "")}{(dot.Active ? "" : "*")}{(dot.Closed ? "_" : "")} ";
                    output.Append($"{value, 6}");
                });
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}
