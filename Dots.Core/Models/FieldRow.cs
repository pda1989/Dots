using System;
using System.Collections.Generic;

namespace Dots.Core.Field.Models
{
    public partial class FieldRow
    {
        private readonly List<Dot> _dots;

        public FieldRow(int rowSize)
        {
            if (rowSize <= 0)
                throw new ArgumentException("Invalid row size");

            _dots = new List<Dot>();
            for (var i = 0; i < rowSize; i++)
                _dots.Add(new Dot());
        }
    }
}