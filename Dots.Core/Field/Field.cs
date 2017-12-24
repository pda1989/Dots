using System;
using System.Collections.Generic;

namespace Dots.Core.Field
{
    public class Field
    {
        #region Fields

        private readonly List<FieldRow> _rows;

        #endregion

        #region Constructors

        public Field(int fieldSize)
        {
            if (fieldSize <= 0)
                throw new ArgumentException("Invalid field size");

            Size = fieldSize;

            _rows = new List<FieldRow>();
            for (var i = 0; i < Size; i++)
                _rows.Add(new FieldRow(Size));
        }

        #endregion

        #region Properties

        public int Size { get; }

        public FieldRow this[int i]
        {
            get
            {
                if (i < 0 || i >= _rows.Count)
                    throw new IndexOutOfRangeException();
                return _rows[i];
            }
        }

        #endregion

        #region Methods

        public Field Clone()
        {
            var newField = new Field(Size);

            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                newField[i][j] = _rows[i][j].Clone();

            return newField;
        }

        #endregion
    }
}