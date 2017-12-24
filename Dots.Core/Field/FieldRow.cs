using System;
using System.Collections.Generic;

namespace Dots.Core.Field
{
    public class FieldRow
    {
        #region Fields

        private readonly List<Dot> _dots;

        #endregion

        #region Constructors

        public FieldRow(int rowSize)
        {
            if (rowSize <= 0)
                throw new ArgumentException("Invalid row size");

            _dots = new List<Dot>();
            for (var i = 0; i < rowSize; i++)
                _dots.Add(new Dot());
        }

        #endregion

        #region Properties

        public Dot this[int i]
        {
            get
            {
                if (i < 0 || i >= _dots.Count)
                    throw new IndexOutOfRangeException();
                return _dots[i];
            }
            set
            {
                if (i < 0 || i >= _dots.Count)
                    throw new IndexOutOfRangeException();
                _dots[i] = value;
            }
        }

        #endregion
    }
}