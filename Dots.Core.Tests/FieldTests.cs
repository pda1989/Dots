using Dots.Core.Field.Models;
using NUnit.Framework;
using System;

namespace Dots.Tests
{
    [TestFixture]
    public class FieldTests
    {
        [Test]
        public void Clone_Field_ReturnsFieldWithSameValues()
        {
            var field = new Field(2);

            var newField = field.Clone();

            Assume.That(field[0][0].Value, Is.EqualTo(newField[0][0].Value));
            Assume.That(field[0][1].Value, Is.EqualTo(newField[0][1].Value));
            Assume.That(field[1][0].Value, Is.EqualTo(newField[1][0].Value));
            Assume.That(field[1][1].Value, Is.EqualTo(newField[1][1].Value));
        }

        [Test]
        public void Field_WithWrongSize_THrowsArgumentExceptionException()
        {
            Assert.Throws<ArgumentException>(() => new Field(-1));
        }

        [Test]
        public void ThisRead_WithWrongIndex_ThrowsArgumentOutOfRangeExceptionException()
        {
            var field = new Field(2);

            Assert.Throws<ArgumentOutOfRangeException>(() => { Dot dot = field[-1][0]; });
        }
    }
}