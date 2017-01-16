using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dots
{
    public class GameField : IGameField
    {
        static public readonly byte Dot1 = 1;
        static public readonly byte Dot2 = 2;


        public List<List<Dot>> Field { get; private set; }
        public bool FirstMove { get; private set; }
        public string Result {
            get
            {
                int dot1Count = 0;
                int dot2Count = 0;
                foreach (List<Dot> row in Field)
                {
                    dot1Count += row.Count(dot => dot.Value == Dot1 && !dot.Active);
                    dot2Count += row.Count(dot => dot.Value == Dot2 && !dot.Active);
                }
                return $"{dot2Count} : {dot1Count}";
            }
        }


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
                    row.Add(new Dot { Value = 0, Chain = false, Active = true, Closed = false });
                Field.Add(row);
            }

            FirstMove = true;
        }

        public bool MakeMove(int i, int j)
        {
            if (i < 0 || i >= Field.Count || j < 0 || j >= Field[i].Count)
                throw new ArgumentOutOfRangeException();

            if (Field[i][j].Value == 0)
            {
                Field[i][j].Value = FirstMove ? Dot1 : Dot2;
                Field[i][j].Active = true;
                Field[i][j].Chain = false;
                Field[i][j].Closed = false;
                FirstMove = !FirstMove;
                return true;
            }
            else
                return false;
        }

        public void CheckChains()
        {
            CalculateClosedDots(FirstMove ? Dot2 : Dot1);
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                    if (Field[i][j].Closed && Field[i][j].Active)
                    {
                        var oldField = Clone();
                        bool isTrueChain = false;
                        var isChain = FindChain(i, j, FirstMove ? Dot2 : Dot1, ref isTrueChain);
                        if (!isChain || !isTrueChain)
                            Field = oldField;
                    }

            CalculateClosedDots(FirstMove ? Dot1 : Dot2);
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                    if (Field[i][j].Closed && Field[i][j].Active)
                    {
                        var oldField = Clone();
                        bool isTrueChain = false;
                        var isChain = FindChain(i, j, FirstMove ? Dot1 : Dot2, ref isTrueChain);
                        if (!isChain || !isTrueChain)
                            Field = oldField;
                    }
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

        public override string ToString()
        {
            var output = new StringBuilder();

            for (int i = 0; i <= Field.Count; i++)
            {
                output.Append($"{i, 8}");
            }
            output.AppendLine();
            output.AppendLine();

            int count = 1;
            foreach (var row in Field)
            {
                output.Append($"{count++, 8}");
                row.ForEach(dot =>
                {
                    string value = $"{(dot.Active ? "" : "{")}{(dot.Chain ? "[" : "")}{dot.Value}{(dot.Chain ? "]" : "")}{(dot.Active ? "" : "}")}{(dot.Closed ? "*" : "")}";
                    output.Append($"{value, 8}");
                });
                output.AppendLine();
            }
            return output.ToString();
        }


        private void CalculateClosedDots(byte dotValue)
        {
            // Determine closed dots on sides
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                {
                    if (Field[i][j].Value == dotValue)
                    {
                        Field[i][j].Closed = false;
                        continue;
                    }

                    if (i == 0 || i == Field.Count - 1 || j == 0 || j == Field.Count - 1)
                    {
                        // Check corners
                        if (i == 0 && j == 0)
                        {
                            Field[i][j].Closed = (Field[0][1].Value == dotValue && Field[0][1].Active &&
                                                  Field[1][0].Value == dotValue && Field[1][0].Active);
                            continue;
                        }
                        if (i == 0 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[0][Field.Count - 2].Value == dotValue && Field[0][Field.Count - 2].Active &&
                                                  Field[1][Field.Count - 1].Value == dotValue && Field[1][Field.Count - 1].Active);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == 0)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][1].Value == dotValue && Field[Field.Count - 1][1].Active &&
                                                  Field[Field.Count - 2][0].Value == dotValue && Field[Field.Count - 2][0].Active);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][Field.Count - 2].Value == dotValue && Field[Field.Count - 1][Field.Count - 2].Active &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value == dotValue && Field[Field.Count - 2][Field.Count - 1].Active);
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
                                if (Field[k][j].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == Field.Count - 1) &&
                                    Field[k][j - 1].Value == dotValue && Field[k][j - 1].Active &&
                                    Field[k][j + 1].Value == dotValue && Field[k][j + 1].Active)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            Field[i][j].Closed = (Field[i][j - 1].Value == dotValue && Field[i][j - 1].Active &&
                                                  Field[i][j + 1].Value == dotValue && Field[i][j + 1].Active &&
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
                                if (Field[i][k].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == Field.Count - 1) &&
                                    Field[i - 1][k].Value == dotValue && Field[i - 1][k].Active &&
                                    Field[i + 1][k].Value == dotValue && Field[i + 1][k].Active)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            Field[i][j].Closed = (Field[i - 1][j].Value == dotValue && Field[i - 1][j].Active &&
                                                  Field[i + 1][j].Value == dotValue && Field[i + 1][j].Active &&
                                                  closedSide);
                        }
                    }
                }

            // Determine closed dots
            for (int i = 1; i < Field.Count - 1; i++)
                for (int j = 1; j < Field.Count - 1; j++)
                {
                    if (Field[i][j].Value == dotValue) continue;

                    // Top
                    bool topClosed = false;
                    for (int k = i - 1; k >= 0; k--)
                        if ((Field[k][j].Value == dotValue && Field[k][j].Active) ||
                            (k == 0 && Field[k][j].Closed))
                        {
                            topClosed = true;
                            break;
                        }
                    // Bottom
                    bool bottomClosed = false;
                    for (int k = i + 1; k < Field.Count; k++)
                        if ((Field[k][j].Value == dotValue && Field[k][j].Active) ||
                            (k == Field.Count - 1 && Field[k][j].Closed))
                        {
                            bottomClosed = true;
                            break;
                        }
                    // Left
                    bool leftClosed = false;
                    for (int k = j - 1; k >= 0; k--)
                        if ((Field[i][k].Value == dotValue && Field[i][k].Active) ||
                            (k == 0 && Field[i][k].Closed))
                        {
                            leftClosed = true;
                            break;
                        }
                    // Right
                    bool rightClosed = false;
                    for (int k = j + 1; k < Field.Count; k++)
                        if ((Field[i][k].Value == dotValue && Field[i][k].Active) ||
                            (k == Field.Count - 1 && Field[i][k].Closed))
                        {
                            rightClosed = true;
                            break;
                        }
                    Field[i][j].Closed = (topClosed && bottomClosed && leftClosed && rightClosed);
                }
        }

        private bool FindChain(int i, int j, byte dotValue, ref bool isTrueChain)
        {
            Field[i][j].Active = Field[i][j].Value == dotValue;
            Field[i][j].Chain = false;

            if (!isTrueChain)
                isTrueChain = Field[i][j].Value != 0 && Field[i][j].Value != dotValue;

            // Top
            bool topChain = true;
            if (i != 0)
            {
                if (Field[i - 1][j].Value != dotValue && !Field[i - 1][j].Closed)
                    return false;
                if (Field[i - 1][j].Value == dotValue && Field[i - 1][j].Active)
                    Field[i - 1][j].Chain = true;
                if (!Field[i - 1][j].Active && Field[i - 1][j].Value == dotValue)
                    topChain = FindChain(i - 1, j, dotValue, ref isTrueChain);
                if (Field[i - 1][j].Closed && Field[i - 1][j].Active)
                    topChain = FindChain(i - 1, j, dotValue, ref isTrueChain);
            }

            // Bottom
            bool bottomChain = true;
            if (i != Field.Count - 1)
            {
                if (Field[i + 1][j].Value != dotValue && !Field[i + 1][j].Closed)
                    return false;
                if (Field[i + 1][j].Value == dotValue && Field[i + 1][j].Active)
                    Field[i + 1][j].Chain = true;
                if (!Field[i + 1][j].Active && Field[i + 1][j].Value == dotValue)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isTrueChain);
                if (Field[i + 1][j].Closed && Field[i + 1][j].Active)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isTrueChain); 
            }

            // Left
            bool leftChain = true;
            if (j != 0)
            {
                if (Field[i][j - 1].Value != dotValue && !Field[i][j - 1].Closed)
                    return false;
                if (Field[i][j - 1].Value == dotValue && Field[i][j - 1].Active)
                    Field[i][j - 1].Chain = true;
                if (!Field[i][j - 1].Active && Field[i][j - 1].Value == dotValue)
                    leftChain = FindChain(i, j - 1, dotValue, ref isTrueChain);
                if (Field[i][j - 1].Closed && Field[i][j - 1].Active)
                    leftChain = FindChain(i, j - 1, dotValue, ref isTrueChain); 
            }

            // Right
            bool rightChain = true;
            if (j != Field.Count - 1)
            {
                if (Field[i][j + 1].Value != dotValue && !Field[i][j + 1].Closed)
                    return false;
                if (Field[i][j + 1].Value == dotValue && Field[i][j + 1].Active)
                    Field[i][j + 1].Chain = true;
                if (!Field[i][j + 1].Active && Field[i][j + 1].Value == dotValue)
                    rightChain = FindChain(i, j + 1, dotValue, ref isTrueChain);
                if (Field[i][j + 1].Closed && Field[i][j + 1].Active)
                    rightChain = FindChain(i, j + 1, dotValue, ref isTrueChain); 
            }

            return topChain && bottomChain && leftChain && rightChain;
        }
    }
}
