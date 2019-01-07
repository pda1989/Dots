using System;

namespace Dots.Core.Field.Models
{
    public partial class FieldRow
    {
        public Dot this[int i]
        {
            get
            {
                if (i < 0 || i >= _dots.Count)
                    throw new ArgumentOutOfRangeException();
                return _dots[i];
            }

            set
            {
                if (i < 0 || i >= _dots.Count)
                    throw new ArgumentOutOfRangeException();
                _dots[i] = value;
            }
        }
    }
}