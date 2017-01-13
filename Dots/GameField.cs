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
            Initialyze(size);
            FirstMove = true;
        }

        public void Initialyze(int size)
        {
            Field = new List<List<Dot>>();

            for (int i = 0; i < size; i++)
            {
                var row = new List<Dot>();
                for (int j = 0; j < size; j++)
                    row.Add(new Dot { Value = 0, Chain = false, Active = true });
                Field.Add(row);
            }
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
            // Determine closed dots on sides
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                    if ((i == 0 || i == Field.Count - 1 || j == 0 || j == Field.Count - 1) && Field[i][j].Value != 0)
                    {
                        // Check corners
                        if (i == 0 && j == 0)
                        {
                            Field[i][j].Closed = (Field[0][1].Value != 0 && 
                                                  Field[0][1].Value != Field[i][j].Value && 
                                                  Field[1][0].Value != 0 &&
                                                  Field[1][0].Value != Field[i][j].Value);
                            continue;
                        }
                        if (i == 0 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[0][Field.Count - 2].Value != 0 &&
                                                  Field[0][Field.Count - 2].Value != Field[i][j].Value &&
                                                  Field[1][Field.Count - 1].Value != 0 &&
                                                  Field[1][Field.Count - 1].Value != Field[i][j].Value);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == 0)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][1].Value != 0 &&
                                                  Field[Field.Count - 1][1].Value != Field[i][j].Value &&
                                                  Field[Field.Count - 2][0].Value != 0 &&
                                                  Field[Field.Count - 2][0].Value != Field[i][j].Value);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][Field.Count - 2].Value != 0 &&
                                                  Field[Field.Count - 1][Field.Count - 2].Value != Field[i][j].Value &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value != 0 &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value != Field[i][j].Value);
                            continue;
                        }

                        // Check sides
                        // Top and bottom rows
                        if (i == 0 || i == Field.Count - 1)
                        {
                            bool closedSide = false;
                            int begin = 0, end = 0, step = 0;

                            if (i == 0)
                                { begin = 1; end = Field.Count - 1; step = 1; }
                            else
                                { begin = Field.Count - 2; end = 0; step = -1; }

                            for (int k = begin; (i == 0 ? k <= end : k >= end); k += step)
                            {
                                if (Field[k][j].Value != 0 && Field[k][j].Value != Field[i][j].Value)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == Field.Count - 1) &&
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
                        //Left and right columns
                        if (j == 0 || j == Field.Count - 1)
                        {
                            bool closedSide = false;
                            int begin = 0, end = 0, step = 0;

                            if (j == 0)
                                { begin = 1; end = Field.Count - 1; step = 1; }
                            else
                                { begin = Field.Count - 2; end = 0; step = -1; }

                            for (int k = begin; (j == 0 ? k <= end : k >= end); k += step)
                            {
                                if (Field[i][k].Value != 0 && Field[i][k].Value != Field[i][j].Value)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == Field.Count - 1) &&
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

            // Determine closed dots
            for (int i = 1; i < Field.Count - 1; i++)
                for (int j = 1; j < Field.Count - 1; j++)
                {
                    if (Field[i][j].Value == 0) continue;

                    // Top
                    bool topClosed = false;
                    for (int k = i - 1; k >= 0; k--)
                        if ((Field[k][j].Value != 0 && Field[k][j].Value != Field[i][j].Value) ||
                            (k == 0 && Field[k][j].Value == Field[i][j].Value && Field[k][j].Closed))
                        {
                            topClosed = true;
                            break;
                        }
                    // Bottom
                    bool bottomClosed = false;
                    for (int k = i + 1; k < Field.Count; k++)
                        if ((Field[k][j].Value != 0 && Field[k][j].Value != Field[i][j].Value) ||
                            (k == Field.Count - 1 && Field[k][j].Value == Field[i][j].Value && Field[k][j].Closed))
                        {
                            bottomClosed = true;
                            break;
                        }
                    // Left
                    bool leftClosed = false;
                    for (int k = j - 1; k >= 0; k--)
                        if ((Field[i][k].Value != 0 && Field[i][k].Value != Field[i][j].Value) ||
                            (k == 0 && Field[i][k].Value == Field[i][j].Value && Field[i][k].Closed))
                        {
                            leftClosed = true;
                            break;
                        }
                    // Right
                    bool rightClosed = false;
                    for (int k = j + 1; k < Field.Count; k++)
                        if ((Field[i][k].Value != 0 && Field[i][k].Value != Field[i][j].Value) ||
                            (k == Field.Count - 1 && Field[i][k].Value == Field[i][j].Value && Field[i][k].Closed))
                        {
                            rightClosed = true;
                            break;
                        }
                    Field[i][j].Closed = (topClosed && bottomClosed && leftClosed && rightClosed);
                }

            // Calculate chains and set states for dots
            for (int i = 1; i < Field.Count - 1; i++)
                for (int j = 1; j < Field.Count - 1; j++)
                {
                    var oldField = Clone();
                    if (Field[i][j].Closed && Field[i][j].Active)
                        Field = oldField;
                }
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            foreach (var row in Field)
            {
                row.ForEach(dot =>
                {
                    string value = $"{(dot.Active ? "" : "{")}{(dot.Chain ? "[" : "")}{dot.Value}{(dot.Chain ? "]" : "")}{(dot.Active ? "" : "}")}{(dot.Closed ? "*" : "")} ";
                    output.Append($"{value, 6}");
                });
                output.AppendLine();
            }
            return output.ToString();
        }

        public List<List<Dot>> Clone()
        {
            var newField = new List<List<Dot>>();
            foreach (var row in Field)
            {
                var newRow = new List<Dot>();
                foreach (var dot in row)
                    newRow.Add(dot.Clone());
                newField.Add(newRow);
            }
            return newField;
        }
    }
}
