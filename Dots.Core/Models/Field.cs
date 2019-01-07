using System;
using System.Collections.Generic;

namespace Dots.Core.Field.Models
{
    public partial class Field
    {
        private readonly List<FieldRow> _rows;

        public Field(int fieldSize)
        {
            if (fieldSize <= 0)
                throw new ArgumentException("Invalid field size");

            Size = fieldSize;

            _rows = new List<FieldRow>();
            for (var i = 0; i < Size; i++)
                _rows.Add(new FieldRow(Size));
        }

        public int Size { get; }
    }
}