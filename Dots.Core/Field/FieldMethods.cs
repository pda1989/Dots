using System;

namespace Dots.Core.Field.Models
{
    public partial class Field
    {
        public FieldRow this[int i]
        {
            get
            {
                if (i < 0 || i >= _rows.Count)
                    throw new ArgumentOutOfRangeException();
                return _rows[i];
            }
        }

        public Field Clone()
        {
            var newField = new Field(Size);

            for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                    newField[i][j] = _rows[i][j].Clone();

            return newField;
        }
    }
}