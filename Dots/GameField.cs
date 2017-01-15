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
                Field[i][j].Active = true;
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
                    {
                        if (Field[i][j].Closed && Field[i][j].Active)
                        {
                            var oldField = Clone();
                            var isChain = FindChain(i, j, FirstMove ? Dot2 : Dot1);
                            if (!isChain) Field = oldField;
                        }
                    }

            CalculateClosedDots(FirstMove ? Dot1 : Dot2);

            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                {
                    if (Field[i][j].Closed && Field[i][j].Active)
                    {
                        var oldField = Clone();
                        var isChain = FindChain(i, j, FirstMove ? Dot1 : Dot2);
                        if (!isChain) Field = oldField;
                    }
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
                    output.Append($"{value, 8}");
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
                            Field[i][j].Closed = (Field[0][1].Value == dotValue &&
                                                  Field[1][0].Value == dotValue);
                            continue;
                        }
                        if (i == 0 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[0][Field.Count - 2].Value == dotValue &&
                                                  Field[1][Field.Count - 1].Value == dotValue);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == 0)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][1].Value == dotValue &&
                                                  Field[Field.Count - 2][0].Value == dotValue);
                            continue;
                        }
                        if (i == Field.Count - 1 && j == Field.Count - 1)
                        {
                            Field[i][j].Closed = (Field[Field.Count - 1][Field.Count - 2].Value == dotValue &&
                                                  Field[Field.Count - 2][Field.Count - 1].Value == dotValue);
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
                                    Field[k][j - 1].Value == dotValue &&
                                    Field[k][j + 1].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            Field[i][j].Closed = (Field[i][j - 1].Value == dotValue &&
                                                  Field[i][j + 1].Value == dotValue &&
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
                                    Field[i - 1][k].Value == dotValue &&
                                    Field[i + 1][k].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            Field[i][j].Closed = (Field[i - 1][j].Value == dotValue &&
                                                  Field[i + 1][j].Value == dotValue &&
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
                        if ((Field[k][j].Value == dotValue) ||
                            (k == 0 && Field[k][j].Closed))
                        {
                            topClosed = true;
                            break;
                        }
                    // Bottom
                    bool bottomClosed = false;
                    for (int k = i + 1; k < Field.Count; k++)
                        if ((Field[k][j].Value == dotValue) ||
                            (k == Field.Count - 1 && Field[k][j].Closed))
                        {
                            bottomClosed = true;
                            break;
                        }
                    // Left
                    bool leftClosed = false;
                    for (int k = j - 1; k >= 0; k--)
                        if ((Field[i][k].Value == dotValue) ||
                            (k == 0 && Field[i][k].Closed))
                        {
                            leftClosed = true;
                            break;
                        }
                    // Right
                    bool rightClosed = false;
                    for (int k = j + 1; k < Field.Count; k++)
                        if ((Field[i][k].Value == dotValue) ||
                            (k == Field.Count - 1 && Field[i][k].Closed))
                        {
                            rightClosed = true;
                            break;
                        }
                    Field[i][j].Closed = (topClosed && bottomClosed && leftClosed && rightClosed);
                }
        }

        private bool FindChain(int i, int j, byte dotValue)
        {
            Field[i][j].Active = false;

            // Top
            bool topChain = true;
            if ((i != 0 && Field[i - 1][j].Value != dotValue && !Field[i - 1][j].Closed) ||
                (i != 0 && !Field[i - 1][j].Active && Field[i - 1][j].Value == dotValue))
                return false;
            if (i != 0 && Field[i - 1][j].Value == dotValue && Field[i - 1][j].Active)
                Field[i - 1][j].Chain = true;
            if (i != 0 && Field[i - 1][j].Closed && Field[i - 1][j].Active)
                topChain = FindChain(i - 1, j, dotValue);

            // Bottom
            bool bottomChain = true;
            if ((i != Field.Count - 1 && Field[i + 1][j].Value != dotValue && !Field[i + 1][j].Closed) ||
                (i != Field.Count - 1 && !Field[i + 1][j].Active && Field[i + 1][j].Value == dotValue))
                return false;
            if (i != Field.Count - 1 && Field[i + 1][j].Value == dotValue && Field[i + 1][j].Active)
                Field[i + 1][j].Chain = true;
            if (i != Field.Count - 1 && Field[i + 1][j].Closed && Field[i + 1][j].Active)
                bottomChain = FindChain(i + 1, j, dotValue);

            // Left
            bool leftChain = true;
            if ((j != 0 && Field[i][j - 1].Value != dotValue && !Field[i][j - 1].Closed) ||
                (j != 0 && !Field[i][j - 1].Active && Field[i][j - 1].Value == dotValue))
                return false;
            if (j != 0 && Field[i][j - 1].Value == dotValue && Field[i][j - 1].Active)
                Field[i][j - 1].Chain = true;
            if (j != 0 && Field[i][j - 1].Closed && Field[i][j - 1].Active)
                leftChain = FindChain(i, j - 1, dotValue);

            // Right
            bool rightChain = true;
            if ((j != Field.Count - 1 && Field[i][j + 1].Value != dotValue && !Field[i][j + 1].Closed) ||
                (j != Field.Count - 1 && !Field[i][j + 1].Active && Field[i][j + 1].Value == dotValue))
                return false;
            if (j != Field.Count - 1 && Field[i][j + 1].Value == dotValue && Field[i][j + 1].Active)
                Field[i][j + 1].Chain = true;
            if (j != Field.Count - 1 && Field[i][j + 1].Closed && Field[i][j + 1].Active)
                rightChain = FindChain(i, j + 1, dotValue);

            return topChain && bottomChain && leftChain && rightChain;
        }
    }
}
