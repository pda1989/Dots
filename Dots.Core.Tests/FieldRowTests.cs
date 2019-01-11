using Dots.Core.Field.Models;
using NUnit.Framework;
using System;

namespace Dots.Tests
{
    [TestFixture]
    public class FieldRowTests
    {
        [Test]
        public void FieldRow_WithSize2_Initializes2EmptyDots()
        {
            var fieldRow = new FieldRow(2);

            Assume.That(fieldRow[0], Is.Not.Null);
            Assume.That(fieldRow[1], Is.Not.Null);
            Assume.That(fieldRow[0], Is.EqualTo(fieldRow[1]));
        }

        [Test]
        public void FieldRow_WithWrongSize_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new FieldRow(-1));
        }

        [Test]
        public void ThisRead_WithWrongIndex_ThrowsArgumentOutOfRangeException()
        {
            var fieldRow = new FieldRow(2);

            Assert.Throws<ArgumentOutOfRangeException>(() => { var dot = fieldRow[-1]; });
        }

        [Test]
        public void ThisWrite_WithRightIndex_InitializesDot()
        {
            var fieldRow = new FieldRow(2);

            fieldRow[0] = new Dot { Value = 10 };

            Assume.That(fieldRow[0].Value, Is.EqualTo(10));
        }

        [Test]
        public void ThisWrite_WithWrongIndex_ThrowsArgumentOutOfRangeExceptionException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FieldRow(2) { [-1] = new Dot() });
        }
    }
}