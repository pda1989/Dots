using System;
using System.Collections.Generic;
using System.Linq;

namespace Dots
{
    public class Game
    {
        static public readonly byte FirstPlayerDot = 1;
        static public readonly byte SecondPlayerDot = 2;


        public List<List<Dot>> Field { get; private set; }
        public bool IsFirstMove { get; private set; }
        public (int FirstPlayer, int SecondPlayer) Result {
            get
            {
                int dotFirstCount = 0;
                int dotSecondCount = 0;
                foreach (var row in Field)
                {
                    dotFirstCount += row.Count(dot => dot.Value == FirstPlayerDot && !dot.Active);
                    dotSecondCount += row.Count(dot => dot.Value == SecondPlayerDot && !dot.Active);
                }
                return (dotSecondCount, dotFirstCount);
            }
        }


        public Game(int fieldSize)
        {
            Initialyze(fieldSize);
            IsFirstMove = true;
        }

        public void Initialyze(int fieldSize)
        {
            Field = new List<List<Dot>>();

            for (int i = 0; i < fieldSize; i++)
            {
                var row = new List<Dot>();
                for (int j = 0; j < fieldSize; j++)
                    row.Add(new Dot());
                Field.Add(row);
            }

            IsFirstMove = true;
        }

        public List<List<Dot>> CloneField()
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

        public void Paint(IGameFieldPainter painter)
        {
            painter.Paint(this);
        }

        public bool MakeMove(int i, int j)
        {
            if (i < 0 || i >= Field.Count || j < 0 || j >= Field[i].Count)
                throw new ArgumentOutOfRangeException();

            if (Field[i][j].Value == 0)
            {
                Field[i][j].Value = IsFirstMove ? FirstPlayerDot : SecondPlayerDot;
                Field[i][j].Active = true;
                Field[i][j].Chain = false;
                Field[i][j].Closed = false;
                IsFirstMove = !IsFirstMove;
                return true;
            }
            else
                return false;
        }

        public void FinishMove()
        {
            CheckChains(IsFirstMove ? SecondPlayerDot : FirstPlayerDot);
            CheckChains(IsFirstMove ? FirstPlayerDot : SecondPlayerDot);
        }

        public void CheckChains(byte priorityDot)
        {
            CalculateClosedDots(priorityDot);
            for (int i = 0; i < Field.Count; i++)
                for (int j = 0; j < Field.Count; j++)
                    if (Field[i][j].Closed && Field[i][j].Active)
                    {
                        var oldField = CloneField();
                        bool isEficientChain = false;
                        var isChain = FindChain(i, j, priorityDot, ref isEficientChain);
                        if (!isChain || !isEficientChain)
                            Field = oldField;
                    }
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

        private bool FindChain(int i, int j, byte dotValue, ref bool isEficientChain)
        {
            Field[i][j].Active = Field[i][j].Value == dotValue;
            Field[i][j].Chain = false;

            if (!isEficientChain)
                isEficientChain = Field[i][j].Value != 0 && Field[i][j].Value != dotValue;

            // Top
            bool topChain = true;
            if (i != 0)
            {
                if (Field[i - 1][j].Value != dotValue && !Field[i - 1][j].Closed)
                    return false;
                if (Field[i - 1][j].Value == dotValue && Field[i - 1][j].Active)
                    Field[i - 1][j].Chain = true;
                if (!Field[i - 1][j].Active && Field[i - 1][j].Value == dotValue)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
                if (Field[i - 1][j].Closed && Field[i - 1][j].Active)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
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
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain);
                if (Field[i + 1][j].Closed && Field[i + 1][j].Active)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain); 
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
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain);
                if (Field[i][j - 1].Closed && Field[i][j - 1].Active)
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain); 
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
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain);
                if (Field[i][j + 1].Closed && Field[i][j + 1].Active)
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain); 
            }

            return topChain && bottomChain && leftChain && rightChain;
        }
    }
}
