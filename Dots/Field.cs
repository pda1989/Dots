using System;
using System.Collections.Generic;

namespace Dots
{
    public class Field
    {
        public class FieldRow
        {
            private List<Dot> _values;

            public Dot this[int i]
            {
                get
                {
                    if (i < 0 || i >= _values.Count)
                        throw new IndexOutOfRangeException();
                    return _values[i];
                }
                set
                {
                    if (i < 0 || i >= _values.Count)
                        throw new IndexOutOfRangeException();
                    _values[i] = value;
                }
            }

            public FieldRow(int rowSize)
            {
                if (rowSize <= 0)
                    throw new ArgumentException("Invalid row size");

                _values = new List<Dot>();
                for (int i = 0; i < rowSize; i++)
                    _values.Add(new Dot());
            }
        }

        private List<FieldRow> _rows;

        public int Size { get; private set; }

        public FieldRow this[int i]
        {
            get
            {
                if (i < 0 || i >= _rows.Count)
                    throw new IndexOutOfRangeException();
                return _rows[i];
            }
            set
            {
                if (i < 0 || i >= _rows.Count)
                    throw new IndexOutOfRangeException();
                _rows[i] = value;
            }
        }

        public Field(int fieldSize)
        {
            if (fieldSize <= 0)
                throw new ArgumentException("Invalid field size");

            Size = fieldSize;

            _rows = new List<FieldRow>();
            for (int i = 0; i < Size; i++)
                _rows.Add(new FieldRow(Size));
        }

        public Field Clone()
        {
            var newField = new Field(Size);
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    newField[i][j] = _rows[i][j].Clone();
            return newField;
        }
    }
}
