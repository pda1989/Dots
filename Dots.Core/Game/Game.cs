using System;
using Dots.Core.Field;

namespace Dots.Core.Game
{
    public class Game
    {
        #region Fields

        private const byte FirstPlayerDot = 1;
        private const byte SecondPlayerDot = 2;

        private Field.Field _gameField;

        #endregion

        #region Delegates and Events

        public event Action<Field.Field> OnFieldChanged;

        #endregion

        #region Properties

        public bool FirstPlayerMove { get; private set; }

        public GameResult Result
        {
            get
            {
                var secondPlayerScore = 0;
                var firstPlayerScore = 0;
                for (var i = 0; i < _gameField.Size; i++)
                for (var j = 0; j < _gameField.Size; j++)
                {
                    Dot dot = _gameField[i][j];
                    switch (dot.Value)
                    {
                        case FirstPlayerDot when !dot.Active:
                            secondPlayerScore++;
                            break;
                        case SecondPlayerDot when !dot.Active:
                            firstPlayerScore++;
                            break;
                    }
                }

                return new GameResult
                {
                    FirstPlayerScore = firstPlayerScore,
                    SecondPlayerScore = secondPlayerScore
                };
            }
        }

        #endregion

        #region Methods

        public void Initialyze(int fieldSize)
        {
            _gameField = new Field.Field(fieldSize);

            FirstPlayerMove = true;

            FieldChanged();
        }

        public void MakeMove(int i, int j)
        {
            if (_gameField == null)
                throw new ArgumentException("The field is not initialized");

            if (i < 0 || i >= _gameField.Size)
                throw new ArgumentOutOfRangeException(nameof(i));
            if (j < 0 || j >= _gameField.Size)
                throw new ArgumentOutOfRangeException(nameof(j));

            if (_gameField[i][j].Value == 0)
            {
                _gameField[i][j].Value = FirstPlayerMove ? FirstPlayerDot : SecondPlayerDot;
                if (_gameField[i][j].Value == _gameField[i][j].ChainValue)
                    _gameField[i][j].Active = true;
                _gameField[i][j].Chain = false;
                _gameField[i][j].Closed = false;
                FinishMove();
            }
            else
            {
                throw new ArgumentException("The cell is not empty");
            }
        }

        private void CalculateClosedDots(byte dotValue)
        {
            // Determine closed dots on sides
            for (var i = 0; i < _gameField.Size; i++)
            for (var j = 0; j < _gameField.Size; j++)
            {
                if (_gameField[i][j].Value == dotValue)
                {
                    _gameField[i][j].Closed = false;
                    continue;
                }

                if (i == 0 || i == _gameField.Size - 1 || j == 0 || j == _gameField.Size - 1)
                {
                    // Check corners
                    if (i == 0 && j == 0)
                    {
                        _gameField[i][j].Closed =
                            _gameField[0][1].Value == dotValue &&
                            _gameField[0][1].Active &&
                            _gameField[1][0].Value == dotValue &&
                            _gameField[1][0].Active;
                        continue;
                    }

                    if (i == 0 && j == _gameField.Size - 1)
                    {
                        _gameField[i][j].Closed =
                            _gameField[0][_gameField.Size - 2].Value == dotValue &&
                            _gameField[0][_gameField.Size - 2].Active &&
                            _gameField[1][_gameField.Size - 1].Value == dotValue &&
                            _gameField[1][_gameField.Size - 1].Active;
                        continue;
                    }

                    if (i == _gameField.Size - 1 && j == 0)
                    {
                        _gameField[i][j].Closed =
                            _gameField[_gameField.Size - 1][1].Value == dotValue &&
                            _gameField[_gameField.Size - 1][1].Active &&
                            _gameField[_gameField.Size - 2][0].Value == dotValue &&
                            _gameField[_gameField.Size - 2][0].Active;
                        continue;
                    }

                    if (i == _gameField.Size - 1 && j == _gameField.Size - 1)
                    {
                        _gameField[i][j].Closed =
                            _gameField[_gameField.Size - 1][_gameField.Size - 2].Value == dotValue &&
                            _gameField[_gameField.Size - 1][_gameField.Size - 2].Active &&
                            _gameField[_gameField.Size - 2][_gameField.Size - 1].Value == dotValue &&
                            _gameField[_gameField.Size - 2][_gameField.Size - 1].Active;
                        continue;
                    }

                    // Check sides
                    // Top and bottom rows
                    if (i == 0 || i == _gameField.Size - 1)
                    {
                        var closedSide = false;
                        int begin, end, step;

                        if (i == 0)
                        {
                            begin = 1;
                            end = _gameField.Size - 1;
                            step = 1;
                        }
                        else
                        {
                            begin = _gameField.Size - 2;
                            end = 0;
                            step = -1;
                        }

                        for (int k = begin; i == 0 ? k <= end : k >= end; k += step)
                        {
                            if (_gameField[k][j].Value == dotValue)
                            {
                                closedSide = true;
                                break;
                            }

                            if ((k == 0 || k == _gameField.Size - 1) &&
                                _gameField[k][j - 1].Value == dotValue && _gameField[k][j - 1].Active &&
                                _gameField[k][j + 1].Value == dotValue && _gameField[k][j + 1].Active)
                            {
                                closedSide = true;
                                break;
                            }
                        }

                        _gameField[i][j].Closed =
                            _gameField[i][j - 1].Value == dotValue && _gameField[i][j - 1].Active &&
                            _gameField[i][j + 1].Value == dotValue && _gameField[i][j + 1].Active &&
                            closedSide;
                    }

                    //Left and right columns
                    if (j == 0 || j == _gameField.Size - 1)
                    {
                        var closedSide = false;
                        int begin, end, step;

                        if (j == 0)
                        {
                            begin = 1;
                            end = _gameField.Size - 1;
                            step = 1;
                        }
                        else
                        {
                            begin = _gameField.Size - 2;
                            end = 0;
                            step = -1;
                        }

                        for (int k = begin; j == 0 ? k <= end : k >= end; k += step)
                        {
                            if (_gameField[i][k].Value == dotValue)
                            {
                                closedSide = true;
                                break;
                            }

                            if ((k == 0 || k == _gameField.Size - 1) &&
                                _gameField[i - 1][k].Value == dotValue && _gameField[i - 1][k].Active &&
                                _gameField[i + 1][k].Value == dotValue && _gameField[i + 1][k].Active)
                            {
                                closedSide = true;
                                break;
                            }
                        }

                        _gameField[i][j].Closed =
                            _gameField[i - 1][j].Value == dotValue && _gameField[i - 1][j].Active &&
                            _gameField[i + 1][j].Value == dotValue && _gameField[i + 1][j].Active &&
                            closedSide;
                    }
                }
            }

            // Determine closed dots
            for (var i = 1; i < _gameField.Size - 1; i++)
            for (var j = 1; j < _gameField.Size - 1; j++)
            {
                if (_gameField[i][j].Value == dotValue) continue;

                // Top
                var topClosed = false;
                for (int k = i - 1; k >= 0; k--)
                    if (_gameField[k][j].Value == dotValue && _gameField[k][j].Active ||
                        k == 0 && _gameField[k][j].Closed)
                    {
                        topClosed = true;
                        break;
                    }

                // Bottom
                var bottomClosed = false;
                for (int k = i + 1; k < _gameField.Size; k++)
                    if (_gameField[k][j].Value == dotValue && _gameField[k][j].Active ||
                        k == _gameField.Size - 1 && _gameField[k][j].Closed)
                    {
                        bottomClosed = true;
                        break;
                    }

                // Left
                var leftClosed = false;
                for (int k = j - 1; k >= 0; k--)
                    if (_gameField[i][k].Value == dotValue && _gameField[i][k].Active ||
                        k == 0 && _gameField[i][k].Closed)
                    {
                        leftClosed = true;
                        break;
                    }

                // Right
                var rightClosed = false;
                for (int k = j + 1; k < _gameField.Size; k++)
                    if (_gameField[i][k].Value == dotValue && _gameField[i][k].Active ||
                        k == _gameField.Size - 1 && _gameField[i][k].Closed)
                    {
                        rightClosed = true;
                        break;
                    }

                _gameField[i][j].Closed = topClosed && bottomClosed && leftClosed && rightClosed;
            }
        }

        private void CheckChains(byte priorityDot)
        {
            CalculateClosedDots(priorityDot);

            for (var i = 0; i < _gameField.Size; i++)
            for (var j = 0; j < _gameField.Size; j++)
                if (_gameField[i][j].Closed && _gameField[i][j].Active)
                {
                    Field.Field oldField = _gameField.Clone();
                    var isEficientChain = false;
                    bool isChain = FindChain(i, j, priorityDot, ref isEficientChain);
                    if (!isChain || !isEficientChain)
                        _gameField = oldField;
                }
        }

        private bool FindChain(int i, int j, byte dotValue, ref bool isEficientChain)
        {
            _gameField[i][j].Active = _gameField[i][j].Value == dotValue;
            _gameField[i][j].ChainValue = !_gameField[i][j].Active ? dotValue : (byte) 0;
            _gameField[i][j].Chain = false;

            if (!isEficientChain)
                isEficientChain = _gameField[i][j].Value != 0 && _gameField[i][j].Value != dotValue;

            // Top
            var topChain = true;
            if (i != 0)
            {
                if (_gameField[i - 1][j].Value != dotValue && !_gameField[i - 1][j].Closed)
                    return false;
                if (_gameField[i - 1][j].Value == dotValue && _gameField[i - 1][j].Active)
                    _gameField[i - 1][j].Chain = true;
                if (!_gameField[i - 1][j].Active && _gameField[i - 1][j].Value == dotValue)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
                if (_gameField[i - 1][j].Closed && _gameField[i - 1][j].Active)
                    topChain = FindChain(i - 1, j, dotValue, ref isEficientChain);
            }

            // Bottom
            var bottomChain = true;
            if (i != _gameField.Size - 1)
            {
                if (_gameField[i + 1][j].Value != dotValue && !_gameField[i + 1][j].Closed)
                    return false;
                if (_gameField[i + 1][j].Value == dotValue && _gameField[i + 1][j].Active)
                    _gameField[i + 1][j].Chain = true;
                if (!_gameField[i + 1][j].Active && _gameField[i + 1][j].Value == dotValue)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain);
                if (_gameField[i + 1][j].Closed && _gameField[i + 1][j].Active)
                    bottomChain = FindChain(i + 1, j, dotValue, ref isEficientChain);
            }

            // Left
            var leftChain = true;
            if (j != 0)
            {
                if (_gameField[i][j - 1].Value != dotValue && !_gameField[i][j - 1].Closed)
                    return false;
                if (_gameField[i][j - 1].Value == dotValue && _gameField[i][j - 1].Active)
                    _gameField[i][j - 1].Chain = true;
                if (!_gameField[i][j - 1].Active && _gameField[i][j - 1].Value == dotValue)
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain);
                if (_gameField[i][j - 1].Closed && _gameField[i][j - 1].Active)
                    leftChain = FindChain(i, j - 1, dotValue, ref isEficientChain);
            }

            // Right
            var rightChain = true;
            if (j != _gameField.Size - 1)
            {
                if (_gameField[i][j + 1].Value != dotValue && !_gameField[i][j + 1].Closed)
                    return false;
                if (_gameField[i][j + 1].Value == dotValue && _gameField[i][j + 1].Active)
                    _gameField[i][j + 1].Chain = true;
                if (!_gameField[i][j + 1].Active && _gameField[i][j + 1].Value == dotValue)
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain);
                if (_gameField[i][j + 1].Closed && _gameField[i][j + 1].Active)
                    rightChain = FindChain(i, j + 1, dotValue, ref isEficientChain);
            }

            return topChain && bottomChain && leftChain && rightChain;
        }

        private void FinishMove()
        {
            CheckChains(FirstPlayerMove ? FirstPlayerDot : SecondPlayerDot);
            CheckChains(FirstPlayerMove ? SecondPlayerDot : FirstPlayerDot);

            FirstPlayerMove = !FirstPlayerMove;

            FieldChanged();
        }

        protected virtual void FieldChanged()
        {
            OnFieldChanged?.Invoke(_gameField.Clone());
        }

        #endregion
    }
}