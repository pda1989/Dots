using System;
using System.Collections.Generic;
using System.Linq;

namespace Dots
{
    public class Game
    {
        static public readonly byte FirstPlayerDot = 1;
        static public readonly byte SecondPlayerDot = 2;


        public Field GameField { get; private set; }
        public bool IsFirstMove { get; private set; }
        public (int FirstPlayer, int SecondPlayer) Result {
            get
            {
                int dotFirstCount = 0;
                int dotSecondCount = 0;
                for (int i = 0; i < GameField.Size; i++)
                    for (int j = 0; j < GameField.Size; j++)
                    {
                        var dot = GameField[i][j];
                        if (dot.Value == FirstPlayerDot && !dot.Active) dotFirstCount++;
                        if (dot.Value == SecondPlayerDot && !dot.Active) dotSecondCount++;
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
            GameField = new Field(fieldSize);

            IsFirstMove = true;
        }

        public void Paint(IGameFieldPainter painter)
        {
            painter.Paint(GameField);
        }

        public bool MakeMove(int i, int j)
        {
            if (i < 0 || i >= GameField.Size || j < 0 || j >= GameField.Size)
                throw new IndexOutOfRangeException();

            if (GameField[i][j].Value == 0)
            {
                GameField[i][j].Value = IsFirstMove ? FirstPlayerDot : SecondPlayerDot;
                GameField[i][j].Active = true;
                GameField[i][j].Chain = false;
                GameField[i][j].Closed = false;
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
            for (int i = 0; i < GameField.Size; i++)
                for (int j = 0; j < GameField.Size; j++)
                    if (GameField[i][j].Closed && GameField[i][j].Active)
                    {
                        var oldField = GameField.Clone();
                        bool isEficientChain = false;
                        var isChain = FindChain(i, j, priorityDot, ref isEficientChain);
                        if (!isChain || !isEficientChain)
                            GameField = oldField;
                    }
        }


        private void CalculateClosedDots(byte dotValue)
        {
            // Determine closed dots on sides
            for (int i = 0; i < GameField.Size; i++)
                for (int j = 0; j < GameField.Size; j++)
                {
                    if (GameField[i][j].Value == dotValue)
                    {
                        GameField[i][j].Closed = false;
                        continue;
                    }

                    if (i == 0 || i == GameField.Size - 1 || j == 0 || j == GameField.Size - 1)
                    {
                        // Check corners
                        if (i == 0 && j == 0)
                        {
                            GameField[i][j].Closed = (GameField[0][1].Value == dotValue && GameField[0][1].Active &&
                                                  GameField[1][0].Value == dotValue && GameField[1][0].Active);
                            continue;
                        }
                        if (i == 0 && j == GameField.Size - 1)
                        {
                            GameField[i][j].Closed = (GameField[0][GameField.Size - 2].Value == dotValue && GameField[0][GameField.Size - 2].Active &&
                                                  GameField[1][GameField.Size - 1].Value == dotValue && GameField[1][GameField.Size - 1].Active);
                            continue;
                        }
                        if (i == GameField.Size - 1 && j == 0)
                        {
                            GameField[i][j].Closed = (GameField[GameField.Size - 1][1].Value == dotValue && GameField[GameField.Size - 1][1].Active &&
                                                  GameField[GameField.Size - 2][0].Value == dotValue && GameField[GameField.Size - 2][0].Active);
                            continue;
                        }
                        if (i == GameField.Size - 1 && j == GameField.Size - 1)
                        {
                            GameField[i][j].Closed = (GameField[GameField.Size - 1][GameField.Size - 2].Value == dotValue && GameField[GameField.Size - 1][GameField.Size - 2].Active &&
                                                  GameField[GameField.Size - 2][GameField.Size - 1].Value == dotValue && GameField[GameField.Size - 2][GameField.Size - 1].Active);
                            continue;
                        }

                        // Check sides
                        // Top and bottom rows
                        if (i == 0 || i == GameField.Size - 1)
                        {
                            bool closedSide = false;
                            int begin = 0, end = 0, step = 0;

                            if (i == 0)
                                { begin = 1; end = GameField.Size - 1; step = 1; }
                            else
                                { begin = GameField.Size - 2; end = 0; step = -1; }

                            for (int k = begin; (i == 0 ? k <= end : k >= end); k += step)
                            {
                                if (GameField[k][j].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == GameField.Size - 1) &&
                                    GameField[k][j - 1].Value == dotValue && GameField[k][j - 1].Active &&
                                    GameField[k][j + 1].Value == dotValue && GameField[k][j + 1].Active)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            GameField[i][j].Closed = (GameField[i][j - 1].Value == dotValue && GameField[i][j - 1].Active &&
                                                  GameField[i][j + 1].Value == dotValue && GameField[i][j + 1].Active &&
                                                  closedSide);
                        }
                        //Left and right columns
                        if (j == 0 || j == GameField.Size - 1)
                        {
                            bool closedSide = false;
                            int begin = 0, end = 0, step = 0;

                            if (j == 0)
                                { begin = 1; end = GameField.Size - 1; step = 1; }
                            else
                                { begin = GameField.Size - 2; end = 0; step = -1; }

                            for (int k = begin; (j == 0 ? k <= end : k >= end); k += step)
                            {
                                if (GameField[i][k].Value == dotValue)
                                {
                                    closedSide = true;
                                    break;
                                }
                                if ((k == 0 || k == GameField.Size - 1) &&
                                    GameField[i - 1][k].Value == dotValue && GameField[i - 1][k].Active &&
                                    GameField[i + 1][k].Value == dotValue && GameField[i + 1][k].Active)
                                {
                                    closedSide = true;
                                    break;
                                }
                            }

                            GameField[i][j].Closed = (GameField[i - 1][j].Value == dotValue && GameField[i - 1][j].Active &&
                                                  GameField[i + 1][j].Value == dotValue && GameField[i + 1][j].Active &&
                                                  closedSide);
                        }
                    }
                }

            // Determine closed dots
            for (int i = 1; i < GameField.Size - 1; i++)
                for (int j = 1; j < GameField.Size - 1; j++)
                {
                    if (GameField[i][j].Value == dotValue) continue;

                    // Top
                    bool topClosed = false;
                    for (int k = i - 1; k >= 0; k--)
                        if ((GameField[k][j].Value == dotValue && GameField[k][j].Active) ||
                            (k == 0 && GameField[k][j].Closed))
                        {
                            topClosed = true;
                            break;
                        }
                    // Bottom
                    bool bottomClosed = false;
                    for (int k = i + 1; k < GameField.Size; k++)
                        if ((GameField[k][j].Value == dotValue && GameField[k][j].Active) ||
                            (k == GameField.Size - 1 && GameField[k][j].Closed))
                        {
                            bottomClosed = true;
                            break;
                        }
                    // Left
                    bool leftClosed = false;
                    for (int k = j - 1; k >= 0; k--)
                        if ((GameField[i][k].Value == dotValue && GameField[i][k].Active) ||
                            (k == 0 && GameField[i][k].Closed))
                        {
                            leftClosed = true;
                            break;
                        }
                    // Right
                    bool rightClosed = false;
                    for (int k = j + 1; k < GameField.Size; k++)
                        if ((GameField[i][k].Value == dotValue && GameField[i][k].Active) ||
                            (k == GameField.Size - 1 && GameField[i][k].Closed))
                        {
                            rightClosed = true;
                            break;
                        }
                    GameField[i][j].Closed = (topClosed && bottomClosed && leftClosed && rightClosed);
                }
        }

        private bool FindChain(int i, int j, byte dotValue, ref bool isEficientChain)
        {
            GameField[i][j].Active = GameField[i][j].Value == dotValue;
            GameField[i][j].Chain = false;

            if (!isEficientChain)
                isEficientChain = GameField[i][j].Value != 0 && GameField[i][j].Value != dotValue;

            // Top
            bool topChain = true;
            if (i != 0)
            {
                if (GameField[i - 1][j].Value != dotValue && !GameField[i - 1][j].Closed)
                    return false;
                if (GameField[i - 1][j].Value == dotValue && GameField[i - 1][j].Active)
                    GameField[i - 1][j].Chain = true;
                if (!GameField[i - 1][j].Active && GameField[i - 1][j].Value == dotValue)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
                if (GameField[i - 1][j].Closed && GameField[i - 1][j].Active)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
            }

            // Bottom
            bool bottomChain = true;
            if (i != GameField.Size - 1)
            {
                if (GameField[i + 1][j].Value != dotValue && !GameField[i + 1][j].Closed)
                    return false;
                if (GameField[i + 1][j].Value == dotValue && GameField[i + 1][j].Active)
                    GameField[i + 1][j].Chain = true;
                if (!GameField[i + 1][j].Active && GameField[i + 1][j].Value == dotValue)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain);
                if (GameField[i + 1][j].Closed && GameField[i + 1][j].Active)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain); 
            }

            // Left
            bool leftChain = true;
            if (j != 0)
            {
                if (GameField[i][j - 1].Value != dotValue && !GameField[i][j - 1].Closed)
                    return false;
                if (GameField[i][j - 1].Value == dotValue && GameField[i][j - 1].Active)
                    GameField[i][j - 1].Chain = true;
                if (!GameField[i][j - 1].Active && GameField[i][j - 1].Value == dotValue)
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain);
                if (GameField[i][j - 1].Closed && GameField[i][j - 1].Active)
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain); 
            }

            // Right
            bool rightChain = true;
            if (j != GameField.Size - 1)
            {
                if (GameField[i][j + 1].Value != dotValue && !GameField[i][j + 1].Closed)
                    return false;
                if (GameField[i][j + 1].Value == dotValue && GameField[i][j + 1].Active)
                    GameField[i][j + 1].Chain = true;
                if (!GameField[i][j + 1].Active && GameField[i][j + 1].Value == dotValue)
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain);
                if (GameField[i][j + 1].Closed && GameField[i][j + 1].Active)
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain); 
            }

            return topChain && bottomChain && leftChain && rightChain;
        }
    }
}
